using System.Collections.Generic;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuStoreSetting : BaseReqPage
    {
        /// <summary>
        /// 关键字集合
        /// </summary>
        public List<string> ListKey { get; set; }
    }
}