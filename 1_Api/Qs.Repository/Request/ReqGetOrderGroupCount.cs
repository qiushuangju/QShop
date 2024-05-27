using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    
    public class ReqGetOrderGroupCount
    {
        /// <summary>
        /// 仅当前用户
        /// </summary>
        public bool OnlyMy { get; set; } = false;
    }

}
