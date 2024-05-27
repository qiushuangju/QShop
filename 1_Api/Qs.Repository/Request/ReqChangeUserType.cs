using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 修改密码(App)
    /// </summary>
   public class ReqChangeUserType
    {
        /// <summary>
        /// 用户Id
        /// </summary>
       public string Id { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 开户银行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 开户支行
        /// </summary>
        public string RegBank { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }
        
        /// <summary>
        /// 用户类型
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string FullAddress { get; set; }

        public void Check()
        {
            if (UserType <= (int)xEnum.UserType.SysAdmin)
            {
                throw new Exception("不可修改为管理员类型!");
            }
        }


    }
}
