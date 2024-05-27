﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Qs.Comm;
using Qs.Repository.Vm;

using WxPayAPI;

namespace Qs.App.Wx
{
    /// <summary>
    /// 微信支付协议接口数据类，所有的API接口通信都依赖这个数据结构，
    /// 在调用接口之前先填充各个字段的值，然后进行接口通信，
    /// 这样设计的好处是可扩展性强，用户可随意对协议进行更改而不用重新设计数据结构，
    /// 还可以随意组合出不同的协议数据包，不用为每个协议设计一个数据包结构
    /// </summary>
    public class WxPayData
    {
        public WxPayData()
        {

        }

        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private SortedDictionary<string, object> m_values = new SortedDictionary<string, object>();

        /**
        * 设置某个字段的值
        * @param key 字段名
         * @param value 字段值
        */
        public void SetValue(string key, object value)
        {
            m_values[key] = value;
        }

        /**
        * 根据字段名获取某个字段的值
        * @param key 字段名
         * @return key对应的字段值
        */
        public object GetValue(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            return o;
        }

        /**
         * 判断某个字段是否已设置
         * @param key 字段名
         * @return 若字段key已被设置，则返回true，否则返回false
         */
        public bool IsSet(string key)
        {
            object o = null;
            m_values.TryGetValue(key, out o);
            if (null != o)
                return true;
            else
                return false;
        }

        /**
        * @将Dictionary转成xml
        * @return 经转换得到的xml串
        * @throws WxPayException
        **/
        public string ToXml()
        {
            //数据为空时不能转化为xml格式
            if (0 == m_values.Count)
            {
                WxLog.Error(this.GetType().ToString(), "WxPayData数据为空!");
                throw new WxPayException("WxPayData数据为空!");
            }

            string xml = "<xml>";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                //字段值不能为null，会影响后续流程
                if (pair.Value == null)
                {
                    WxLog.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Value.GetType() == typeof(int))
                {
                    xml += "<" + pair.Key + ">" + pair.Value + "</" + pair.Key + ">";
                }
                else if (pair.Value.GetType() == typeof(string))
                {
                    xml += "<" + pair.Key + ">" + "<![CDATA[" + pair.Value + "]]></" + pair.Key + ">";
                }
                else//除了string和int类型不能含有其他数据类型
                {
                    WxLog.Error(this.GetType().ToString(), "WxPayData字段数据类型错误!");
                    throw new WxPayException("WxPayData字段数据类型错误!");
                }
            }
            xml += "</xml>";
            return xml;
        }

        /**
        * @将xml转为WxPayData对象并返回对象内部的数据
        * @param string 待转换的xml串
        * @return 经转换得到的Dictionary
        * @throws WxPayException
        */
        public SortedDictionary<string, object> FromXml(string xml, VmSettingBasicWxApp setting)
        {
            if (string.IsNullOrEmpty(xml))
            {
                WxLog.Error(this.GetType().ToString(), "将空的xml串转换为WxPayData不合法!");
                throw new WxPayException("将空的xml串转换为WxPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;//获取到根节点<xml>
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = (XmlElement)xn;
                m_values[xe.Name] = xe.InnerText;//获取xml的键值对到WxPayData内部的数据中
            }
			
            try
            {
				//2015-06-29 错误是没有签名
				if(m_values["return_code"] != "SUCCESS")
				{
					return m_values;
				}
                CheckSign( setting);//验证签名,不通过会抛异常
            }
            catch(WxPayException ex)
            {
                throw new WxPayException(ex.Message);
            }

            return m_values;
        }

        /**
        * @Dictionary格式转化成url参数格式
        * @ return url格式串, 该串不包含sign字段值
        */
        public string ToUrl()
        {
            string buff = "";
            foreach (KeyValuePair<string, object> pair in m_values)
            {
                if (pair.Value == null)
                {
                    WxLog.Error(this.GetType().ToString(), "WxPayData内部含有值为null的字段!");
                    throw new WxPayException("WxPayData内部含有值为null的字段!");
                }

                if (pair.Key != "sign" && pair.Value.ToString() != "")
                {
                    buff += pair.Key + "=" + pair.Value + "&";
                }
            }
            buff = buff.Trim('&');
            return buff;
        }


      
        /**
        * @生成签名，详见签名生成算法
        * @return 签名, sign字段不参加签名
        */
        public string MakeSign(VmSettingBasicWxApp setting)
        {
            //转url格式
            string str = ToUrl();
            //在string后加入API KEY
            str += "&key=" + setting.PartnerAppKey;
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

        /**
        * 
        * 检测签名是否正确
        * 正确返回true，错误抛异常
        */
        public bool CheckSign(VmSettingBasicWxApp setting)
        {
            //如果没有设置签名，则跳过检测
            if (!IsSet("sign"))
            {
                WxLog.Error(this.GetType().ToString(), "WxPayData签名存在但不合法!");
               throw new WxPayException("WxPayData签名存在但不合法!");
            }
            //如果设置了签名但是签名为空，则抛异常
            else if(GetValue("sign") == null || GetValue("sign").ToString() == "")
            {
                WxLog.Error(this.GetType().ToString(), "WxPayData签名存在但不合法!");
                throw new WxPayException("WxPayData签名存在但不合法!");
            }

            //获取接收到的签名
            string return_sign = GetValue("sign").ToString();

            //在本地计算新的签名
            string cal_sign = MakeSign(setting);

            if (cal_sign == return_sign)
            {
                return true;
            }

            WxLog.Error(this.GetType().ToString(), "WxPayData签名验证错误!");
            throw new WxPayException("WxPayData签名验证错误!");         
        }

        /**
        * @获取Dictionary
        */
        public SortedDictionary<string, object> GetValues()
        {
            return m_values;
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="ticket">ticket</param>
        /// <param name="noncestr">随机字符串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSignature(string ticket, string noncestr, long timestamp, string url)
        {
            var string1Builder = new StringBuilder();
            //拼接字符串
            string1Builder.Append("jsapi_ticket=").Append(ticket).Append("&")
                .Append("noncestr=").Append(noncestr).Append("&")
                .Append("timestamp=").Append(timestamp).Append("&")
                .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
            string str = string1Builder.ToString();
            return SHA1(str);//加密
        }


        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }

        /// <summary>
        /// SHA1 加密
        /// </summary>
        /// <param name="content">需要加密字符串</param>
        /// <param name="encode">指定加密编码</param>
        /// <returns>返回40位小写字符串</returns>
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "").ToLower();//转小写
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
    }
}