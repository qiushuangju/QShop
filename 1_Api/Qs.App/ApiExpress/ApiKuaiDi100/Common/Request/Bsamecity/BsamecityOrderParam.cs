using System.Collections.Generic;
using Newtonsoft.Json;
namespace Common.Request.Bsamecity
{
    public class BsamecityOrderParam{
        /// <summary>
        /// 快递公司的编码，一律用小写字母，见《快递公司编码》
        /// <summary>
        /// <value></value>
        public string kuaidicom {get; set;}
        /// <summary>
        /// 坐标类型(1：百度坐标，2：高德坐标 ,默认2)
        /// <summary>
        /// <value></value>
        public int lbsType {get; set;}
        /// <summary>
        /// 收件人姓名，长度最大20
        /// <summary>
        /// <value></value>
        public string recManName {get; set;}
        /// <summary>
        /// 收件人的手机号（有手机号和固话正则校验）
        /// <summary>
        /// <value></value>
        public string recManMobile {get; set;}
        /// <summary>
        /// 收件人所在的省，长度最大20
        /// <summary>
        /// <value></value>
        public string recManProvince {get; set;}
        /// <summary>
        /// 收件人所在的市，长度最大20
        /// <summary>
        /// <value></value>
        public string recManCity {get; set;}
        /// <summary>
        /// 收件人所在的区，长度最大20
        /// <summary>
        /// <value></value>
        public string recManDistrict {get; set;}
        /// <summary>
        /// 收件人所在的完整地址，如 科技南十二路2号金蝶软件园B10，长度最大100
        /// <summary>
        /// <value></value>
        public string recManAddr {get; set;}
        /// <summary>
        /// 收件人地址纬度，默认高德坐标，长度最大10
        /// <summary>
        /// <value></value>
        public string recManLat {get; set;}
        /// <summary>
        /// 收件人地址经度，默认高德坐标，长度最大10
        /// <summary>
        /// <value></value>
        public string recManLng {get; set;}
        /// <summary>
        /// 寄件人姓名，长度最大20
        /// <summary>
        /// <value></value>
        public string sendManName {get; set;}
        /// <summary>
        /// 寄件人的手机号（有手机号和固话正则校验）
        /// <summary>
        /// <value></value>
        public string sendManMobile {get; set;}
        /// <summary>
        /// 寄件人所在的省，长度最大20
        /// <summary>
        /// <value></value>
        public string sendManProvince {get; set;}
        /// <summary>
        /// 寄件人所在的市，长度最大20
        /// <summary>
        /// <value></value>
        public string sendManCity {get; set;}
        /// <summary>
        /// 寄件人所在的区，长度最大20
        /// <summary>
        /// <value></value>
        public string sendManDistrict {get; set;}
        /// <summary>
        /// 寄件人所在的完整地址，如 科技南十二路2号金蝶软件园B10，长度最大100
        /// <summary>
        /// <value></value>
        public string sendManAddr {get; set;}
        /// <summary>
        /// 寄件人地址纬度，默认高德坐标，长度最大10
        /// <summary>
        /// <value></value>
        public string sendManLat {get; set;}
        /// <summary>
        /// 寄件人地址经度，默认高德坐标，长度最大10
        /// <summary>
        /// <value></value>
        public string sendManLng {get; set;}
        /// <summary>
        /// 物品总重量KG，例：1.5，单位kg
        /// <summary>
        /// <value></value>
        public string weight {get; set;}
        /// <summary>
        /// 备注,例：测试寄件，长度最多255
        /// <summary>
        /// <value></value>
        public string remark {get; set;}
        /// <summary>
        /// 体积cm3，长度最多20
        /// <summary>
        /// <value></value>
        public string volume {get; set;}
        /// <summary>
        /// 0：无需预约 1：预约单送达时间 2：预约单上门时间 默认为0
        /// <summary>
        /// <value></value>
        public int orderType {get; set;}
        /// <summary>
        /// 取货时间，orderType=2时必填，例子：2020-02-02 22:00
        /// <summary>
        /// <value></value>
        public string expectPickupTime {get; set;}
        /// <summary>
        /// 期望送达时间，orderType=1时必填（例子：2020-02-02 22:00）
        /// <summary>
        /// <value></value>
        public string expectFinishTime {get; set;}
        /// <summary>
        /// 保价物品金额
        /// <summary>
        /// <value></value>
        public string insurance {get; set;}
        /// <summary>
        /// 物品总金额，例：100.23
        /// <summary>
        /// <value></value>
        public string price {get; set;}
        /// <summary>
        /// 是否为专人直送订单，0为否，1为是
        /// <summary>
        /// <value></value>
        public int directDelivery {get; set;}
        /// <summary>
        /// 商品详情
        /// <summary>
        /// <value></value>
        public List<Goods> goods {get; set;}
        /// <summary>
        /// 下单结果回调地址
        /// <summary>
        /// <value></value>
        public string callbackUrl {get; set;}
        /// <summary>
        ///  签名用随机字符串，长度最多20
        /// <summary>
        /// <value></value>
        public string salt {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}