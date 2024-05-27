using System;

namespace Qs.App.Request.ReqApi
{
    /// <summary>
    ///     更新用户信息
    /// </summary>
    public class ReqBindPhone
    {
        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey { set; get; }
        /// <summary>
        /// 微信Code
        /// </summary>
        public string WxCode { set; get; }

        public void Trim()
        {
            if (string.IsNullOrEmpty(WxCode))
            {
                throw new Exception("参数WxCode,AppKey不能为空");
            }
        }

    }
}