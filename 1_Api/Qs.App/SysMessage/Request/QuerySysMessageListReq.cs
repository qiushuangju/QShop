using Qs.App.Base;
using Qs.Repository.Base;

namespace Qs.App.Request
{
    public class QuerySysMessageListReq : BaseReqPage
    {
        /// <summary>
        /// 消息状态 0:全部;-10:未读;10：已读;
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 类型 0:全部;10:公告;20:消息;
        /// </summary>
        public int Type { get; set; }

    }
}