using Newtonsoft.Json;

namespace Common.Request.Border
{
    public class BOrderCallBackParamData {
        /// <summary>
        /// 平台订单ID
        /// </summary>
        public string orderId {get; set;}
        /// <summary>
        /// 订单状态： 1,‘已接单’, 2,‘收件中’, 3,‘改派’, 7,‘快递员修改订单信息’, 9,‘用户主动取消’, 10,‘已取件’, 15,‘已结算’, 99,‘订单已取消’
        /// </summary>
        public string status {get; set;}
        /// <summary>
        /// 快递员姓名
        /// </summary>
        public string courierName {get; set;}
        /// <summary>
        /// 快递员电话
        /// </summary>
        public string courierMobile {get; set;}
        /// <summary>
        /// 重量
        /// </summary>
        public string weight {get; set;}
        /// <summary>
        /// 运费
        /// </summary>
        public string freight {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }

}