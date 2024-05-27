using Newtonsoft.Json;

namespace Common.Request.Electronic
{
    public class ManInfo
    {
        /**
     * 收件人姓名 (必填)
     */
    public string name {get; set;}
    /**
     * 收件人的手机号，手机号和电话号二者其一必填 (必填)
     */
    public string mobile {get; set;}
    /**
     * 收件人所在完整地址 (必填)
     */
    public string printAddr {get; set;}
    /**
     * 收件人所在公司名称(可选)
     */
    public string company {get; set;}

     public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}