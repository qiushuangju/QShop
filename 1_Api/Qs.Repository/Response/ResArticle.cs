using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
   public class ResArticle : ModelArticle
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 图标地址
        /// </summary>
        public string UrlCover { get; set; }
    }
}
