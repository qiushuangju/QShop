using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuArticle : BaseReqPage
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        public string CateId { get; set; }
    }
}