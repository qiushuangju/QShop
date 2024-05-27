using System;
using System.Collections.Generic;
using System.Linq;
using Qs.Comm;
using Qs.Comm.Helpers;
using Qs.Repository.Vm;

namespace Qs.App.Wx
{
    public class Code2Session
    {
        public static ResultData Get(string code, VmSettingBasicWxApp wxAppConfig)
        {
            ResultData ResultData = new ResultData();
            string url =
                $"https://api.weixin.qq.com/sns/jscode2session?appid={wxAppConfig.AppId}&secret={wxAppConfig.AppSecret}&js_code={code}&grant_type=authorization_code";
            string strResult = Qs.Comm.Helpers.Utils.HttpGet(url);
            ResultData = xConv.JsonToObj<ResultData>(strResult);
            return ResultData;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="wxAppConfig"></param>
        /// <returns></returns>
        public static AccessToken GetAccessToken(VmSettingBasicWxApp wxAppConfig)
        {
            string url =
                $"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={wxAppConfig.AppId}&secret={wxAppConfig.AppSecret}";
            string strResult = Qs.Comm.Helpers.Utils.HttpGet(url);
            AccessToken res = xConv.JsonToObj<AccessToken>(strResult);
            if (!string.IsNullOrEmpty(res.errcode))
            {
                throw new Exception($"GetAccessToken, errcode:{res.errcode},errmsg{res.errmsg}");
            }
            return res;
        }

        /// <summary>
        /// 获取微信手机号
        /// </summary>
        /// <param name="code"></param>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public static ResWxPhone GetWxPhone(string code, string appKey,VmSettingBasicWxApp wxAppConfig)
        {
            AccessToken accessToken= GetAccessToken(wxAppConfig);
            string url =$"https://api.weixin.qq.com/wxa/business/getuserphonenumber?access_token={accessToken.access_token}";
            var reqData = new
            {
                code = code
            };
            string result = HttpHelper.HttpSend(HttpHelper.HttpSendType.PostJson, url, xConv.ToJson(reqData));

            ResWxPhone res = xConv.JsonToObj<ResWxPhone>(result);
            if (!string.IsNullOrEmpty(res.errcode)&&res.errcode!="0")
            {
                throw new Exception($"GetWxPhone, errcode:{res.errcode},errmsg{res.errmsg}");
            }
            return res;
        }


        #region ResultData
         /// <summary>
         /// 微信返回公共参数
         /// </summary>
        public class WxResBase
        {
            /// <summary>
            /// 错误码
            /// </summary>
            public string errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
        }
        /// <summary>
        /// AccessToken
        /// </summary>
        public class AccessToken : WxResBase
        {
           public  string access_token { get; set; }
           /// <summary>
           /// 失效时间
           /// </summary>
           public int expires_in { get; set; } 
        }

        public class ResWxPhone : WxResBase
        {
            public PhoneInfo phone_info { get; set; }
        }

        public class PhoneInfo
        {   
            /// <summary>
            /// 无区号手机号
            /// </summary>
            public string purePhoneNumber { get; set; }
            /// <summary>
            /// 区号
            /// </summary>
            public string countryCode { get; set; }
            /// <summary>
            /// 用户绑定的手机号(疑虑)
            /// </summary>
            public string phoneNumber { get; set; }
        }

        public class ResultData : BaseResultData
        {

            /// <summary>
            /// 用户唯一标识
            /// </summary>
            public string openid { set; get; }

            /// <summary>
            /// 会话密钥
            /// </summary>
            public string session_key { set; get; }

            /// <summary>
            /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回，详见 UnionID 机制说明。
            /// </summary>
            public string unionid { set; get; }

        }

        #endregion
    }
}
