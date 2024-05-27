using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 账户注册设置
    /// </summary>
    public class VmSettingRegister
    {
        /// <summary>
        /// 微信小程序是否一键登录
        /// </summary>
        public int IsOauthMpweixin { get; set; } = 1;
        /// <summary>
        /// 微信小程序 是否强制绑定手机号
        /// </summary>
        public int IsForceBindMpweixin { get; set; } = 1;

        /// <summary>
        /// 注册方式  10:手机号+短信验证码
        /// </summary>
        public int RegisterMethod { get; set; } = 10;
        /// <summary>
        /// 手动绑定
        /// </summary>
        public int isManualBind { get; set; } = 1;
    }
}
