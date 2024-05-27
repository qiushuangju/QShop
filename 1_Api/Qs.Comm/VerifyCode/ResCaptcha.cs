namespace Qs.Comm.VerifyCode
{
    /// <summary>
    /// 图形验证码
    /// </summary>
    public class ResCaptcha
    {
        /// <summary>
        /// 验证码Id
        /// </summary>
        public string VerifyCodeId { get; set; }

        /// <summary>
        /// Base64
        /// </summary>
        public string Base64Str { get; set; }
    }

    /// <summary>
    /// 图形验证码-带Code(内部使用)
    /// </summary>
    public class ResCaptchaCode : ResCaptcha
    {
        /// <summary>
        /// 验证码
        /// </summary>
        public string Code { get; set; }
    }

}
