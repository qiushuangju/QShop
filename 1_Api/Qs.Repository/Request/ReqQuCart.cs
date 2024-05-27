using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuCart : BaseReqPage
    {
        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
        
    }
}