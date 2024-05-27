using Newtonsoft.Json;

namespace Common.Request.Subscribe
{
    public class SubscribeParam
    {
         /**
     * 订阅的快递公司的编码，一律用小写字母
     */
    public string company  {get; set;}
    /**
     * 订阅的快递单号，单号的最大长度是32个字符
     */
    public string number  {get; set;}
    /**
     * 出发地城市，省-市-区，非必填，填了有助于提升签收状态的判断的准确率，请尽量提供
     */
    public string from  {get; set;}
    /**
     * 目的地城市，省-市-区，非必填，填了有助于提升签收状态的判断的准确率，且到达目的地后会加大监控频率，请尽量提供
     */
    public string to  {get; set;}
    /**
     * 我方分配给贵司的的授权key
     */
    public string key  {get; set;}
    /**
     * 附加参数信息
     */
    public SubscribeParameters parameters  {get; set;}

     public override string ToString()
    {
        return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
    }
    }
}