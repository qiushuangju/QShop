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
    ///余额详情
    /// </summary>
    public class ResUserBalanceLog : ModelUserBalanceLog
    {
        /// <summary>
        ///变动场景
        /// </summary>
        public string StrScene { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string UrlAvater { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResUserBalanceLog ToView(ModelUserBalanceLog model)
        {
            ResUserBalanceLog vm = xConv.CopyMapper<ResUserBalanceLog, ModelUserBalanceLog>(model);
            vm.StrScene = xEnum.GetEnumDescription(typeof(xEnum.BalanceType), model.Scene);
            return vm;
        }
    }
}
