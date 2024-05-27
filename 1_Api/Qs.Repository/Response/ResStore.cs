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
    /// Vm
    /// </summary>
   public class ResStore :ModelStore
    {
        /// <summary>
        /// Logo图
        /// </summary>
        public string LogoImageUrl {  get;set;}
        /// <summary>
        /// 状态
        /// </summary>
        public string StrStatus { get; set; }

        /// <summary>
        /// 转为Vm
        /// </summary>
        /// <param name="model"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ResStore ToView(ModelStore model,List<ModelFileUpload> list)
        {
            ResStore res = xConv.CopyMapper<ResStore, ModelStore>(model);
            var file = list.FirstOrDefault(p => p.Id == res.LogoImageId);
            res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.StoreStatus), model.Status);
            res.LogoImageUrl = file.Thumbnail;
            return res;
        }
    }
}
