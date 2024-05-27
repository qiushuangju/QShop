using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
   public class ReqExportOrder
    {
        /// <summary>
        /// 地区
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 地区
        /// </summary>
        public string StorageName { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string OrderTypeName { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string StrUserType { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int? UserType { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 收货人 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string FullAddress { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 规格名称
        /// </summary>
        public string SkuName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int TotalNum { get; set; }
        /// <summary>
        /// 实付单价
        /// </summary>
        public decimal? PayPrice { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public decimal? RetailPrice { get; set; }
        /// <summary>
        /// 会员价
        /// </summary>
        public decimal? VipPrice { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// 一级提成
        /// </summary>
        public decimal Commission1 { get; set; }
        /// <summary>
        /// 二级提成
        /// </summary>
        public decimal Commission2 { get; set; }
        /// <summary>
        /// 总提成
        /// </summary>
        public decimal TotalCommission { get; set; }
        /// <summary>
        /// 期望 时间
        /// </summary>
        public string ExpectTimeSlot { get; set; }
        
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PayTime { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime? OutBoundTime { get; set; }
        /// <summary>
        /// 送达时间
        /// </summary>
        public DateTime? ArrivedTime { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus { get; set; }
        /// <summary>
        /// 导出状态
        /// </summary>
        public string StrOrderStatus { get; set; }
        /// <summary>
        /// 导出状态
        /// </summary>
        public string StrExportStatus { get; set; }
        /// <summary>
        /// 是否售后
        /// </summary>
        public int IsRefund { get; set; }

        /// <summary>
        /// 司机名称
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// 司机电话
        /// </summary>
        public string DriverPhone { get; set; }
        /// <summary>
        /// 司机类型 (10:固定司机 20:临时司机)
        /// </summary>
        public int DriverType { get; set; }
        /// <summary>
        /// 司机类型 (10:固定司机 20:临时司机)
        /// </summary>
        public string StrDriverType { get; set; }
        /// <summary>
        /// 运费
        /// </summary>
        public decimal? FreightPrice { get; set; }
        /// <summary>
        /// 上楼费
        /// </summary>
        public decimal? FloorPrice { get; set; }
        /// <summary>
        /// 入户费
        /// </summary>
        public decimal? EntryPrice { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public decimal? ShippingFee { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string BuyerRemark { get; set; }
    }
}
