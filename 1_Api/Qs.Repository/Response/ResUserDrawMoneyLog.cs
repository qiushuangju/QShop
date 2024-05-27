using Qs.Comm;
using Qs.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class ResUserDrawMoneyLog: ModelUserDrawMoneyLog
    {
        /// <summary>
        /// 用户名 
        /// </summary>
        public string UsereName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        ///   状态名
        /// </summary>
        public string StrStatus{ get; set; }
    }
}
