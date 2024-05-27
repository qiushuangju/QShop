using System.Collections.Generic;
using Qs.Repository.Base;
using Qs.Comm;
namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuUserDrawMoneyLog : BaseReqPage
    {
        /// <summary>
        /// id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
        /// <summary>
        /// 开始日期
        /// </summary>
        public System.DateTime? StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public System.DateTime? EndDate { get; set; }
        /// <summary>
        /// 验证
        /// </summary>
        public  void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>(){new ValueTip("字段","工地Id") });
        }
    }
}