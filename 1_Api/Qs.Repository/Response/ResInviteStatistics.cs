using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
  public class ResInviteStatistics
    {
        /// <summary>
        /// 已注册会员数
        /// </summary>
        public int CountReg { get; set; }
        /// <summary>
        /// 已充会员数
        /// </summary>
        public int CountVip { get; set; }
    }
}
