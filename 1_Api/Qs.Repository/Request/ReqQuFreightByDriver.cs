using System;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuFreightByDriver : ReqQuOrder
    {
        /// <summary>
        /// 司机查询
        /// </summary>
        public string KeyDriver { get; set; }

      
    }
}