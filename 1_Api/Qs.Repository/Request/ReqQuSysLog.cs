using Qs.Repository.Base;

namespace Qs.App.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuSysLog : BaseReqPage
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Type { get; set; }
    }
}