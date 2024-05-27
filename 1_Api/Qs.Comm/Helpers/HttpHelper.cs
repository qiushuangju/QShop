using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Qs.Comm.Helpers
{
    /// <summary>
    /// http请求类
    /// </summary>
    public class HttpHelper
    {
        private HttpClient _httpClient;
        private string _baseIPAddress;

        /// <param name="ipaddress">请求的基础IP，例如：http://192.168.0.33:8080/ </param>
        public HttpHelper(string ipaddress = "")
        {
            this._baseIPAddress = ipaddress;
            if (!string.IsNullOrEmpty(ipaddress))
            {
                _httpClient = new HttpClient { BaseAddress = new Uri(_baseIPAddress) };
            }
            else
            {
                _httpClient=new HttpClient();
            }
            
        }

        /// <summary>
        /// 创建带用户信息的请求客户端
        /// </summary>
        /// <param name="userName">用户账号</param>
        /// <param name="pwd">用户密码，当WebApi端不要求密码验证时，可传空串</param>
        /// <param name="uriString">The URI string.</param>
        public HttpHelper(string userName, string pwd = "", string uriString = "")
            : this(uriString)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                _httpClient.DefaultRequestHeaders.Authorization = CreateBasicCredentials(userName, pwd);
            }
        }

        /// <summary>
        /// Get请求数据
        ///   /// <para>最终以url参数的方式提交</para>
        /// <para> 2016-3-3 重构与post同样异步调用</para>
        /// </summary>
        /// <param name="parameters">参数字典,可为空</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Get(Dictionary<string, string> parameters, string requestUri)
        {
            if (parameters != null)
            {
                var strParam = string.Join("&", parameters.Select(o => o.Key + "=" + o.Value));
                requestUri = string.Concat(ConcatUrl(requestUri), '?', strParam);
            }
            else
            {
                requestUri = ConcatUrl(requestUri);
            }

            var result = _httpClient.GetStringAsync(requestUri);
            return result.Result;
        }

        /// <summary>
        /// Get请求数据
        /// <para>最终以url参数的方式提交</para>
        /// </summary>
        /// <param name="parameters">参数字典</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns>实体对象</returns>
        public T Get<T>(Dictionary<string, string> parameters, string requestUri) where T : class
        {
            string jsonString = Get(parameters, requestUri);
            if (string.IsNullOrEmpty(jsonString))
                return null;

            return JsonHelper.Instance.Deserialize<T>(jsonString);
        }

        /// <summary>
        /// 以json的方式Post数据 返回string类型
        /// <para>最终以json的方式放置在http体中</para>
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="requestUri">例如/api/Files/UploadFile</param>
        /// <returns></returns>
        public string Post(object entity, string requestUri)
        {
            string request = string.Empty;
            if (entity != null)
                request = JsonHelper.Instance.Serialize(entity);
            HttpContent httpContent = new StringContent(request);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return Post(requestUri, httpContent);
        }

        /// <summary>
        /// 提交字典类型的数据
        /// <para>最终以formurlencode的方式放置在http体中</para>
        /// <para>于2016-07-20 19:01:59</para>
        /// </summary>
        /// <returns>System.String.</returns>
        public string PostDicObj(Dictionary<string, object> para, string requestUri)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (var item in para)
            {
                if (item.Value != null)
                {
                    if (item.Value.GetType().Name.ToLower() != "string")
                    {
                        temp.Add(item.Key, JsonHelper.Instance.Serialize(item.Value));
                    }
                    else
                    {
                        temp.Add(item.Key, item.Value.ToString());
                    }
                }
                else {
                    temp.Add(item.Key, "");
                }
            }

            return PostDic(temp, requestUri);
        }

        /// <summary>
        /// Post Dic数据
        /// <para>最终以formurlencode的方式放置在http体中</para>
        /// <para>于2016-07-15 15:28:41</para>
        /// </summary>
        /// <returns>System.String.</returns>
        public string PostDic(Dictionary<string, string> temp, string requestUri)
        {
            HttpContent httpContent = new FormUrlEncodedContent(temp);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            return Post(requestUri, httpContent);
        }

        /// <summary>
        /// Post返回byte[]
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="url"></param>
        /// <param name="isUseCert"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static byte[] PostBackByte(string xml, string url, bool isUseCert, int timeout)
        {
            byte[] btArr = null;//返回结果

            HttpWebRequest request;
            try
            {
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json;charset=UTF-8";
                string paraUrlCoded = xml;
                //byte[] payload;
                byte[] payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s = response.GetResponseStream();//返回图片数据流
                btArr = xConv.StreamToBytes(s);//将数据流转为byte[]
            }
            catch (System.Threading.ThreadAbortException e)
            {
                System.Threading.Thread.ResetAbort();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return btArr;
        }

        public string PostByte(byte[] bytes, string requestUrl)
        {
            HttpContent content = new ByteArrayContent(bytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return Post(requestUrl, content);
        }

        private string Post(string requestUrl, HttpContent content)
        {
            var result = _httpClient.PostAsync(ConcatUrl(requestUrl), content);
            return result.Result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// 把请求的URL相对路径组合成绝对路径
        /// <para>于2016-07-21 9:54:07</para>
        /// </summary>
        private string ConcatUrl(string requestUrl)
        {
            string url = _httpClient.BaseAddress==null
                ? requestUrl
                : new Uri(_httpClient.BaseAddress, requestUrl).OriginalString;
            return url;
        }

        private AuthenticationHeaderValue CreateBasicCredentials(string userName, string password)
        {
            string toEncode = userName + ":" + password;
            // The current HTTP specification says characters here are ISO-8859-1.
            // However, the draft specification for the next version of HTTP indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.GetEncoding("utf-8");
            byte[] toBase64 = encoding.GetBytes(toEncode);
            string parameter = Convert.ToBase64String(toBase64);

            return new AuthenticationHeaderValue("Basic", parameter);
        }

        public static string HttpSend(HttpSendType type, string url, string postData = "", Encoding encode = null)
        {
            string result = "";
            bool flag = encode == null;
            if (flag)
            {
                encode = Encoding.UTF8;
            }
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                switch (type)
                {
                    case HttpSendType.Post:
                        httpWebRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                        break;
                    case HttpSendType.Get:
                        httpWebRequest.Method = "GET";
                        break;
                    case HttpSendType.PostJson:
                        httpWebRequest.ContentType = "application/json;charset=utf-8";
                        break;
                }
                bool flag2 = type != HttpSendType.Get;
                if (flag2)
                {
                    byte[] bytes = encode.GetBytes(postData);
                    httpWebRequest.ContentLength = (long)bytes.Length;
                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), encode);
                result = streamReader.ReadToEnd().Trim();
                streamReader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public enum HttpSendType
        {
            [Description("Get")] Get = 101,
            [Description("Post")] Post = 201,
            [Description("PostJson")] PostJson = 202,
        }
    }
}