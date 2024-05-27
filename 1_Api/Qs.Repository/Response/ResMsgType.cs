using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Response
{
    public class ResMsgSubType 
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 未读数量
        /// </summary>
        public int CountMsg { get; set; }
    }
}
