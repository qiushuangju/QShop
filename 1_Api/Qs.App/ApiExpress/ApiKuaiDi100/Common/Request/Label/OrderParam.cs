using Common.Request.Electronic;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Request.Label
{
    public class OrderParam
    {
        /// <summary>
        ///  收件人信息
        /// </summary>
        /// <value></value>
        public ManInfo recMan;
        /// <summary>
        ///  寄件人信息
        /// </summary>
        /// <value></value>
        public ManInfo sendMan;
        /// <summary>
        ///  快递公司编码
        /// </summary>
        /// <value></value>
        public string kuaidicom;
        /// <summary>
        ///  快递公司单号
        /// </summary>
        /// <value></value>
        public string kuaidinum;
        /// <summary>
        ///  订单号
        /// </summary>
        /// <value></value>
        public string orderId;
        /// <summary>
        ///  SHIPPER:寄方付 CONSIGNEE:到付 MONTHLY:月结 THIRDPARTY:第三方支付
        /// </summary>
        /// <value></value>
        public string payType = "MONTHLY";
        /// <summary>
        ///  快递类型: 标准快递 顺丰特惠 电商特惠 EMS经济
        /// </summary>
        /// <value></value>
        public string expType;
        /// <summary>
        ///  重量
        /// </summary>
        /// <value></value>
        public double weight;
        /// <summary>
        ///  体积（长 /// 宽 /// 高）
        /// </summary>
        /// <value></value>
        public string volume;
        /// <summary>
        ///  物品总数量
        /// </summary>
        /// <value></value>
        public int count = 1;
        /// <summary>
        ///  备注
        /// </summary>
        /// <value></value>
        public string remark;
        /// <summary>
        ///  保价额度
        /// </summary>
        /// <value></value>
        public double valinsPay;
        /// <summary>
        ///  代收货款
        /// </summary>
        /// <value></value>
        public double collection;
        /// <summary>
        ///  物品名称,例：文件
        /// </summary>
        /// <value></value>
        public string cargo;
        /// <summary>
        ///  是否需要子单： 1：需要 0：不需要(默认) 如果需要子单（指同一个订单打印出多张电子面单，即同一个订单返回多个面单号）； needChild = 1、count 需要大于1，如count = 2 一个主单 一个子单，count = 3 一个主单 二个子单，返回的子单号码见返回结果的childNum字段
        /// </summary>
        /// <value></value>
        public string needChild;
        /// <summary>
        ///  是否需要回单： 1：需要 0：不需要(默认) 返回的回单号见返回结果的returnNum字段
        /// </summary>
        /// <value></value>
        public string needBack;
        /// <summary>
        ///  电子面单客户账户或月结账号
        /// </summary>
        /// <value></value>
        public string partnerId;
        /// <summary>
        ///  电子面单密码
        /// </summary>
        /// <value></value>
        public string partnerKey;
        /// <summary>
        ///  电子面单密钥
        /// </summary>
        /// <value></value>
        public string net;
        /// <summary>
        ///  电子面单承载编号
        /// </summary>
        /// <value></value>
        public string code;
        /// <summary>
        ///  电子面单客户账户名称
        /// </summary>
        /// <value></value>
        public string partnerName;
        /// <summary>
        ///  电子面单承载快递员名
        /// </summary>
        /// <value></value>
        public string checkMan;
        /// <summary>
        ///  电子面单密钥，需贵司向当地快递公司网点申请
        /// </summary>
        /// <value></value>
        public string partnerSecret;
        /// <summary>
        ///  在使用菜鸟/淘宝/拼多多授权电子面单时，若月结账号下存在多个网点，则tbNet="网点名称,网点编号" ，注意此处为英文逗号
        /// </summary>
        /// <value></value>
        public string tbNet;
        /// <summary>
        ///  邮费
        /// </summary>
        /// <value></value>
        public double freight;
        /// <summary>
        ///  京东增值服务用
        /// </summary>
        /// <value></value>
        public string expressExtra;
        /// <summary>
        ///  增值服务 {"backnum":{"value":"无需返单"}}
        /// </summary>
        /// <value></value>
        public string addService;
        /// <summary>
        ///  订单类型;京东订单-JINGDONG;淘宝订单-TAOBAOSENT
        /// </summary>
        /// <value></value>
        public string recordType;
        /// <summary>
        ///  预约取件开始时间
        /// </summary>
        /// <value></value>
        public long startGotTime;
        /// <summary>
        ///  预约取件结束时间
        /// </summary>
        /// <value></value>
        public long endGotTime;
        /// <summary>
        ///  代收账户
        /// </summary>
        /// <value></value>
        public string colAcctNumber;
        /// <summary>
        ///  代收账户名
        /// </summary>
        /// <value></value>
        public string colAcctName;
        /// <summary>
        ///  顺丰电子验收
        /// </summary>
        /// <value></value>
        // public int elecValidateType ;
        /// <summary>
        ///  顺丰电子验收图片熟悉数量
        /// </summary>
        /// <value></value>
        // public int elecPicCount;
        /// <summary>
        ///  顺丰手持设备扫描设置
        /// </summary>
        /// <value></value>
        public int scanSupport ;
        /// <summary>
        ///  文件url
        /// </summary>
        /// <value></value>
        public string fileUrl;
        /// <summary>
        ///  取件方式
        /// </summary>
        /// <value></value>
        public string pickMethod;
        /// <summary>
        ///  是否外发,1-外发,0不外发
        /// </summary>
        /// <value></value>
        public int isOut = 0;
        /// <summary>
        ///  是否合伙人自提:1-是,0-否
        /// </summary>
        /// <value></value>
        public int isPickupSelf = 0;
        /// <summary>
        ///  是否接受仅镇中心派送:1-是,0-否
        /// </summary>
        /// <value></value>
        public int isCenterDelivery = 0;
        /// <summary>
        ///  第三方平台订单号
        /// </summary>
        /// <value></value>
        public string thirdOrderId;
        /// <summary>
        ///  开放地址ID 淘宝订单收件人ID (Open Addressee ID)，长度不超过128个字符，淘宝订单加密情况用于解密。
        /// </summary>
        /// <value></value>
        public string oaid;
        /// <summary>
        ///  菜鸟地址ID，针对电商平台加密订单场景使用，淘系订单使用oaid，非淘使用caid。
        /// </summary>
        /// <value></value>
        public string caid;

        /// <summary>
        ///  normal-常规的字母单,multi-一票多件
        /// </summary>
        /// <value></value>
        public string childNumType = "normal";
        /// <summary>
        /// 回单数量
        /// </summary>
        /// <value></value>
        public int backSign;
        /// <summary>
        ///  第三方平台模板url
        /// </summary>
        /// <value></value>
        public string thirdTemplateURL;
        /// <summary>
        ///  京东快运站点揽收字段
        /// </summary>
        /// <value></value>
        public int siteCollect;
        /// <summary>
        ///  京东快运站点派送字段
        /// </summary>
        /// <value></value>
        public int siteDelivery;
        /// <summary>
        ///  回单号
        /// </summary>
        /// <value></value>
        public string returnNum;
        /// <summary>
        ///  车辆类型名称（京东快运整车需要）
        /// </summary>
        /// <value></value>
        public string vehicleTypeName;
        /// <summary>
        ///  车辆类型编码（京东快运整车需要）
        /// </summary>
        /// <value></value>
        public string vehicleTypeNo;
        /// <summary>
        ///  整车单号（京东快运整车需要）
        /// </summary>
        /// <value></value>
        public string vehicleOrderNo;
        /// <summary>
        ///  自定义参数，优先级高于系统生成值，即出现相同key时，使用该参数的value
        /// </summary>
        /// <value></value>
        public Dictionary<string, object> customParam;
        /// <summary>
        ///  打印方向（默认0） 0-正方向 1-反方向
        /// </summary>
        /// <value></value>
        public string direction;
        /// <summary>
        ///  打印设备，通过打印机输出的设备码进行获取
        /// </summary>
        /// <value></value>
        public string siid;
        /// <summary>
        ///  打印状态回调地址
        /// </summary>
        /// <value></value>
        public string callBackUrl;
        /// <summary>
        ///  签名用随机字符串，用于验证签名sign。salt值不为null时，推送数据将包含该加密签名，加密方式：md5(param+salt)。注意： salt值为空串时，推送的数据也会包含sign，此时可忽略sign的校验。
        /// </summary>
        /// <value></value>
        public string salt;
        /// <summary>
        ///  是否开启订阅功能 false：不开启(默认) true：开启 说明开启订阅功能时：pollCallBackUrl必须填入 此功能只针对有快递单号的单
        /// </summary>
        /// <value></value>
        public bool needSubscribe;
        /// <summary>
        ///  如果op设置为1时，pollCallBackUrl必须填入，用于跟踪回调
        /// </summary>
        /// <value></value>
        public string pollCallBackUrl;
        /// <summary>
        ///  添加此字段表示开通行政区域解析或地图轨迹功能 。
        ///  0：关闭（默认）
        ///  1：开通行政区域解析功能
        ///  3：开通地图轨迹及时效返回
        /// </summary>
        /// <value></value>
        public string resultv2;
        /// <summary>
        ///  快递100模板url
        /// </summary>
        /// <value></value>
        public string tempId;
        /// <summary>
        ///  快递100子单模板url()
        /// </summary>
        /// <value></value>
        public string childTempId;
        /// <summary>
        ///  快递100回单模板url
        /// </summary>
        /// <value></value>
        public string backTempId;
        /// <summary>
        ///  是否脱敏 false：关闭（默认）true：开启
        /// </summary>
        /// <value></value>
        public bool needDesensitization;
        /// <summary>
        ///  是否需要logo false：关闭（默认）true：开启
        /// </summary>
        /// <value></value>
        public bool needLogo;
        /// <summary>
        ///  打印类型（HTML,IMAGE,CMD,CLOUD,NON）
        ///  NON:只下单不打印（默认）
        ///  HTML:生成html短链
        ///  IMAGE:生成图片短链
        ///  CMD:生成打印指令
        ///  CLOUD:使用快递100云打印机打印，使用CLOUD时siid必填
        /// </summary>
        /// <value></value>
        public string printType;

        /// <summary>
        ///  第三方平台订单是否需要ocr，开启后将会通过推送方式推送 false：关闭（默认）true：开启
        /// </summary>
        /// <value></value>
        public bool needOcr;

        /// <summary>
        ///  需要检测识别的面单元素。取值范围：barcode,qrcode,receiver,sender,bulkpen。不传或者 null 则默认为 ["barcode", "receiver", "sender"]
        /// </summary>
        /// <value></value>
        public string[] ocrInclude;

        public string height;

        public string width;
        /// <summary>
        ///  第三方平台自定义区域模板地址
        /// </summary>
        /// <value></value>
        public string thirdCustomTemplateUrl;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}