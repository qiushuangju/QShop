using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuSysArea : BaseReqPage
    {
        
        /// <summary>
        /// 父id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 最小等级
        /// </summary>
        public int? MaxLevel { get; set; }
    }
}