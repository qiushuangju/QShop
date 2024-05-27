using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 分类
    /// </summary>
   public class ResGoodsCate :ModelGoodsCate
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        public string UrlImg { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string StoreName { get; set; }
    }
}
