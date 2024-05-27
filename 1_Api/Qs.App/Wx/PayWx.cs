using System;
using System.Xml;
using Qs.Comm;
using Microsoft.AspNetCore.Http;
using Qs.Comm.Helpers;
using Qs.Repository.Vm;
using Qs.Repository.Wx;
using WxPayAPI;

namespace Qs.App.Wx
{


    /// <summary>
    /// 微信支付
    /// </summary>
    public class PayWx
    {
        private VmSettingBasicWxApp _wxAppConfig;

        public string notifyUrl = $"{Define.HttpBaseApi()}/api/WeChatPayNotify/PayNotifyUrl";
        /// <summary>
        ///     实例化支付模板
        /// </summary>
        /// <param name="appid">微信公众号APPID</param>
        /// <param name="appsecret">微信公众号AppSecret</param>
        /// <param name="partnerid">微信支付店铺Id</param>
        /// <param name="parerappkey">微信支付API安全码</param>
        public PayWx(VmSettingBasicWxApp wxAppConfig)
        {
            _wxAppConfig = wxAppConfig;
            // _accessToken = AccessToken.GetAccessToken(wxAppConfig);
        }
        #region GetPayParams


        /// <summary>
        /// 获取支付参数
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="money"></param>
        /// <param name="title"></param>
        /// <param name="outTradeNo"></param>
        /// <param name="attach">自定义参数</param>
        /// <returns></returns>
        public ResPayParams GetPayParams(string openid, decimal money, string title, string outTradeNo, string attach,
            string appKey)
        {
            var model = new ResPayParams();
            try
            {
                //                var attach = uid.ToString();
                var timeStamp = xConv.GetTimeStampTen().ToString();
                var nonceStr = xConv.NewGuid();
                var package = "";
                var paySign = "";
                var sign = "";
                var packageReqHandler = new RequestHandler();

                if (appKey == Define.AppWxApp && string.IsNullOrEmpty(openid))
                {
                    throw new Exception($"支付错误,没有找到微信用户信息");
                }
                    packageReqHandler.init();
                packageReqHandler.setParameter("body", title); //商品信息 127字符
                packageReqHandler.setParameter("appid", _wxAppConfig.AppId);
                packageReqHandler.setParameter("mch_id", _wxAppConfig.PartnerId);
                packageReqHandler.setParameter("nonce_str", nonceStr.ToLower());
                packageReqHandler.setParameter("notify_url", $"{notifyUrl}/{_wxAppConfig.StoreId}");
                if (!string.IsNullOrEmpty(openid))
                {
                    packageReqHandler.setParameter("openid", openid);
                }
                packageReqHandler.setParameter("out_trade_no", outTradeNo); //商家订单号
                // packageReqHandler.setParameter("spbill_create_ip", accessor.HttpContext.Connection.LocalIpAddress.ToString()); //用户的公网ip，不是商户服务器IP
                packageReqHandler.setParameter("total_fee", xConv.ToInt(money * 100).ToString()); //商品金额,以分为单位(money * 100).ToString()
                if (appKey == Define.AppWxApp)
                {
                    packageReqHandler.setParameter("trade_type", "JSAPI");
                }
                else
                {
                    packageReqHandler.setParameter("trade_type", "APP");
                }

                packageReqHandler.setParameter("attach", attach); //自定义参数 127字符
                sign = packageReqHandler.CreateMd5Sign("key", _wxAppConfig.PartnerAppKey);
                packageReqHandler.setParameter("sign", sign);
                var data = packageReqHandler.parseXML();
                var prepayXml = HttpHelper.HttpSend(HttpHelper.HttpSendType.Post, "https://api.mch.weixin.qq.com/pay/unifiedorder", data);

                var xdoc = new XmlDocument();
                xdoc.LoadXml(prepayXml);
                var xn = xdoc.SelectSingleNode("xml");
                var xnl = xn.ChildNodes;
                string prepayId = "";

                if (xnl.Count > 7)
                {
                    prepayId = xnl[7].InnerText;
                    package = $"prepay_id={prepayId}";
                }
                var paySignReqHandler = new RequestHandler();
                // appId、timeStamp、nonceStr、prepayid

                paySignReqHandler.setParameter("appId", _wxAppConfig.AppId);
                paySignReqHandler.setParameter("timeStamp", timeStamp);
                paySignReqHandler.setParameter("nonceStr", nonceStr);
                paySignReqHandler.setParameter("package", package);
                paySignReqHandler.setParameter("signType", "MD5");


                // paySignReqHandler.setParameter("prepayid", prepayId);
                // paySignReqHandler.setParameter("package", "Sign=WXPay");
                // paySignReqHandler.setParameter("partnerid", _wxAppConfig.PartnerId);
                paySign = paySignReqHandler.CreateMd5Sign("key", _wxAppConfig.PartnerAppKey);

                model.AppId = _wxAppConfig.AppId;
                model.PartnerId = _wxAppConfig.PartnerId;
                model.PrepayId = prepayId;
                model.SignType = "MD5";
                model.PaySign = paySign;
                model.TimeStamp = timeStamp;
                model.NonceStr = nonceStr;
                model.OpenId = openid;
                model.Package = package;
                model.OutTradeNo = outTradeNo;
            }
            catch (Exception em)
            {
                throw em;
            }
            return model;
        }

      /// <summary>
      /// 申请退款
      /// </summary>
      /// <param name="orderId"></param>
      /// <param name="refundOrderId"></param>
      /// <param name="orderAmout"></param>
      /// <param name="refundAmount"></param>
      /// <returns></returns>
        public ResPayParams Refund(string orderId, string refundOrderId, decimal orderAmout,decimal refundAmount)
        {
            var model = new ResPayParams();
            try
            {
                WxPayData inputObj = new WxPayData();
                inputObj.SetValue("out_trade_no", orderId);
                inputObj.SetValue("out_refund_no", refundOrderId);
                inputObj.SetValue("total_fee", xConv.ToInt(orderAmout * 100));
                inputObj.SetValue("refund_fee", xConv.ToInt(refundAmount * 100) );
                // inputObj.SetValue("op_user_id", outTradeNo);
                
                WxPayApi.Refund(inputObj, _wxAppConfig);
            }
            catch (Exception em)
            {
                throw em;
            }
            return model;
        }

        #endregion
        /// <summary>
        ///     会话
        /// </summary>
        private string _accessToken { get; }

      


     
        #region Notify
        /// <summary>
        /// 运行回调模板
        /// </summary>
        /// <returns></returns>
        public NotifyResult GetNotify(string xml, VmSettingBasicWxApp setting)
        {
            NotifyResult NotifyResult = new NotifyResult();

            //创建ResponseHandler实例
            ResponseHandler resHandler = new ResponseHandler(xml);
            resHandler.setKey(setting.PartnerAppKey);

            //判断签名
            try
            {
                if (resHandler.isWXsign(out string error))
                {
                    #region 协议参数=====================================
                    //--------------协议参数--------------------------------------------------------
                    //SUCCESS/FAIL此字段是通信标识，非交易标识，交易是否成功需要查
                    string return_code = resHandler.getParameter("return_code");
                    //返回信息，如非空，为错误原因签名失败参数格式校验错误
                    string return_msg = resHandler.getParameter("return_msg");
                    //微信分配的公众账号 ID
                    string appid = resHandler.getParameter("appid");

                    //以下字段在 return_code 为 SUCCESS 的时候有返回--------------------------------
                    //微信支付分配的商户号
                    string mch_id = resHandler.getParameter("mch_id");
                    //微信支付分配的终端设备号
                    string device_info = resHandler.getParameter("device_info");
                    //微信分配的公众账号 ID
                    string nonce_str = resHandler.getParameter("nonce_str");
                    //业务结果 SUCCESS/FAIL
                    string result_code = resHandler.getParameter("result_code");
                    //错误代码 
                    string err_code = resHandler.getParameter("err_code");
                    //结果信息描述
                    string err_code_des = resHandler.getParameter("err_code_des");

                    //以下字段在 return_code 和 result_code 都为 SUCCESS 的时候有返回---------------
                    //-------------业务参数---------------------------------------------------------
                    //用户在商户 appid 下的唯一标识
                    string openid = resHandler.getParameter("openid");
                    //用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效
                    string is_subscribe = resHandler.getParameter("is_subscribe");
                    //JSAPI、NATIVE、MICROPAY、APP
                    string trade_type = resHandler.getParameter("trade_type");
                    //银行类型，采用字符串类型的银行标识
                    string bank_type = resHandler.getParameter("bank_type");
                    //订单总金额，单位为分
                    string total_fee = resHandler.getParameter("total_fee");
                    //货币类型，符合 ISO 4217 标准的三位字母代码，默认人民币：CNY
                    string fee_type = resHandler.getParameter("fee_type");
                    //微信支付订单号
                    string transaction_id = resHandler.getParameter("transaction_id");
                    //商户系统的订单号，与请求一致。
                    string out_trade_no = resHandler.getParameter("out_trade_no");
                    //商家数据包，原样返回
                    string attach = resHandler.getParameter("attach");
                    //支 付 完 成 时 间 ， 格 式 为yyyyMMddhhmmss，如 2009 年12 月27日 9点 10分 10 秒表示为 20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器
                    string time_end = resHandler.getParameter("time_end");

                    #endregion
                    //支付成功
                    if (!out_trade_no.Equals("") && return_code.Equals("SUCCESS") && result_code.Equals("SUCCESS"))
                    {
//                        Log.Append("Notify 页面  支付成功，支付信息：商家订单号：" + out_trade_no + "、支付金额(分)：" + total_fee + "、自定义参数：" + attach);

                        NotifyResult.OrderId = out_trade_no;
                        NotifyResult.Uid = xConv.ToLong(attach);
                        NotifyResult.Attach = attach;
                        NotifyResult.PayMoney = xConv.ToDecimal(total_fee) / 100;
                        NotifyResult.Status = true;
                        NotifyResult.LinkId = transaction_id;
                    }
                    else
                    {
//                        xhand.Log.Append("Notify 页面  支付失败，支付信息   total_fee= " + total_fee + "、err_code_des=" + err_code_des + "、result_code=" + result_code);
                    }
                }
                else
                {
//                    xhand.Log.Append("Notify 页面  isWXsign= false ，错误信息：" + error);
                }
            }
            catch (Exception ee)
            {
//                xhand.Log.Append("Notify 页面  发送异常错误：" + ee.Message);
            }

            return NotifyResult;
        }
        #endregion

    }
}