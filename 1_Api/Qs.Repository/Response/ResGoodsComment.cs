using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResGoodsComment:ModelGoodsComment
    {
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UrlAvater { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        ///  商品图片
        /// </summary>
        public string GoodsUrlImageMain { get; set; }
        /// <summary>
        ///  Sku名称
        /// </summary>
        public string SkuName { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public List<ModelFileUpload>  ListImg{ get; set; }

        public static ResGoodsComment ToView(ModelGoodsComment model,List<ModelUser> listUser, List<ModelFileUpload> listFile, List<ResGoods> listGoods ,List<ModelGoodsSku> listSku)
        {
            var sku = listSku.FirstOrDefault(p => p.Id == model.SkuId)??new ModelGoodsSku();
            var goods = listGoods.FirstOrDefault(p => p.Id == model.GoodsId) ?? new ResGoods();
            ResGoodsComment res=xConv.CopyMapper<ResGoodsComment, ModelGoodsComment>(model);
            var user = listUser.FirstOrDefault(p => p.Id == model.UserId);
            res.UrlAvater = user.UrlAvater;
            res.NickName = user.NickName;
            res.GoodsUrlImageMain = goods.UrlImageMain;
            res.GoodsName = goods.GoodsName;
            res.SkuName = sku.SkuName;
            res.ListImg = listFile.Where(p=>model.ImgIds.Contains(p.Id)).ToList();
            return res;
        }
    }
}
