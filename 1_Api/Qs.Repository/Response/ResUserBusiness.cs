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
    /// 业务员信息
    /// </summary>
    public class ResUserBusiness 
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string UrlAvater { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 微信二维码
        /// </summary>
        public string UrlWxQrCode { get; set; }

    }
}
