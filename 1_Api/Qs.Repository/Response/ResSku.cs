using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
   public class ResGoodSku: ModelGoodsSku
   {
        /// <summary>
        /// 商品规格(10单规格 20多规格)
        /// </summary>
        public int SpecType { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string GoodsNo { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// Sku主图
        /// </summary>
        public string SkuUrlImage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="goods"></param>
        /// <param name="listFile"></param>
        /// <returns></returns>
        public static ResGoodSku ToView(ModelGoodsSku sku, ModelGoods goods,List<ModelFileUpload> listFile)
        {
            ResGoodSku res = xConv.CopyMapper<ResGoodSku, ModelGoodsSku>(sku);
            res.SkuUrlImage = listFile == null ? "" : listFile.FirstOrDefault(p => p.Id == sku.ImageId)?.Thumbnail;
            res.GoodsName = goods.GoodsName;
            res.GoodsNo = goods.GoodsNo;
            res.SpecType =  goods.SpecType;
            return res;
        }
    }
}
