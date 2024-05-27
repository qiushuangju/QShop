using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    public class ReqMoveGroup
    {
        /// <summary>
        /// fileId
        /// </summary>
        public List<string> ListId { get; set; }
        /// <summary>
        /// 分组
        /// </summary>
        public string GroupId { get; set; }
    }
}
