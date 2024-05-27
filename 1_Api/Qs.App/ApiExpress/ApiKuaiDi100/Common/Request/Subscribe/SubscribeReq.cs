namespace Common.Request.Subscribe
{
    public class SubscribeReq
    {
        /**
     * 返回数据格式(json、xml、text)
     */
    public string schema  {get; set;}
    /**
     * 其他参数
     */
    public SubscribeParam param  {get; set;}
    }
}