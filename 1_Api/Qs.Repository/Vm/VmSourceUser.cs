using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    public class VmSourceUser
    {
        /// <summary>
        ///归属父节点ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        ///归属路径
        /// </summary>
        public string SourceUserId { get; set; }
    }
}
