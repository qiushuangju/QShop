using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Interface;
using Qs.App.UserManager;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Comm.Model;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Vm;
using TencentCloud.Iot.V20180123.Models;
using WxPayAPI;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 微信支付回调
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeChatPayNotify :  ControllerBase
    {
        private readonly AppOrder _appOrder;
        private readonly AppRechargeOrder _appRechargeOrder;
        private readonly AppUserManager _appUser;
        private readonly AppStoreSetting _appSetting;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appLog"></param>
        /// <param name="appBalance"></param>
        /// <param name="appBalanceRecord"></param>
        /// <param name="appCoupon"></param>
        /// <param name="appOrder"></param>
        public WeChatPayNotify( AppOrder appOrder, AppRechargeOrder appRechargeOrder ,
            AppUserManager appUser, AppStoreSetting appSetting)
        {
            _appRechargeOrder = appRechargeOrder;
            _appOrder = appOrder;               
            _appUser = appUser;
            _appSetting = appSetting;
        }

        /// <summary>
        /// 支付回调地址（在统一下单接口中设置notify_url）
        /// 注意:回调必须是post
        /// </summary>
        /// <returns></returns>
        [HttpPost("{id}")]
        [AllowAnonymous]
        public ActionResult PayNotifyUrl(string storeId)
        {
            try
            {

                VmSettingBasicWxApp setting = _appSetting.GetDetail(DefineSetting.BasicWxApp, storeId);
                Stream stream = HttpContext.Request.Body;
                // _appLog.Add(new ModelSysLog() { Href = "PayNotifyUrl1", ApiInContent = "进入调用" });
                if (HttpContext.Request.ContentLength != null)
                {
                    byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                    stream.Read(buffer, 0, buffer.Length);
                    string content = Encoding.UTF8.GetString(buffer);


                    //                     content = @"<xml><return_code><![CDATA[SUCCESS]]></return_code>
                    // <return_msg><![CDATA[OK]]></return_msg>
                    // <result_code><![CDATA[SUCCESS]]></result_code>
                    // <mch_id><![CDATA[1630696987]]></mch_id>
                    // <appid><![CDATA[wxf287d8ae61ca4942]]></appid>
                    // <openid><![CDATA[ozU4V41t8wGwP9vhG2I5PR2VBW34]]></openid>
                    // <is_subscribe><![CDATA[N]]></is_subscribe>
                    // <trade_type><![CDATA[JSAPI]]></trade_type>
                    // <trade_state><![CDATA[SUCCESS]]></trade_state>
                    // <bank_type><![CDATA[BOC_DEBIT]]></bank_type>
                    // <total_fee>1</total_fee>
                    // <fee_type><![CDATA[CNY]]></fee_type>
                    // <cash_fee>1</cash_fee>
                    // <cash_fee_type><![CDATA[CNY]]></cash_fee_type>
                    // <transaction_id><![CDATA[4200001734202303060230985771]]></transaction_id>
                    // <out_trade_no><![CDATA[96678d8500f240fc97565dbf9f40e903]]></out_trade_no>
                    // <attach><![CDATA[OpenVip]]></attach>
                    // <time_end><![CDATA[20230306175708]]></time_end>
                    // <trade_state_desc><![CDATA[支付成功]]></trade_state_desc>
                    // <nonce_str><![CDATA[aR22greO8f4loKNk]]></nonce_str>
                    // <sign><![CDATA[328E46DBC64E27F7CD8CE856CE3332AF]]></sign>
                    // </xml>";
                     xLog.Add(new ModelLog()
                    {
                        Title = $"支付通知1",
                        Href = $"WeChatPayNotify.PayNotifyUrl",
                        CreateName = "",
                        TypeName = "操作日志",
                        ApiInContent = content,
                        ApiOutContent = ""
                    });

                    WxPayData notifyData = new WxNotify().GetNotifyData(content, setting);
                    //检查支付结果中transaction_id是否存在
                    if (!notifyData.IsSet("transaction_id"))
                    {
                        throw new Exception($"支付结果中微信订单号不存在");
                    }
                    string transactionId = xConv.ToString(notifyData.GetValue("transaction_id")); //微信支付订单号
                    string orderId = xConv.ToString(notifyData.GetValue("out_trade_no")).Trim(); //商户系统的订单号，与请求一致。
                    string totalFee = xConv.ToString(notifyData.GetValue("total_fee")); //订单总金额，单位为分
                    string tradeType = xConv.ToString(notifyData.GetValue("trade_type")); //JSAPI、NATIVE、APP
                    string openid = xConv.ToString(notifyData.GetValue("openid")); //用户在商户appid下的唯一标识
                    string attach = xConv.ToString(notifyData.GetValue("attach")); //自定义参数
                    string wxResultNotify = $@"orderId:{orderId},total_fee:{totalFee},trade_type:{tradeType},transaction_id{transactionId},
                    WxPayTime:{DateTime.Now}";

                    xLog.Add(new ModelLog()
                    {
                        Title = $"支付通知2",
                        Href = $"WeChatPayNotify.PayNotifyUrl",
                        CreateName = "",
                        TypeName = "操作日志",
                        ApiInContent = "",
                        ApiOutContent = wxResultNotify
                    });


                    //查询订单，判断订单真实性
                    decimal totalAmount = xConv.ToDecimal(totalFee) / 100;
                   
                    //业务逻辑处理
                    if (attach == "Order")//订单
                    {
                        ModelOrder order = _appOrder.Get(orderId);
                       
                    }
                    else if (attach == "Recharge")//充值
                    {
                        ModelRechargeOrder order = _appRechargeOrder.Get(orderId);
                    }
                    if (!QueryOrder(transactionId, setting) )
                    {
                        throw new Exception($"QueryOrder失败:订单查询失败");
                    }
                    //查询订单成功
                    else
                    {   
                        if (attach == "Order")//订单
                        {
                            _appOrder.OrderPay(orderId, totalAmount);
                        }
                        else if (attach == "Recharge")//充值
                        {
                            _appUser.RechargePaid(orderId, totalAmount);
                        }
                       
                    }
                }

                var xml = PayNotifyXml();
                return Content(xml, "text/xml");
            }
            catch (Exception ex)
            {
                var xml = PayNotifyXml("FAIL", ex.Message);
                WxLog.Error(this.GetType().ToString(), "error:" + xml);
                return Content(xml, "text/xml");
            }
        }

        //查询订单
        private bool QueryOrder(string transactionId, VmSettingBasicWxApp wxAppConfig)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transactionId);
            WxPayData res = WxPayApi.OrderQuery(req, wxAppConfig);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 收到财付通消息后返回的回调通知
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string PayNotifyXml(string returnCode = "SUCCESS", string returnMsg = "OK")
        {
            return
                $@"<xml><return_code><![CDATA[{returnCode}]]></return_code><return_msg><![CDATA[{returnMsg}]]></return_msg></xml>";

        }
    }
}
