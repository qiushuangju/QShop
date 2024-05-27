using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 我的邀请统计
    /// </summary>
    public  class ResInviteLinkRecordStatistics
    {
        /// <summary>
        /// 已邀请
        /// </summary>
        public int InviteTotal { get; set; }

        /// <summary>
        /// 已办卡人数
        /// </summary>
        public int InviteOpenVipTotal { get; set; }
    }
}
