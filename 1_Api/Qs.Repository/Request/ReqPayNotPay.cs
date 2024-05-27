using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Request
{
   public class ReqPayNotPay
    {
        /// <summary>
        /// AppUser:用户端 AppBusiness:业务员 AppDriver:司机 AppDispatch:调度
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 支付方式(10:微信支付 20:余额支付 30云闪付 50:找人代付)
        /// </summary>
        public int PayType { get; set; }
        /// <summary>
        /// 自定义标识(微信支付)
        /// </summary>
        public string PayAttach { get; set; }
        /// <summary>
        /// 余额支付密码
        /// </summary>
        public string BalancePwd { get; set; }
        /// <summary>
        /// 支付备注
        /// </summary>
        public string Remark { get; set; }
        

        /// <summary>
        /// 参数验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {

                new ValueTip(OrderId,"订单Id")  ,
            });
            if (PayType == (int)xEnum.PayType.Balance && string.IsNullOrEmpty(BalancePwd))
            {
                throw new Exception("余额支付密码不能为空");
            }
        }
    }
} 
