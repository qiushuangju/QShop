using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResPoster:ModelInvitePoster
    {
        /// <summary>
        /// 缩略图
        /// </summary>
        public string UrlThumbnail { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public string UrlImg { get; set; }
    }
}
