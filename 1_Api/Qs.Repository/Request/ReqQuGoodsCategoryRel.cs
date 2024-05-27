using System.Collections.Generic;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuGoodsCategoryRel : BaseReqPage
    {
        
        /// <summary>
        /// 商品Id
        /// </summary>
        public List<string> ListGoodsId { get; set; }
    }
}