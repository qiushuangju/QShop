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
    /// res
    /// </summary>
    public class ResStoreAddress:ModelStoreAddress
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string StrType { get; set; }         

        /// <summary>
        /// 转为Vm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResStoreAddress ToView(ModelStoreAddress model)
        {
            ResStoreAddress res = xConv.CopyMapper<ResStoreAddress, ModelStoreAddress>(model);
            res.StrType = xEnum.GetEnumDescription(typeof(xEnum.StoreAddressType), model.Type);
            return res;
        }
    }
}
