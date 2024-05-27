using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 充值余额
    /// </summary>
    public  class ReqRechargeBalance
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId;
        /// <summary>
        /// 充值方式：inc:增加  dec:减少  final:最终金额
        /// </summary>
        public string Mode;
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money;
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark;

    }
}
