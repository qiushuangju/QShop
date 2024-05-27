using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Qs.Comm.Helpers
{
    public static class Utils
    {
        private static readonly Random Random = new Random();

        /// <summary>
        /// 生成主键Guid 32位小写无横线
        /// </summary>
        /// <returns></returns>
        public static string GuidNew()
        {
            string guid= Guid.NewGuid().ToString("N");
            return guid;
        }

      
        /// <summary>
        /// 裁剪字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string TrimMaxLength(this string str,int length)
        {
            if(string.IsNullOrWhiteSpace(str))
                return str;
            string newStr = str.Trim();
            if (newStr.Length > length)
            {
                return newStr.Substring(0, length);
            }
            else
                return newStr;
        }

        /// <summary>
        /// 删除字符串最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StrRemoveLastChar(string str)
        {
            string strReturn = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                strReturn = str.Remove(str.LastIndexOf(",", StringComparison.Ordinal), 1);
            }
            return strReturn;
        }

        public static string EnsureJsVarStr(string var)
        {
            if (string.IsNullOrEmpty(var))
                return var;
            return var.Replace("\\", "\\\\").Replace("\r", "").Replace("\n", "\\n").Replace("'", "\\'").Replace("\"", "&quot;");
        }


       /// <summary>
        /// 根据日期+随机数生成序号
       /// </summary>
       /// <param name="length">序号长度</param>
       /// <returns></returns>
        public static long GetSerialNumber(int length)
        {
            string serialNumberTime = DateTime.Now.ToString("yyyyMMdd");
            string serialNumberGuid = GuidToLongId().ToString().Substring(1, length-8);
            //            select top 1000000 left(abs(checksum(newid())),7) from syscolumns a ,syscolumns b,syscolumns c,syscolumns d,syscolumns e
            return long.Parse(serialNumberTime + serialNumberGuid);
        }

        /// <summary>
        /// 根据GUID获取16位的唯一字符串
        /// </summary>
        /// <returns></returns>
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        /// <summary> 
        /// 根据GUID获取19位的唯一数字序列 
        /// </summary> 
        /// <returns></returns> 
        public static long GuidToLongId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>
        /// 按比例生成1,2,3,比例总和为100
        /// </summary>
        /// <param name="proportion1">结果1的比例(如10%,则传入10)</param>
        /// <param name="proportion2">结果2的比例</param>
        /// <returns>按传入比例随机生成结果</returns>
        public static int RandomInProportion(int proportion1, int proportion2)
        {
            int num = 0;
            int randomNum = Random.Next(1, 100);
            if (randomNum <=proportion1)
            {
                num = 1;
            }
            else if (randomNum <= proportion2 + proportion1)
            {
                num = 2;
            }
            else
            {
                num = 3;
            }
            return num;
        }

        /// <summary>
        /// 随机生成数字
        /// </summary>
        /// <returns></returns>
        public static int RandomNumByMinMax(int minNum, int maxNum)
        {
            
            int randomNum = Random.Next(minNum, maxNum);
            return randomNum;
        }

        

        /// <summary>
        /// 生成单号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="lastSuffix">上一个单号的后缀编码</param>
        /// <returns>prefix+YYYYMMDD+(lastSuffix+1)</returns>
        public static string GenerateNo(string prefix, string lastSuffix)
        {
            string no = "";
            string strDate = DateTime.Now.ToString("yyyyMMdd");
            int suffix = 0;
            if (string.IsNullOrEmpty(lastSuffix))
            {
                suffix = 1;
            }
            else
            {
                suffix = Convert.ToInt32(lastSuffix.Remove(0, lastSuffix.Length - 3)) + 1;
            }

            string strSuffix= string.Format("{0:000}", suffix);
            no = prefix + strDate + strSuffix;
            return no;
        }


     
        

        #region URL请求数据
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;

            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();

                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }

            return responseStr;
        }

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            //request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            WebResponse response = null;
            string responseStr = null;

            try
            {
                response = request.GetResponse();

                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }

            return responseStr;
        }

       
        #endregion


    }
}