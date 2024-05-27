using System;


namespace Common
{
    public class ApiInfoConstant
    {
        /// <summary>
        /// 查询url
        /// </summary>
        public const string QUERY_URL = "http://poll.kuaidi100.com/poll/query.do";

        /// <summary>
        /// 订阅url
        /// </summary>
        public const string SUBSCRIBE_URL = "https://poll.kuaidi100.com/poll";
        /// <summary>
        /// 地图轨迹查询url
        /// </summary>
        public const string QUERY_URL_WITH_MAP = "https://poll.kuaidi100.com/poll/maptrack.do";

        /// <summary>
        /// 地图轨迹订阅url
        /// </summary>
        public const string SUBSCRIBE_URL_WITH_MAP = "http://poll.kuaidi100.com/pollmap";
        /// <summary>
        ///  订阅SCHEMA
        /// </summary>
        public const string SUBSCRIBE_SCHEMA = "json";
        /// <summary>
        ///  智能单号识别url
        /// </summary>
        public const string AUTO_NUM_URL = "http://www.kuaidi100.com/autonumber/auto?num={0}&key={1}";
        /// <summary>
        ///  电子面单html url
        /// </summary>
        public const string ELECTRONIC_ORDER_HTML_URL = "http://poll.kuaidi100.com/eorderapi.do";
        /// <summary>
        ///  电子面单html方法
        /// </summary>
        public const string ELECTRONIC_ORDER_HTML_METHOD = "getElecOrder";
        /// <summary>
        ///  电子面单获取图片 url
        /// </summary>
        public const string ELECTRONIC_ORDER_PIC_URL = "https://poll.kuaidi100.com/printapi/printtask.do";
        /// <summary>
        ///  电子面单获取图片
        /// </summary>
        public const string ELECTRONIC_ORDER_PIC_METHOD = "getPrintImg";
        /// <summary>
        ///  电子面单打印 url
        /// </summary>
        public const string ELECTRONIC_ORDER_PRINT_URL = "https://poll.kuaidi100.com/printapi/printtask.do";
        /// <summary>
        ///  电子面单打印方法
        /// </summary>
        public const string ELECTRONIC_ORDER_PRINT_METHOD = "eOrder";
        /// <summary>
        ///  菜鸟淘宝账号授权
        /// </summary>
        public const string AUTH_THIRD_URL = "https://poll.kuaidi100.com/printapi/authThird.do";
        /// <summary>
        ///  云打印url
        /// </summary>
        public const string CLOUD_PRINT_URL = "http://poll.kuaidi100.com/printapi/printtask.do?method={0}&t={1}&key={2}&sign={3}&param={4}";
        /// <summary>
        ///  自定义打印方法
        /// </summary>
        public const string CLOUD_PRINT_CUSTOM_METHOD = "printOrder";
        /// <summary>
        ///  附件打印方法
        /// </summary>
        public const string CLOUD_PRINT_ATTACHMENT_METHOD = "imgOrder";
        /// <summary>
        ///  复打方法
        /// </summary>
        public const string CLOUD_PRINT_OLD_METHOD = "printOld";
        /// <summary>
        ///  硬件状态接口方法
        /// </summary>
        public const string CLOUD_PRINT_DEV_STATUS = "devstatus";
        /// <summary>
        ///  短信url
        /// </summary>
        public const string SEND_SMS_URL = "http://apisms.kuaidi100.com:9502/sms/send.do";
        /// <summary>
        ///  商家寄件
        /// </summary>
        public const string B_ORDER_URL = "https://order.kuaidi100.com/order/borderbestapi.do";
        /// <summary>
        ///  商家寄件查询运力
        /// </summary>
        public const string B_ORDER_QUERY_TRANSPORT_CAPACITY_METHOD = "querymkt";
        /// <summary>
        ///  商家寄件下单
        /// </summary>
        public const string B_ORDER_SEND_METHOD = "bOrderBest";
        /// <summary>
        ///  商家寄件获取验证码
        /// </summary>
        public const string B_ORDER_CODE_METHOD = "getCode";
        /// <summary>
        ///  商家寄件取消
        /// </summary>
        public const string B_ORDER_CANCEL_METHOD = "cancelBest";
        /// <summary>
        ///  商家寄件(官方寄件)请求url
        /// </summary>
        public const string B_ORDER_OFFICIAL_URL = "https://poll.kuaidi100.com/order/borderapi.do";
        /// <summary>
        ///  商家寄件(官方寄件)下单
        /// </summary>
        public const string B_ORDER_OFFICIAL_ORDER_METHOD = "bOrder";
        /// <summary>
        ///  商家寄件(官方寄件)取消
        /// </summary>
        public const string B_ORDER_OFFICIAL_CANCEL_METHOD = "cancel";
        /// <summary>
        ///  商家寄件(官方寄件)查询价格
        /// </summary>
        public const string B_ORDER_OFFICIAL_PRICE_METHOD = "Price";

        /// <summary>
        ///  同城配送请求url
        /// </summary>
        public const string SAME_CITY_ORDER_URL = "https://order.kuaidi100.com/sameCity/order";
        /// <summary>
        ///  同城配送授权方法
        /// </summary>
        public const string SAME_CITY_AUTH_METHOD = "auth";
        /// <summary>
        ///  同城配送下单方法
        /// </summary>
        public const string SAME_CITY_ORDER_METHOD = "order";
        /// <summary>
        ///  同城配送查询订单方法
        /// </summary>
        public const string SAME_CITY_QUERY_METHOD = "query";
        /// <summary>
        ///  同城配送取消订单方法
        /// </summary>
        public const string SAME_CITY_CANCEL_METHOD = "cancel";
        /// <summary>
        ///  取消方法
        /// </summary>
        public const string CANCEL_METHOD = "cancel";
        /// <summary>
        ///  指令打印方法
        /// </summary>
        public const string CLOUD_PRINT_COMMAND = "printCommand";
        /// <summary>
        ///  指令打印
        /// </summary>
        public const string INTERNATIONAL_SHIPMENT_URL = "http://api.kuaidi100.com/sendAssistant/order/apiCall";
        /// <summary>
        ///  订单导入授权url
        /// </summary>
        public const string THIRD_PLATFORM_ORDER_SHOP_AUTHORIZE_url = "https://api.kuaidi100.com/ent/shop/authorize";
        /// <summary>
        ///  订单导入提交订单获取任务接口
        /// </summary>
        public const string THIRD_PLATFORM_ORDER_COMMIT_TASK = "https://api.kuaidi100.com/ent/order/task";
        /// <summary>
        ///  订单导入提交订单回填单号
        /// </summary>
        public const string THIRD_PLATFORM_ORDER_UPLOAD_NUM = "https://api.kuaidi100.com/ent/logistics/send";
        /// <summary>
        ///  发货单接口url
        /// </summary>
        public const string BILL_PARCELS_URL = "https://poll.kuaidi100.com/print/billparcels.do";
        /// <summary>
        ///  发货单接口方法
        /// </summary>
        public const string BILL_PARCELS_METHOD = "billparcels";

        /// <summary>
        ///  面单余额接口方法
        /// </summary>
        public const string THIRD_PLATFORM_BRANCH_INFO_METHOD = "getThirdInfo";

        /// <summary>
        ///  快递面单OCR识别接口
        /// </summary>
        public const string OCR_URL = "http://api.kuaidi100.com/elec/detocr";
        /// <summary>
        ///  新模板编辑器请求url
        /// </summary>
        public const string NEW_TEMPLATE_URL = "https://api.kuaidi100.com/label/order";
        /// <summary>
        /// 下单
        /// </summary>
        public const String ORDER = "order";
        /// <summary>
        /// 自定义
        /// </summary>
        public const String CUSTOM = "custom";
        /// <summary>
        /// 详情
        /// </summary>
        public const String DETAIL = "detail";
        /// <summary>
        ///  C端寄件下单接口url
        /// </summary>
        public const string C_ORDER_URL = "https://order.kuaidi100.com/order/corderapi.do";
        /// <summary>
        ///  价格
        /// </summary>
        public const string PRICE = "price";
        /// <summary>
        /// cOrder
        /// </summary>
        public const string CORDER = "cOrder";
         /// <summary>
        ///  快递可用性接口url
        /// </summary>
        public const string EXPRESS_REACHABLE_URL = "http://api.kuaidi100.com/reachable.do";
        /// <summary>
        ///  快递可用性接口方法
        /// </summary>
        public const string EXPRESS_REACHABLE_METHOD = "reachable";
        /// <summary>
        ///  同城寄件接口url
        /// </summary>
        public const string BSAMECITY_EXPRESS_URL = "https://api.kuaidi100.com/bsamecity/order";
        /// <summary>
        ///  同城寄件-预下单方法
        /// </summary>
        public const string BSAMECITY_EXPRESS_PRICE = "price";
        /// <summary>
        ///  同城寄件-下单方法
        /// </summary>
        public const string BSAMECITY_EXPRESS_ORDER = "order";
        /// <summary>
        ///  同城寄件-预取消方法
        /// </summary>
        public const string BSAMECITY_EXPRESS_PRECANCEL = "precancel";
        /// <summary>
        ///  同城寄件-取消方法
        /// </summary>
        public const string BSAMECITY_EXPRESS_CANCEL = "cancel";
        /// <summary>
        ///  同城寄件-加小费方法
        /// </summary>
        public const string BSAMECITY_EXPRESS_ADDFEE = "addfee";
    }
}
