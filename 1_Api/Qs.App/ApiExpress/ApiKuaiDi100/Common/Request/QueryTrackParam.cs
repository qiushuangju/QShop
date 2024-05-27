
using Newtonsoft.Json;

namespace Common.Request
{
    public class QueryTrackParam
    {
    /**
     * 查询的快递公司的编码，一律用小写字母
     */
    public string com  {get; set;}
    /**
     * 查询的快递单号， 单号的最大长度是32个字符
     */
    public string num {get; set;}
    /**
     * 收件人或寄件人的手机号或固话
     */
    public string phone {get; set;}
    /**
     * 出发地城市，省-市-区
     */
    public string from {get; set;}
    /**
     * 目的地城市，省-市-区
     */
    public string to {get; set;}
    /**
     * 添加此字段表示开通行政区域解析功能。0：关闭（默认），1：开通行政区域解析功能，2：开通行政解析功能并且返回出发、目的及当前城市信息
     */
    public string resultv2 {get; set;}
    /**
     * 返回数据格式。0：json（默认），1：xml，2：html，3：text
     */
    public string show {get; set;}
    /**
     * 返回结果排序方式。desc：降序（默认），asc：升序
     */
    public string order  {get; set;}

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
    }
    
    }
}