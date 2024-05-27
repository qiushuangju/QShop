using Qs.Comm;
using Qs.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class ResRechargeOrder : ModelRechargeOrder
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 套餐
        /// </summary>
        public ModelRechargePlan PlanInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static ResRechargeOrder ToView(ModelRechargeOrder order, ModelUser user,  ModelRechargePlan plan)
        {
            ResRechargeOrder vm = xConv.CopyMapper<ResRechargeOrder, ModelRechargeOrder>(order);
            vm.UserInfo = user == null ? new UserInfo() : new UserInfo() { Account = user.Account, NickName = user.NickName, Phone = user.Phone };
            vm.PlanInfo = plan==null?new ModelRechargePlan(): plan;
            return vm;
        }
    }
}
