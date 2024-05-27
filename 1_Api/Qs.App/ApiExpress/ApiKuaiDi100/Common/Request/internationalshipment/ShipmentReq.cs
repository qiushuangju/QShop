using Newtonsoft.Json;
using System.Collections.Generic;
namespace Common.Request.internationalshipment
{
    public class ShipmentReq
    {
        /// <summary>
        ///  收件人信息
        /// </summary>
        public InterManInfo recMan { get; set; }
        /// <summary>
        ///  寄件人信息
        /// </summary>
        public InterManInfo sendMan { get; set; }
        /// <summary>
        ///  快递公司的编码，一律用小写字母，见参数字典）
        /// </summary>
        public string kuaidicom { get; set; }
        /// <summary>
        ///  产品类型： 默认标准快递，请参考参数字典）
        /// </summary>
        public string expType { get; set; }
        /// <summary>
        ///  物品总重量
        /// </summary>
        public double weight { get; set; }
        /// <summary>
        ///  包裹数
        /// </summary>
        public int count { get; set; }
        /// <summary>
        ///  备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        ///  货物描述
        /// </summary>
        public string cargo { get; set; }
        /// <summary>
        ///  月结或支付账号，详见字典表
        /// </summary>
        public string partnerId { get; set; }
        /// <summary>
        ///  月结账号密钥，详见字典表
        /// </summary>
        public string partnerKey { get; set; }
        /// <summary>
        ///  账号参数，详见字典表
        /// </summary>
        public string code { get; set; }
        /// <summary>
        ///  月结账号用户名，详见字典表
        /// </summary>
        public string partnerName { get; set; }
        /// <summary>
        ///  月结账号用户密码，详见字典表
        /// </summary>
        public string partnerSecret { get; set; }
        /// <summary>
        ///  贸易条款 {get; set;}CFR,DAP等,国际贸易规范用于,默认DAP
        /// </summary>
        public string tradeTerm { get; set; }
        /// <summary>
        ///  申报价值,包裹类必填,货币单位根据currency确定,人民币单位是元
        /// </summary>
        public double customsValue { get; set; }
        /// <summary>
        ///  包裹信息集合
        /// </summary>
        public List<PackageInfo> packageInfos { get; set; }
        /// <summary>
        ///  出口信息集合,一般包裹类要求必填,文件类不用填,用于清关报备
        /// </summary>
        public List<ExportInfo> exportInfos { get; set; }
        /// <summary>
        ///  货币单位,CNY-人民币 {get; set;}USD-美元 {get; set;}默认CNY
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        ///  关税支付方式,默认收件人支付
        /// </summary>
        public Payment dutiesPaymentType { get; set; }
        /// <summary>
        ///  运费支付方式（默认寄件人支付）SHIPPER:寄方付 CONSIGNEE:到付 MONTHLY:月结  THIRDPARTY:第三方支付
        /// </summary>
        public Payment freightPaymentType { get; set; }
        /// <summary>
        ///  下单时间（格式：yyyy-MM-dd HH:mm:ss,不传或者格式不正确默认当前时间）
        /// </summary>
        public string shipTime { get; set; }
        /// <summary>
        ///  打包方式:默认用户自行打包
        /// </summary>
        public string packagingType { get; set; }
        /// <summary>
        ///  商业发票信息
        /// </summary>
        public InvoiceInfo invoiceInfo { get; set; }
        /// <summary>
        ///  是否需要发票
        /// </summary>
        public bool needInvoice { get; set; }
        /// <summary>
        ///  清关信息
        /// </summary>
        public CustomsClearance customsClearance { get; set; }
        /// <summary>
        ///  时区,默认东八区时间 'GMT+8'预留
        /// </summary>
        public string timezone { get; set; }
        /// <summary>
        ///  SI表示千克和厘米组合；SU表示磅和英寸组合
        /// </summary>
        public string unitOfMeasurement { get; set; }
        /// <summary>
        ///  路线ID(极兔国际必填)
        /// </summary>
        public string routeId { get; set; }
        /// <summary>
        ///  是否需要通知
        /// </summary>
        public bool needNotification { get; set; }
        /// <summary>
        ///  通知郵箱
        /// </summary>
        public string notifyEmail { get; set; }
        /// <summary>
        ///  送达确认签名 （0-不需要签名 1-需提供签名 2-要求成年人签名）
        /// </summary>
        public int needDeliveryConfirmation { get; set; }
        /// <summary>
        ///  是否周六交貨
        /// </summary>
        public bool needSaturdayDelivery { get; set; }
        /// <summary>
        ///  是否包含电池
        /// </summary>
        public bool butterFlag { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

    }

}