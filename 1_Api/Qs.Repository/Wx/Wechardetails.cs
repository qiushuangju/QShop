using System;
using System.Collections.Generic;
using System.Text;

namespace Qs.App.Request.ReqApi
{
    //获取用户手机号
    public class Wechardetails<T>
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string phoneNumber { get; set; }
        /// <summary>
        /// 区域手机号
        /// </summary>
        public string purePhoneNumber { get; set; }
        /// <summary>
        /// 区码
        /// </summary>
        public string countryCode { get; set; }

        public T watermark { get; set; }
    }

}
