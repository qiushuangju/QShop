namespace Common.Request.Border
{
    public class BOrderCallBackReq{
        /// <summary>
        /// 任务id
        /// </summary>
        /// <value></value>
        public string taskId {get; set;}
        /// <summary>
        /// 签名	
        /// </summary>
        /// <value></value>
        public string sign {get; set;}
        /// <summary>
        /// 参数主体	
        /// </summary>
        /// <value></value>
        public BOrderCallBackParam param {get; set;}
    }
}