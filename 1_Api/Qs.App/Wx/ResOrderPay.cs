namespace Qs.App.Wx
{
    /// <summary>
    /// 订单付款返回
    /// </summary>
    public class ResOrderPay
    {
        public string AppId { get; set; }
        public string signtype { get; set; }
        public string paysign { get; set; }
        public string timestamp { get; set; }
        public string noncestr { get; set; }
        public string openid { get; set; }
        public string package{ get; set; }
       public string orderid { get; set; }
    }
}
