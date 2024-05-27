using System.Collections.Generic;
using Qs.Comm;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuPhoneCode : BaseReqPage
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