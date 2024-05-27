using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Comm
{
    /// <summary>
    ///错误码
    /// </summary>
    public static class DefineErrCode
    {
        /// <summary>
        /// 认证失败
        /// </summary>
        public const int InvalidToken = 50014;
        /// <summary>
        ///用户未审核或禁用
        /// </summary>
        public const int InvalidUserStatus = 50015;
        /// <summary>
        /// 余额密码为空
        /// </summary>
        public const int PayPwdIsNull = 5001;
    }
}
