using System;
using System.Collections.Generic;
using System.Linq;
using Qs.App.Request;
using Qs.Comm;
using Qs.Comm.Helpers;
using Qs.Repository.Vm;

using WxPayAPI;

namespace Qs.App.Wx
{
    /// <summary>
    /// 微信Token
    /// </summary>
    public class WxAccessToken
    {
        public static List<ModelAccessToken> ListAccessToken = new List<ModelAccessToken>();

        /// <summary>
        ///  AccessToken
        /// </summary>
        /// <param name="wxAppConfig"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string GetWxAccessToken(VmSettingBasicWxApp setting)
        {
            ModelAccessToken model = ListAccessToken.FirstOrDefault(p => p.StoreId == setting.StoreId);
            if (model == null)
            {
                model = new ModelAccessToken() { StoreId = setting.StoreId };
            }
            if (model.OutTime <= DateTime.Now)
            {
                var resultData = GetNewAccessToken(setting);
                if (resultData != null)
                {
                    model.access_token = resultData.access_token;
                    model.OutTime = DateTime.Now.AddSeconds(resultData.expires_in - 10);
                }
                else
                {
                    throw new Exception(resultData.errmsg);
                }
            }

            return model.access_token;
        }

        private static ResultData GetNewAccessToken(VmSettingBasicWxApp setting)
        {
            var xx = new HttpHelper().Get(null,$"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={setting.AppId}&secret={setting.AppSecret}");

            var resultData = xConv.JsonToObj<ResultData>(xx);

            return resultData;
        }

        /// <summary>
        /// 获取WxJsTicket
        /// </summary>
        /// <returns></returns>
        public static ResWxJsTicket GetWxJsTicket(string accessToken)
        {
            string url = $"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={accessToken}&type=jsapi";
            var xx = new HttpHelper().Get(null, url);
            ResWxJsTicket resWxJsTicket = xConv.JsonToObj<ResWxJsTicket>(xx);
            return resWxJsTicket;
        }


        public static ReqWxJsParam GetWxJsParam(string url , VmSettingBasicWxApp setting)
        {
            string token = WxAccessToken.GetWxAccessToken(setting);
            ReqWxJsParam param = new ReqWxJsParam();
            param.AppId = setting.AppId;
            ResWxJsTicket ticket = WxAccessToken.GetWxJsTicket(token);
            param.Noncestr = xConv.NewGuid();
            param.Timestamp = xConv.GetTimeStampTen(DateTime.Now);
            param.Signature = WxPayData.GetSignature(ticket.ticket, param.Noncestr, param.Timestamp, url);
            WxLog.Debug("GetWxJsParam", $"ticket:{ticket.ticket},AppId:{param.AppId},Noncestr:{param.Noncestr},Timestamp:{param.Timestamp},Signature:{param.Signature}");
            return param;
        }

        /// <summary>
        ///   Ticket
        /// </summary>
        public class ResWxJsTicket
        {
            public int errcode { get; set; } //含义见错误码
            public string errmsg { get; set; } //含义见错误码

            public string ticket { get; set; } //临时票据，用于在获取授权链接时作为参数传入

            public int expires_in { get; set; } //ticket 的有效期，一般为 7200 秒
        }

        #region ResultData

        public class ResultData : BaseResultData
        {
            //access_token 的存储至少要保留 512 个字符空间；
            //access_token 的有效期目前为 2 个小时，需定时刷新，重复获取将导致上次获取的 access_token 失效；
            //建议开发者使用中控服务器统一获取和刷新 access_token，其他业务逻辑服务器所使用的 access_token 均来自于该中控服务器，不应该各自去刷新，否则容易造成冲突，导致 access_token 覆盖而影响业务；
            //access_token 的有效期通过返回的 expire_in 来传达，目前是7200秒之内的值，中控服务器需要根据这个有效时间提前去刷新。在刷新过程中，中控服务器可对外继续输出的老 access_token，此时公众平台后台会保证在5分钟内，新老 access_token 都可用，这保证了第三方业务的平滑过渡；
            //access_token 的有效时间可能会在未来有调整，所以中控服务器不仅需要内部定时主动刷新，还需要提供被动刷新 access_token 的接口，这样便于业务服务器在API调用获知 access_token 已超时的情况下，可以触发 access_token 的刷新流程。
            public string access_token { set; get; }
            public int expires_in { set; get; }
        }


     

        #endregion

        public class ModelAccessToken
        {
            public string StoreId { get; set; }
            public string access_token { set; get; }
            public DateTime OutTime { set; get; }
        }
    }
    
  
    public class BaseResultData
    {
        public string errmsg { set; get; }
        public string errcode { set; get; }
    }
}