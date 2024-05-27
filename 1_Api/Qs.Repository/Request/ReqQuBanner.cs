using Qs.Comm;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuBanner : BaseReqPage
    {
        /// <summary>
        /// 是否可用(是:10 默认10)
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        public string StoreId { get; set; }
    }
}