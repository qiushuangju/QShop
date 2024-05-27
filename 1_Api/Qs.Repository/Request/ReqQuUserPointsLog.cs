using System.Collections.Generic;
using Qs.Repository.Base;
using Qs.Comm;
namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuUserPointsLog : BaseReqPage
    {
       
        /// <summary>
        /// 只查询当前商户
        /// </summary>
        public bool OnlyStore { get; set; } = true;
        /// <summary>
        /// 验证
        /// </summary>
        public  void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>(){new ValueTip("字段","工地Id") });
        }
    }
}