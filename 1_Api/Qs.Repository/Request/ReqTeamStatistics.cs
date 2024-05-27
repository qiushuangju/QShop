using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    public class ReqTeamStatistics : BaseReqPage
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StrStartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public string StrEndDate { get; set; }
    }
}
