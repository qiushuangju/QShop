using System.Collections.Generic;
using Qs.Repository.Base;
using Qs.Comm;
namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuOrderTrack : BaseReqPage
    {
        /// <summary>
        /// 是否只查自己记录 (默认否)
        /// </summary>
        public bool OnlyMy { get; set; } = false;
        /// <summary>
        /// 验证
        /// </summary>
        public  void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>(){new ValueTip("字段","工地Id") });
        }
    }
}