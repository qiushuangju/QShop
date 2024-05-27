using Qs.Comm;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuUserAddress : BaseReqPage
    {
        /// <summary>
        /// 是否默认(<0:全部(默认),1:是,0:否)
        /// </summary>
        public int IsDefault { get; set; } = -1;
        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
    }
}