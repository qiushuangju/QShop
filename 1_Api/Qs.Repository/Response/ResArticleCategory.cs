using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResArticleCategory: ModelArticleCategory
    {
        /// <summary>
        /// 图标地址
        /// </summary>
        public string UrlIcon { get; set; }
    }
}
