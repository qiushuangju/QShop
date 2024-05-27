using Qs.Comm;
using Qs.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public  class ResOrder : ModelOrder
    {
        /// <summary>
        /// 状态名
        /// </summary>
        public string StrOrderStatus { get; set; }
        /// <summary>
        /// 支付类型
        /// </summary>
        public string StrPayType { get; set; }
        /// <summary>
        /// 售后状态
        /// </summary>
        public int RefundStatus { get; set; }

        /// <summary>
        /// 售后状态
        /// </summary>
        public string StrRefundStatus { get; set; }
        /// <summary>
        /// 用户路径
        /// </summary>
        public string StrSourceUser { get; set; }
        /// <summary>
        /// 申请售后时间
        /// </summary>
        public DateTime? DateTimeRefund { get; set; }
        /// <summary>
        /// 商品总数
        /// </summary>
        public int GoodsTotalNum { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public UserInfo UserInfo { get;set; }
        /// <summary>
        /// 配送/自提地址信息
        /// </summary>
        public Address AddressInfo { get; set; }
        /// <summary>
        /// 商品信息
        /// </summary>
        public List<ResOrderSku> ListSku { get; set; }    

        /// <summary>
        /// 订单地址
        /// </summary>
        public ModelOrderAddress OrderAddressInfo { get; set; }

        /// <summary>
        /// 是否接受售后
        /// </summary>
        public bool IsAllowRefund { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="user">用户信息</param>
        /// <param name="orderAddress">订单配送地址</param>
        /// <returns></returns>
        public static ResOrder ToView(ModelOrder order, ModelUser user, List<ResOrderSku> listOrderSku, int refundDays,
             ModelOrderAddress orderAddress=null)
        {
            ResOrder vm = xConv.CopyMapper<ResOrder, ModelOrder>(order);
            vm.UserInfo = user == null ? new UserInfo() : new UserInfo() { Account = user.Account, NickName = user.NickName, Phone = user.Phone };
            vm.ListSku = listOrderSku;
            vm.StrOrderStatus = xEnum.GetEnumDescription(typeof(xEnum.OrderStatus), order.OrderStatus);
            vm.StrPayType = xEnum.GetEnumDescription(typeof(xEnum.PayType), order.PayType);
            vm.OrderAddressInfo = orderAddress;
            bool isAllowRefund = true;
            if (order.OrderStatus <(int)xEnum.OrderStatus.WaitReceiving)
            {
                isAllowRefund = false;
            }
            if(refundDays!=0&& order.DoneTime!=null
                &&xConv.ToDateTime(order.DoneTime).AddDays(refundDays)<DateTime.Now)
            {
                isAllowRefund = false;
            }
            vm.IsAllowRefund = isAllowRefund;
                                                  
            if (orderAddress != null)
            {
                vm.AddressInfo = new Address()
                {
                    Name = orderAddress.Name,
                    Phone = orderAddress.Phone,
                    FullAddress = orderAddress.FullAddress
                };
            }
            vm.GoodsTotalNum = listOrderSku.Select(p => p.Quantity).Sum();
            return vm;
        }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string UrlAvater { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        ///手机号码
        /// </summary>
        public string Phone { get; set; }
    }

    /// <summary>
    /// 地址信息
    /// </summary>
    public class Address
    {
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string FullAddress { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
    }
}
