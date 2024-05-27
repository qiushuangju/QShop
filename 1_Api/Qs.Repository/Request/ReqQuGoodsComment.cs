using System.Collections.Generic;
using Qs.Repository.Base;
using Qs.Comm;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuGoodsComment : BaseReqPage
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string GoodsId { get; set; }

        /// <summary>
        /// 评价类型 ((10:好评 20:中评 30:差评))
        /// </summary>
        public int? CommentType { get; set; }

        /// <summary>
        /// 是否显示隐藏数据
        /// </summary>
        public bool IsShowHide { get; set; } = false;

        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
           
        }
    }
}