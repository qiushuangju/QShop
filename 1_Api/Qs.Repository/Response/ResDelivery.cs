using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Request;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 运费模板
    /// </summary>
   public  class ResDelivery  :ModelDelivery
    {   
        /// <summary>
        /// 计费方式
        /// </summary>
        public string StrMethod { get; set; }

        public List<ReqDeliveryRule> ListRule { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResDelivery ToView(ModelDelivery model)
        {
            ResDelivery res = xConv.CopyMapper<ResDelivery, ModelDelivery>(model);
            res.StrMethod = xEnum.GetEnumDescription(typeof(xEnum.DeliveryMethod), res.Method);
            res.ListRule = new List<ReqDeliveryRule>();
            return res;
        }
    }
}
