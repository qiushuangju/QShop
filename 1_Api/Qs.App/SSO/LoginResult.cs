using Qs.Comm;    

namespace Qs.App.SSO
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginResult :Response<string>
    {
        /// <summary>
        /// 用户Id/IM用户Id
        /// </summary>
        public string UserId;
        /// <summary>
        /// 注册跳转路径
        /// </summary>
        public string ReturnUrl;
        /// <summary>
        /// Token
        /// </summary>
        public string Token;
        /// <summary>
        /// 默认昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 用户类型( 10:用户 15:会员 20:司机 30:调度 40:业务)
        /// </summary>
        public int UserType { get; set; }

        /// <summary>
        ///状态 10:正常 -10:禁用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否已绑定手机
        /// </summary>
        public bool IsBindPhone { get; set; }
        /// <summary>
        /// 是否已设置支付密码
        /// </summary>
        public bool IsHavePayPwd { get; set; }

        /// <summary>
        /// 默认头像
        /// </summary>
        public string UrlAvatar { get; set; }

    }
}