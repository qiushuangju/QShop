using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.App.ApiExpress.ApiKuaiDiNiao;
using Qs.Comm;

namespace Qs.App.ApiExpress
{
    /// <summary>
    /// 创建快递查询
    /// </summary>
    public class FactoryExpress
    {
        /// <summary>
        /// 创建快递查询
        /// </summary>
        /// <param name="expressName"></param>
        /// <returns></returns>
        public static IApiExpress CreateExpress(xEnum.ExpressName expressName)
        {
            switch (expressName)
            {
                case xEnum.ExpressName.Kd100:
                    return new ApiKuaiDi100.ApiKuaiDi100();
                case xEnum.ExpressName.Kdn:
                    return new ApiKdn();
                default:
                    throw new ArgumentException("快递查询工厂,expressName 错误.");
            }
        }
    }
}
