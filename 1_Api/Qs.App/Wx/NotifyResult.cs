namespace Qs.App.Wx
{
    /// <summary>
    /// 微信支付通知
    /// </summary>
    public class NotifyResult
    {
        public bool Status { set; get; }
        public long Uid { set; get; }
        public decimal PayMoney { set; get; }
        public string OrderId { set; get; }
        public string LinkId { set; get; }
        public string Attach { set; get; }

        public NotifyResult()
        {
            Attach = "";
            Status = false;
            Uid = 0;
            PayMoney = 0;
            OrderId = "";
            LinkId = "";
        }
    }
}
