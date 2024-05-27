using System.Collections.Generic;
using Qs.Repository.Base;
using Qs.Comm;
namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuDeliveryRule : BaseReqPage
    {
       
        /// <summary>
        /// 验证
        /// </summary>
        public  void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>(){new ValueTip("字段","工地Id") });
        }
    }
}