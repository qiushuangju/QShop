using System.Collections.Generic;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuGoodsSku : BaseReqPage
    {
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }
           /// <summary>
           /// SkuId
           /// </summary>
        public List<string> ListSkuId { get;set; }
        /// <summary>
        /// 只查询当前商户
        /// </summary>
        public bool OnlyStore { get; set; } = true;
    }
}