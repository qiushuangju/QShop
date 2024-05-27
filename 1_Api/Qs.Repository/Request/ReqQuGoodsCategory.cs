using System.Collections.Generic;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuGoodsCategory : BaseReqPage
    {
        /// <summary>
        /// 分类Id
        /// </summary>
        public List<string> ListCateId { get; set; }
       
        /// <summary>
        /// 是否显示(10:正常 -10:禁用)
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        ///分类层级
        /// </summary>
        public int? Level { get; set; }
    }
}