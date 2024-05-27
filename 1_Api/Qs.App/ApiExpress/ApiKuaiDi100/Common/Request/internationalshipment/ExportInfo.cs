using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class ExportInfo
    {
        /// <summary>
        ///  净重
        /// </summary>
        public double netWeight { get; set; }
        /// <summary>
        ///  毛重
        /// </summary>
        public double grossWeight { get; set; }
        /// <summary>
        ///  原产国国家代码,中国-CN,美国-US
        /// </summary>
        public string manufacturingCountryCode { get; set; }
        /// <summary>
        ///  单位价格
        /// </summary>
        public double unitPrice { get; set; }
        /// <summary>
        ///  商品数量,INT 1 -1000000000,必须整数,小数dhl会报错,且不返回具体错误
        /// </summary>
        public int quantity { get; set; }
        /// <summary>
        ///  计数单位,件-PCS {get; set;}双-PRS {get; set;}千克-KG {get; set;}默认件
        /// </summary>
        public string quantityUnitOfMeasurement { get; set; }
        /// <summary>
        ///  物品描述
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        ///  进口商品编码
        /// </summary>
        public string importCommodityCode { get; set; }
        /// <summary>
        ///  出口商品编码
        /// </summary>
        public string exportCommodityCode { get; set; }
        /// <summary>
        ///  件数
        /// </summary>
        public int numberOfPieces { get; set; }
        /// <summary>
        ///  sku
        /// </summary>
        public string sku { get; set; }
        /// <summary>
        ///  产品的中文名
        /// </summary>
        public string zhName { get; set; }
        /// <summary>
        ///  产品的英文名
        /// </summary>
        public string enName { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}