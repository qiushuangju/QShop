using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Qs.App.ApiKuaiDiNiao.Res;
using Qs.Comm;

namespace Qs.App.ApiExpress.ApiKuaiDiNiao {

    /// <summary>
    /// 快递鸟查询
    /// </summary>
    public class ApiKdn : IApiExpress
    {
        //用户ID，快递鸟提供，注意保管，不要泄漏  
        private string EBusinessID = "1809698";//即用户ID，登录快递鸟官网会员中心获取 https://www.kdniao.com/UserCenter/v4/UserHome.aspx
        //API key，快递鸟提供，注意保管，不要泄漏        
        private string ApiKey = "4e65ba98-a293-46b8-b02c-4260da8d6d98";//即API key，登录快递鸟官网会员中心获取 https://www.kdniao.com/UserCenter/v4/UserHome.aspx
        //请求url，正式环境
        private string ReqURL = "https://api.kdniao.com/Ebusiness/EbusinessOrderHandle.aspx";

        /// <summary>
        /// 查询快递信息
        /// </summary>
        /// <param name="comCode">快递公司编码</param>
        /// <param name="expressNo">物流单号</param>
        /// <param name="phone">手机号(顺丰要,寄件人/收件人都可)</param>
        public List<TrackInfo> GetTrack(string comCode, string expressNo, string phone)
        {
            // 组装应用级参数
            string requestData = "{" +
                                 "'CustomerName': ''," +
                                 "'OrderCode': ''," +
                                 $"'ShipperCode': '{comCode}'," +
                                 "'Sort': '1'," +
                                 $"'LogisticCode': '{expressNo}'," +
                                 "}";
            // 组装系统级参数
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("RequestData", HttpUtility.UrlEncode(requestData, Encoding.UTF8));
            param.Add("EBusinessID", EBusinessID);
            param.Add("RequestType", "1002"); //免费即时查询接口指令1002/在途监控即时查询接口指令8001/地图版即时查询接口指令8003
            string dataSign = encrypt(requestData, ApiKey, "UTF-8");
            param.Add("DataSign", HttpUtility.UrlEncode(dataSign, Encoding.UTF8));
            param.Add("DataType", "2");
            // 以form表单形式提交post请求，post请求体中包含了应用级参数和系统级参数
            string result = sendPost(ReqURL, param);

            ResGetTrack res = xConv.JsonToObj<ResGetTrack>(result);
            //根据公司业务处理返回的信息......
            return res.Traces;
        }

        /// <summary>
        /// Post方式提交数据，返回网页的源代码
        /// </summary>
        /// <param name="url">发送请求的 URL</param>
        /// <param name="param">请求的参数集合</param>
        /// <returns>远程资源的响应结果</returns>
        private string sendPost (string url, Dictionary<string, string> param) {
            string result = "";
            StringBuilder postData = new StringBuilder ();
            if (param != null && param.Count > 0) {
                foreach (var p in param) {
                    if (postData.Length > 0) {
                        postData.Append ("&");
                    }
                    postData.Append (p.Key);
                    postData.Append ("=");
                    postData.Append (p.Value);
                }
            }
            byte[] byteData = Encoding.GetEncoding ("UTF-8").GetBytes (postData.ToString ());
            try {

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create (url);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = url;
                request.Accept = "*/*";
                request.Timeout = 30 * 1000;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
                request.Method = "POST";
                request.ContentLength = byteData.Length;
                Stream stream = request.GetRequestStream ();
                stream.Write (byteData, 0, byteData.Length);
                stream.Flush ();
                stream.Close ();
                HttpWebResponse response = (HttpWebResponse) request.GetResponse ();
                Stream backStream = response.GetResponseStream ();
                StreamReader sr = new StreamReader (backStream, Encoding.GetEncoding ("UTF-8"));
                result = sr.ReadToEnd ();
                sr.Close ();
                backStream.Close ();
                response.Close ();
                request.Abort ();
            } catch (Exception ex) {
                result = ex.Message;
            }
            return result;
        }

        ///<summary>
        ///电商Sign签名
        ///</summary>
        ///<param name="content">内容</param>
        ///<param name="keyValue">ApiKey</param>
        ///<param name="charset">URL编码 </param>
        ///<returns>DataSign签名</returns>
        private string encrypt (String content, String keyValue, String charset) {
            if (keyValue != null) {
                return base64 (MD5 (content + keyValue, charset), charset);
            }
            return base64 (MD5 (content, charset), charset);
        }

        ///<summary>
        /// 字符串MD5加密
        ///</summary>
        ///<param name="str">要加密的字符串</param>
        ///<param name="charset">编码方式</param>
        ///<returns>密文</returns>
        private string MD5 (string str, string charset) {
            byte[] buffer = System.Text.Encoding.GetEncoding (charset).GetBytes (str);
            try {
                System.Security.Cryptography.MD5CryptoServiceProvider check;
                check = new System.Security.Cryptography.MD5CryptoServiceProvider ();
                byte[] somme = check.ComputeHash (buffer);
                string ret = "";
                foreach (byte a in somme) {
                    if (a < 16)
                        ret += "0" + a.ToString ("X");
                    else
                        ret += a.ToString ("X");
                }
                return ret.ToLower ();
            } catch {
                throw;
            }
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="str">内容</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>
        private string base64 (String str, String charset) {
            return Convert.ToBase64String (System.Text.Encoding.GetEncoding (charset).GetBytes (str));
        }
    }
}