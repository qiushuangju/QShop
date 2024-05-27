namespace  Common.Request.Sms
{
    public class SendSmsReq
    {
        /**
     * 加密签名信息：MD5(key + userid)；加密后字符串转大写
     */
    public string sign {get; set;}
    /**
     * 我方分配给贵司的的短信接口用户ID，点击查看账号信息
     */
    public string userid {get; set;}
    /**
     * 商户名称签名；最好用简称，该字段信息会在短信标签处显示。不要超过5个字符
     */
    public string seller {get; set;}
    /**
     * 接收短信手机号
     */
    public string phone {get; set;}
    /**
     * 短信模板ID
     */
    public string tid {get; set;}
    /**
     * 短信模板替换内容
     */
    public string content {get; set;}
    /**
     * 外部订单号：当该短信发送模板有回调地址时，外部订单号会返回给调用者，方便用户更新数据
     */
    public string outorder {get; set;}
    /**
     * 回调地址：如果客户在发送短信时填写该参数，将按照这个参数回调短信发送状态；
     * 如果为空，将按照模板配置的地址回调短信发送状态；如果两个参数都不填写，将不会回调通知状态
     */
    public string callback {get; set;}
    }
}