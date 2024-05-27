using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Http;

namespace Qs.App.Wx
{
    /** 
    '============================================================================
    'api说明：
    'getKey()/setKey(),获取/设置密钥
    'getParameter()/setParameter(),获取/设置参数值
    'getAllParameters(),获取所有参数
    'isTenpaySign(),是否正确的签名,true:是 false:否
    'isWXsign(),是否正确的签名,true:是 false:否
    ' * isWXsignfeedback判断微信维权签名
    ' *getDebugInfo(),获取debug信息
    '============================================================================
    */

    internal class ResponseHandler
    {
        // 密钥 
        private string key;
        //参与签名的参数列表
        //private static string SignField = "appid,appkey,timestamp,openid,noncestr,issubscribe";
        // 微信服务器编码方式
        private readonly string charset = "gb2312";

        //参与签名的参数列表
        protected HttpContext httpContext;
        //protected Hashtable parameters;
        private Hashtable xmlMap;

        //获取页面提交的get和post参数
        public ResponseHandler(string xml)
        {
            //parameters = new Hashtable();
            xmlMap = new Hashtable();
           
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xml);
                XmlNode root = xmlDoc.SelectSingleNode("xml");
                XmlNodeList xnl = root.ChildNodes;


                foreach (XmlNode xnf in xnl)
                {
                    xmlMap.Add(xnf.Name, xnf.InnerText);
                }
        }

        #region 参数=======================================
        /// <summary>
        /// 初始化加载
        /// </summary>
        public virtual void init()
        {
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <returns></returns>
        public string getKey()
        {
            return key;
        }

        /// <summary>
        /// 设置密钥
        /// </summary>
        /// <param name="key"></param>
        public void setKey(string key)
        {
            this.key = key;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string getParameter(string parameter)
        {
            string s = (string)xmlMap[parameter];
            return (null == s) ? "" : s;
        }

        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="parameterValue"></param>
        public void setParameter(string parameter, string parameterValue)
        {
            //if (parameter != null && parameter != "")
            //{
            //    if (parameters.Contains(parameter))
            //    {
            //        parameters.Remove(parameter);
            //    }

            //    parameters.Add(parameter, parameterValue);
            //}
        }
        #endregion

        #region 辅助方法===================================
        /// <summary>
        /// 判断微信签名
        /// </summary>
        /// <returns></returns>
        public virtual bool isWXsign(out string error)
        {
            

            StringBuilder sb = new StringBuilder();
            Hashtable signMap = new Hashtable();
            foreach (string k in xmlMap.Keys)
            {
                if (k != "sign")
                {
                    signMap.Add(k.ToLower(), xmlMap[k]);
                }
            }

            ArrayList akeys = new ArrayList(signMap.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)signMap[k];
                sb.Append(k + "=" + v + "&");
            }
            sb.Append("key=" + key);

//            Log.Append("Notify 页面  返回数据：" + sb.ToString() + "、sign： " + xmlMap["sign"].ToString());

            string sign = SignMd5(sb.ToString()).ToUpper();
            error = "sign = " + sign + "\r\n xmlMap[sign]=" + xmlMap["sign"].ToString();
            return sign.Equals(xmlMap["sign"]);

        }

        /**
      * @生成签名，详见签名生成算法
      * @return 签名, sign字段不参加签名
      */
        public static string SignMd5(string str)
        {

            //MD5加密
            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }
            //所有字符转为大写
            return sb.ToString().ToUpper();
        }
        #endregion
    }
}