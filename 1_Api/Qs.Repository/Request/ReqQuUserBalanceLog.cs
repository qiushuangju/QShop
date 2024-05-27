using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuUserBalanceLog : BaseReqPage
    {
        

        /// <summary>
        /// 余额变动场景(10用户充值 20用户消费 30管理员操作 40订单退款)
        /// </summary>
        public int Scene { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
    }
}