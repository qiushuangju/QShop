using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Vm;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 商品
    /// </summary>
    public class ResGoods : ModelGoods
    {
        /// <summary>
        /// 商城名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        ///  字符串状态 (10上架 -10下架)
        /// </summary>
        public string StrStatus { get; set; }
       

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        ///主图
        /// </summary>
        public string UrlImageMain { get; set; }

        /// <summary>
        /// 商品类型
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 销量(初始+实销)
        /// </summary>
        public int GoodsSales { get; set; }
        /// <summary>
        /// 服务承诺
        /// </summary>
        public List<string> ListServiceId { get; set; } 
        /// <summary>
        /// 主图列表
        /// </summary>
        public List<ModelFileUpload> ListImg { get; set; }
        /// <summary>
        /// 详情图列表
        /// </summary>
        public List<ModelFileUpload> ListImgDetail { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public List<SpecModel> SpecList { get; set; }

        /// <summary>
        /// Sku
        /// </summary>
        public List<ResSku> ListSku { get; set; }

        /// <summary>
        /// 促销信息
        /// </summary>
        public string Promotion { get; set; }

      


        /// <summary>
        /// Vm
        /// </summary>
        /// <param name="goods"></param>     
        /// <param name="listSpec"></param>
        /// <param name="listSpecValue"></param>
        /// <returns></returns>
        public static ResGoods ToView(ModelGoods goods,List<ModelFileUpload> listFile, List<ModelGoodsSku> listSku = null,
               List<ModelGoodsSpec> listSpec = null,List<ModelGoodsSpecValue> listSpecValue = null)
        {
            listSku = listSku ?? new List<ModelGoodsSku>();
            ResGoods vm = xConv.CopyMapper<ResGoods, ModelGoods>(goods);
            vm.StrStatus = xConv.ToString(goods.Status);
            vm.StatusName = xEnum.GetEnumDescription(typeof(xEnum.GoodsStatus), xConv.ToInt(goods.Status));

            listFile = listFile ?? new List<ModelFileUpload>();
            var listMainFileId = goods.ImageIdMains.Split(",").ToList();
            var listDetailFileId = goods.ImageIdDetails.Split(",").ToList();

            var mainFile = listFile.FirstOrDefault(p=>p.Id== listMainFileId.FirstOrDefault())??new ModelFileUpload();
            vm.UrlImageMain = mainFile.FilePath;
            vm.ListImg = listFile.Where(p => listMainFileId.Contains(p.Id)).ToList();
            vm.ListImgDetail = listFile.Where(p => listDetailFileId.Contains(p.Id)).ToList();
            vm.GoodsSales = vm.SalesActual + vm.SalesInitial;
            vm.CategoryId = goods.Cate2Id;
            vm.ListServiceId = xConv.ToListString(goods.ServiceIds);

            List<ResSku> listSkuVm = new List<ResSku>();
            foreach (var sku in listSku)
            {
                ResSku vmSku = xConv.CopyMapper<ResSku, ModelGoodsSku>(sku);
                vmSku.FileSkuImg = listFile.FirstOrDefault(p => p.Id == sku.ImageId);
                listSkuVm.Add(vmSku);
            }
            vm.ListSku = listSkuVm;                          

            vm.SpecList = new List<SpecModel>();
            foreach (var item in (listSpec ?? new List<ModelGoodsSpec>()).OrderBy(p => p.SortNo).ToList())
            {
                List<SpecValueModel> list = new List<SpecValueModel>();
                var listItemValue = listSpecValue.OrderBy(p => p.Key).Where(p => p.SpecId == item.Id).ToList();
                foreach (var itemValue in listItemValue)
                {
                    list.Add(new SpecValueModel()
                    {
                        SpecValueId = itemValue.Id,
                        Key = itemValue.Key,
                        GroupKey = xConv.ToInt(itemValue.GroupKey),
                        SpecValue = itemValue.SpecValue,
                    });
                }

                SpecModel sm = new SpecModel()
                {
                    Key = item.SortNo,
                    SpecId = item.Id,
                    SpecName = item.SpecName,
                    ValueList = list
                };
                vm.SpecList.Add(sm);
            }

            vm.Promotion = "";
            return vm;
        }
    }

    public class ResSku : ModelGoodsSku
    {
        /// <summary>
        /// Sku文件
        /// </summary>
        public ModelFileUpload FileSkuImg { get; set; }
    }
}

