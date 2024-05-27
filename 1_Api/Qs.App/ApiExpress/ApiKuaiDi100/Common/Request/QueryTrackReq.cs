using System ;
namespace Common.Request
{
    public class QueryTrackReq
    {
    /**
     * 我方分配给贵司的的公司编号, 点击查看账号信息
     */
    public string customer {get; set;}
    /**
     * 签名， 用于验证身份， 按param + key + customer 的顺序进行MD5加密（注意加密后字符串要转大写）， 不需要“+”号
     */
    public string sign {get; set;}
    /**
     * 其他参数组合成的json对象
     */
    public QueryTrackParam param {get; set;}
    }
}
