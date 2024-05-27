namespace  Common.Request.Electronic.Print
{
    public class PrintCloudReq
    {
         /**
     * 业务类型（默认：getPrintImg）
     */
    public string method {get; set;}
    /**
     * 快递100分配给贵司的的授权key
     */
    public string key {get; set;}
    /**
     * 加密签名信息：MD5(param+t+key+secret)；加密后字符串转大写
     */
    public string sign {get; set;}
    /**
     * 当前请求时间戳
     */
    public string t {get; set;}
    /**
     * 其他参数组合成的json对象
     */
    public PrintCloudParam param {get; set;}
    }
}