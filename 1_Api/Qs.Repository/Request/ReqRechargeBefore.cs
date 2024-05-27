using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public  class ReqRechargeBefore
    {
        /// <summary>
        /// 充值套餐Id
        /// </summary>
        public string PlanId { get; set; }
        /// <summary>
        /// 充值金额自定义
        /// </summary>
        public decimal? CustomMoney { get; set; }
    }
}
