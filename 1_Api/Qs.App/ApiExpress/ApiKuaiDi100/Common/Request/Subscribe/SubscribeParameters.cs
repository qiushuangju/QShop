using Newtonsoft.Json;

namespace Common.Request.Subscribe
{
    public class SubscribeParameters
    {
    /**
     * 回调接口的地址。如果需要在推送信息回传自己业务参数，可以在回调地址URL后面拼接上去，如示例中的orderId
     * http://www.xxxxx.com/callback?orderId=123
     */
    public string callbackurl{get; set;}
    /**
     * 签名用随机字符串。32位自定义字符串。添加该参数，则推送的时候会增加sign给贵司校验消息的可靠性
     */
    public string salt{get; set;}
    /**
     * 添加此字段表示开通行政区域解析功能。0：关闭（默认），1：开通行政区域解析功能
     */
    public string resultv2{get; set;}
    /**
     * 添加此字段且将此值设为1，则表示开始智能判断单号所属公司的功能，
     * 开启后，company字段可为空,即只传运单号（number字段），我方收到后会根据单号判断出其所属的快递公司（即company字段）。
     * 建议只有在无法知道单号对应的快递公司（即company的值）的情况下才开启此功能。
     */
    public string autoCom{get; set;}
    /**
     * 添加此字段表示开启国际版
     * 开启后，若订阅的单号（即number字段）属于国际单号，会返回出发国与目的国两个国家的跟踪信息{get; set;}
     * 本功能暂时只支持邮政体系（国际类的邮政小包、EMS）内的快递公司{get; set;}
     * 若单号我方识别为非国际单，即使添加本字段，也不会返回destResult元素组.
     */
    public string interCom{get; set;}
    /**
     * 出发国家编码
     */
    public string departureCountry{get; set;}
    /**
     * 出发国家快递公司的编码
     */
    public string departureCom{get; set;}
    /**
     * 目的国家编码
     */
    public string destinationCountry{get; set;}
    /**
     * 目的国家快递公司的编码
     */
    public string destinationCom{get; set;}
    /**
     * 收件人或寄件人的手机号或固话（顺丰单号必填，也可以填写后四位，如果是固话，请不要上传分机号）
     */
    public string phone{get; set;}

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
    }
    }
}