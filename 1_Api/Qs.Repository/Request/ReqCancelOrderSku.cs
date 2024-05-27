using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 取消订单
    /// </summary>
   public class ReqCancelOrderSku
    {
        /// <summary>
        /// OrderSkuId
        /// </summary>
        public string Id { get; set; }

        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(Id,"OrderSkuId")
            });
        }
    }
}
