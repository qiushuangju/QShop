using Newtonsoft.Json;

namespace  Common.Request.Electronic.Print
{
    public class PrintCloudParam
    {
        /**
     * 业务类型，默认为10
     */
    public string type {get; set;}
    /**
     * 电子面单客户账户或月结账号，需贵司向当地快递公司网点申请
     */
    public string partnerId {get; set;}
    /**
     * 电子面单密码，需贵司向当地快递公司网点申请
     */
    public string partnerKey {get; set;}
    /**
     * 收件网点名称,由快递公司当地网点分配，
     * 若使用淘宝授权填入（taobao），使用菜鸟授权填入（cainiao）
     */
    public string net {get; set;}
    /**
     * 快递公司的编码，一律用小写字母
     */
    public string kuaidicom {get; set;}
    /**
     * 收件人信息
     */
    public ManInfo recMan {get; set;}
    /**
     * 收件人信息
     */
    public ManInfo sendMan {get; set;}
    /**
     * 	物品名称(部分快递公司必填)
     */
    public string cargo {get; set;}
    /**
     * 物品总数量。
     * 另外该属性与子单有关，如果需要子单（指同一个订单打印出多张电子面单，即同一个订单返回多个面单号），
     * needChild = 1、count 需要大于1，如count = 2 则一个主单 一个子单，
     * count = 3则一个主单 二个子单；返回的子单号码见返回结果的childNum字段
     */
    public string count {get; set;}
    /**
     * 物品总重量，单位：KG （例子：0.5）
     */
    public string weight {get; set;}
    /**
     * 支付方式：
     * SHIPPER:寄方付（默认）
     * CONSIGNEE:到付
     * MONTHLY:月结
     * THIRDPARTY:第三方支付
     */
    public string payType {get; set;}
    /**
     * 快递类型：
     * 标准快递（默认）
     * 顺丰特惠
     * EMS经济
     */
    public string expType {get; set;}
    /**
     * 备注
     */
    public string remark {get; set;}
    /**
     * 电子面单模板编码
     */
    public string tempid {get; set;}
    /**
     * 打印设备编码。通过打印机输出的设备码进行获取
     */
    public string siid {get; set;}
    /**
     * 保价额度
     */
    public string valinsPay {get; set;}
    /**
     * 代收货款额度
     */
    public string collection {get; set;}
    /**
     * 是否需要子单(支持子单的快递公司才可以用，是否支持可以参考参数字典)
     * 1:需要
     * 0:不需要(默认)
     * 如果需要子单（指同一个订单打印出多张电子面单，即同一个订单返回多个面单号）；
     * needChild = 1、count 需要大于1，如count = 2 一个主单 一个子单，
     * count = 3 一个主单 二个子单，返回的子单号码见返回结果的childNum字段
     */
    public string needChild {get; set;}
    /**
     * 是否需要回单(支持回单的快递公司才可以用，是否支持可以参考参数字典)
     * 1:需要
     * 0:不需要(默认)
     * 返回的回单号见返回结果的returnNum字段
     */
    public string needBack {get; set;}
    /**
     * 贵司内部自定义的订单编号,需要保证唯一性
     */
    public string orderId {get; set;}
    /**
     * 生成图片的高，以mm为单位(默认100)
     */
    public string height {get; set;}
    /**
     * 生成图片的宽，以mm为单位（默认180）
     */
    public string width {get; set;}
    /**
     * 签名用随机字符串
     */
    public string salt {get; set;}
    /**
     * 是否开启订阅功能：
     * 0：不开启(默认)
     * 1：开启
     * 说明开启订阅功能时：pollCallBackUrl必须填入
     * 此功能只针对有快递单号的单
     */
    public string op {get; set;}
    /**
     * 如果op设置为1时，pollCallBackUrl必须填入，用于跟踪回调
     */
    public string pollCallBackUrl {get; set;}
    /**
     * 添加此字段表示开通行政区域解析功能：0：关闭（默认）；1：开通行政区域解析功能
     */
    public string resultv2 {get; set;}
    /**
     * 该字段为申通专用，其他公司勿传；申通的需要传 44
     */
    public string code {get; set;}

     public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}