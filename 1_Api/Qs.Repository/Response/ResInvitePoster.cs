using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Model.ModelRes
{
    /// <summary>
    /// 邀请海报
    /// </summary>
    public class ResInviteLink
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 二维码海报地址
        /// </summary>
        public string UrlPoster { get; set; }
    }
}
