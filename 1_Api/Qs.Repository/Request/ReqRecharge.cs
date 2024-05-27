using System;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 充值
    /// </summary>
    public class ReqRecharge
    {
        /// <summary>
        /// AppUser (AppUser:用户端 AppBusiness:业务员 AppDriver:司机 AppDispatch:调度)
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 充值计划Id
        /// </summary>
        public decimal RechargeMoney { get; set; }

        public void Check()
        {
            if (xConv.ToDecimal(RechargeMoney)<=0)
            {
                throw new Exception("充值金额必须大于0");
            }
        }
    }
}
