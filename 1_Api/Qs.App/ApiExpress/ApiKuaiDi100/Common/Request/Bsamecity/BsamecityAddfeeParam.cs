using Newtonsoft.Json;
namespace Common.Request.Bsamecity
{
    public class BsamecityAddfeeParam{
        /// <summary>
        /// 下单时返回的orderId
        /// <summary>
        /// <value></value>
        public string orderId {get; set;}
        /// <summary>
        /// 下单时返回的taskId
        /// <summary>
        /// <value></value>
        public string taskId {get; set;}
        /// <summary>
        /// 订单小费，单位元
        /// <summary>
        /// <value></value>
        public string tips {get; set;}
        /// <summary>
        /// 备注
        /// <summary>
        /// <value></value>
        public string remark {get; set;}
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}