using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.Request.samecity
{
    public class SameCityOrderParam
    {
        /// <summary>
        /// 快递公司的编码，一律用小写字母，见《快递公司编码》
        /// </summary>
        public string com { get; set; }
        /// <summary>
        /// 收件人姓名
        /// </summary>
        public string recManName { get; set; }
        /// <summary>
        /// 收件人的手机号，手机号和电话号二者其一必填
        /// </summary>
        public string recManMobile { get; set; }
        /// <summary>
        /// 收件人所在完整地址，如广东深圳市深圳市南山区科技南十二路2号金蝶软件园
        /// </summary>
        public string recManPrintAddr { get; set; }
        /// <summary>
        /// 寄件人姓名
        /// </summary>
        public string sendManName { get; set; }
        /// <summary>
        /// 寄件人的手机号，手机号和电话号二者其一必填
        /// </summary>
        public string sendManMobile { get; set; }
        /// <summary>
        /// 寄件人所在的完整地址，如广东深圳市深圳市南山区科技南十二路2号金蝶软件园B10
        /// </summary>
        public string sendManPrintAddr { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string serviceType { get; set; }
        /// <summary>
        /// 物品总重量KG，例：1.5，单位kg
        /// </summary>
        public double weight { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 签名用随机字符串
        /// </summary>
        public string salt { get; set; }
        /// <summary>
        /// callBackUrl订单信息回调
        /// </summary>
        public string callbackUrl { get; set; }
        /// <summary>
        /// 订单类型，默认为0 0: 立即单  1: 预约单
        /// </summary>
        public int orderType { get; set; }
        /// <summary>
        /// 取货时间（2020-02-02 22:00,指的是预约取件时间）
        /// </summary>

        public string pickupTime { get; set; }
        /// <summary>
        /// 支付方式，SHIPPER: 寄付（默认），CONSIGNEE: 到付
        /// </summary>
        public string payment { get; set; }
        /// <summary>
        /// 对应商家版物品来源流水号
        /// </summary>
        public string orderSourceNo { get; set; }
        /// <summary>
        /// 物品来源
        /// </summary>
        public string orderSourceType { get; set; }
        /// <summary>
        /// 店铺ID
        /// </summary>
        public string storeId { get; set; }
        /// <summary>
        /// 小费（分）
        /// </summary>
        public int additionFee { get; set; }
        /// <summary>
        /// 保险费用(闪送支持)
        /// </summary>
        public int insurance { get; set; }
        /// <summary>
        /// 保险产品ID(闪送支持)
        /// </summary>
        public string insuranceProId { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public int price { get; set; }
        /// <summary>
        /// 代收价格
        /// </summary>
        public int CollectionPrice { get; set; }

        public string partnerId { get; set; }

        public string partnerKey { get; set; }

        /// <summary>
        /// 商品详情（强烈建议提供，方便骑手在取货时确认货品信息 ；顺丰时必填）
        /// </summary>
        public List<OrderGoods> goods { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}