namespace Common.Request
{
    public class BaseReq<T>{

        /// <summary>
        /// 业务类型
        /// </summary>
        public string method {get; set;}
        /// <summary>
        /// 快递100分配给贵司的的授权key
        /// </summary>
        public string key {get; set;}
        /// <summary>
        /// 加密签名信息：MD5(param+t+key+secret)；加密后字符串转大写
        /// </summary>
        public string sign {get; set;}
        /// <summary>
        /// 当前请求时间戳
        /// </summary>
        public string t {get; set;}
        /// <summary>
        /// 其他参数
        /// </summary>
        public T param {get; set;}
    }
}