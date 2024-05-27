using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuUserCoupon : BaseReqPage
    {
        

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 状态 (-20:已使用 -10:已过期 10:可使用
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
    }
}