using System;
using Qs.Comm;

namespace Qs.App.Request
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqWxJsParam
    {
        /// <summary>
        /// 
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Noncestr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long Timestamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Signature { get; set; }
    }
}