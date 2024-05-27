using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    public class ReqStatisticOperate : BaseReqPage
    {    
        /// <summary>
        /// 开始日期
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 父节点ID
        /// </summary>
        public string ParentUserId { get; set; }
    }
}
