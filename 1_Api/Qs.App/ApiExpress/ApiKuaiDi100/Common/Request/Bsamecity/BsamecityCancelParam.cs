using Newtonsoft.Json;
namespace Common.Request.Bsamecity
{
    public class BsamecityCancelParam{
        /// <summary>
        /// 下单时返回的taskId
        /// <summary>
        /// <value></value>
        public string taskId {get; set;}
        /// <summary>
        /// 订单id
        /// <summary>
        /// <value></value>
        public string orderId {get; set;}
        /// <summary>
        /// 取消原因类型
        /// <summary>
        /// <value></value>
        public int cancelMsgType {get; set;}
        /// <summary>
        /// 取消原因
        /// <summary>
        /// <value></value>
        public string cancelMsg {get; set;}
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}