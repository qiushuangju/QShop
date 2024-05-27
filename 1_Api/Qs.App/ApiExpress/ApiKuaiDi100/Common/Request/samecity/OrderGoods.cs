using Newtonsoft.Json;

namespace Common.Request.samecity
{
    public class OrderGoods
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 商品价格（分）
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 商品单位
        /// </summary>
        public string unit { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}