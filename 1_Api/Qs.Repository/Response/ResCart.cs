using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
  public  class ResCart : ModelCart
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string UrlImg { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 转为Vm
        /// </summary>
        /// <param name="listFile">文件</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResCart ToView(List<ModelFileUpload> listFile,ModelCart model)
        {
            ResCart res = xConv.CopyMapper<ResCart, ModelCart>(model);
            res.UrlImg = listFile.FirstOrDefault(p => p.Id == model.ImageId)?.Thumbnail;
            return res;
        }
    }
}
