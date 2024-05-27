using System;
using System.Runtime.Serialization;

namespace Qs.Comm.Extensions
{
    /// <summary>
    /// 自定义异常
    /// </summary>
	[Serializable]
    public  class CustomException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public CustomException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nErrorCode"></param>
        /// <param name="message"></param>
        public CustomException(int nErrorCode, string message) : base(message)
        {
            ErrorCode = nErrorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nErrorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CustomException(int nErrorCode, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCode = nErrorCode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CustomException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// 错误编码
        /// </summary>
        public int ErrorCode { get; private set; }
    }

}
