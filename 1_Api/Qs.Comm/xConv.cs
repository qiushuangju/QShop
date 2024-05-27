using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NPinyin;

namespace Qs.Comm
{
    /// <summary>
    ///     类型转换
    /// </summary>
    public static class xConv
    {

        /// <summary>
        /// 获取本机Ip
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            var yyyy = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()
                .Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .Where(p => p.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                            !System.Net.IPAddress.IsLoopback(p.Address)).ToList();
            string localIp = yyyy.FirstOrDefault()?.Address.ToString();
            return localIp;
        }

   


        /// <summary>
        /// 26个大写字母
        /// </summary>
        public static List<string> ListCapital = new List<string>()
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U",
            "V", "W", "X", "Y", "Z"
        };
        /// <summary>
        /// 解析省市区
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static (string province, string city, string county, string town, string village,string fullName) AnalysisProvinceCity
            (string address)
        {
            string regex = "(?<province>[^省]+自治区|.*?省|.*?行政区|.*?市)?(?<city>[^市]+自治州|.*?地区|.*?行政单位|.+盟|市辖区|.*?市|.*?县)?(?<county>[^县]+县|.+区|.+市|.+旗|.+海域|.+岛)?(?<town>[^区]+区|.+镇)?(?<village>.*)";

            var m = Regex.Match(address, regex, RegexOptions.IgnoreCase);

            var province = m.Groups["province"].Value;
            var city = m.Groups["city"].Value;
            var county = m.Groups["county"].Value;
            var town = m.Groups["town"].Value;
            var village = m.Groups["village"].Value;

            var fullName = $"{province}{city}{town}{village}";

            return (province, city, county, town, village, fullName);
        }
       
        /// <summary>
        /// 中文首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String GetSpellCode(string str)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);//注册编码对象
            Encoding gb2312 = Encoding.GetEncoding("GB2312");
            string strA = Pinyin.ConvertEncoding(str, Encoding.UTF8, gb2312);
            //首字母
            string strB = Pinyin.GetInitials(strA, gb2312);
            //拼音 
            //string strC = Pinyin.GetPinyin(str);
            return strB;
        }

        /// <summary>
        ///获取业务员Id
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public static string GetBusinessId(string sourceId)
        {
            string businessId = ToListString(sourceId).FirstOrDefault();
            return businessId;
        }
        /// <summary>
        ///获取上级Id
        /// </summary>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public static string GetParentId(string sourceId)
        {
            List<string> listUserId = ToListString(sourceId);
            string parentId = string.Join(",", listUserId);
            if (listUserId.Count > 2)
            {
                parentId = listUserId[listUserId.Count - 2];
            }
            return parentId;
        }

        /// <summary>
        /// 获取上二级Id(顺序为自己,父级,爷级)
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="userId">当前用户ID</param>
        /// <returns></returns>
        public static List<string> GetListParentId(string sourceId)
        {
            List<string> listUserId = ToListString(sourceId);
            // listUserId.Remove(userId);
            listUserId.Reverse();//倒置
            if (listUserId.Count>3)
            {
                listUserId = listUserId.GetRange(0, 3);//只获取二级
            }
            return listUserId;
        }

        /// <summary>
        ///     去除头尾空格
        /// </summary>
        /// <returns></returns>
        public static string Trim(string str)
        {
            str = ToString(str).Trim();
            return str;
        }

        /// <summary>
        ///     增加分割符
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="splitSign">分割字符</param>
        /// <returns></returns>
        public static string AddSplit(List<string> listData, string splitSign)
        {
            var temp = "";

            foreach (var data in listData)
            {
                temp +=string.IsNullOrEmpty(data)?"": data + ",";
            }

            temp = temp.Length > 0 ? temp.Substring(0, temp.Length - 1) : "";
            return temp;
        }

        /// <summary>
        ///     增加分割符
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="splitSign">分割字符</param>
        /// <returns></returns>
        public static string AddSplitSqlIn(List<string> listData, string splitSign)
        {
            var temp = "'";

            foreach (var data in listData)
            {
                temp += data + "','";
            }

            temp = temp.Length > 1 ? temp.Substring(0, temp.Length - 2) : "''";
            return temp;
        }

        /// <summary>
        ///     增加分割符
        /// </summary>
        /// <param name="listData"></param>
        /// <param name="splitSign">分割字符</param>
        /// <returns></returns>
        public static string AddSplitSqlIn(List<int?> listData, string splitSign)
        {
            var temp = "";
            foreach (var data in listData)
            {
                temp += data + ",";
            }

            temp = temp.Length > 0 ? temp.Substring(0, temp.Length - 1) : "";
            return temp;
        }

        /// <summary>
        ///     增加分割符
        /// </summary>
        /// <param name="date"></param>
        /// <param name="splitSign">分割字符</param>
        /// <returns></returns>
        public static string AddSplit(string date, string splitSign)
        {
            var temp = "";
            if (!string.IsNullOrEmpty(date))
            {
                foreach (var c in date)
                {
                    temp += c + ",";
                }

                temp = temp.Substring(0, temp.Length - 1);
            }

            return temp;
        }

        /// <summary>
        ///     去除字符串后边几位
        /// </summary>
        /// <param name="val"></param>
        /// <param name="count">去除字符数量</param>
        /// <returns></returns>
        public static string RemoveLastChar(string val, int count = 1)
        {
            var length = val.Length;
            //截取除最后一位的前面所有字符
            if (length > 0)
            {
                val = val.Substring(0, length - count);
            }

            return val;
        }

        /// <summary>
        ///     截取
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="lenth"></param>
        /// <returns></returns>
        public static string Substring(string str, int startIndex, int lenth)
        {
            if (ToString(str).Length < startIndex + lenth)
            {
                return "";
            }

            str = str.Substring(startIndex, lenth);
            return str;
        }

        /// <summary>
        ///     获取配送点ID
        /// </summary>
        public static string GetDpId(string strTsId)
        {
            var lengthDpId = 15;
            var returnStr = "";
            if (!string.IsNullOrEmpty(strTsId) && strTsId.Length >= lengthDpId)
            {
                returnStr = Substring(strTsId, 0, lengthDpId);
            }
            else
            {
                returnStr = ToString(strTsId);
            }

            return returnStr;
        }

        public static IList<T> DtToList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            var rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (var row in rows)
                {
                    var item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            var obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    var prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        var value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        //You can log something here     
                        //throw;    
                    }
                }
            }

            return obj;
        }


        /// <summary>
        ///     替换手机号中间四位为*
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        public static string ReturnPhoneNo(string phoneNo)
        {
            var re = new Regex(@"(\d{3})(\d{4})(\d{4})", RegexOptions.None);
            phoneNo = re.Replace(phoneNo, "$1****$3");
            return phoneNo;
        }

        /// <summary>
        ///     替换邮箱中间几位为*号
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string ReturnEmail(string email)
        {
            var re = new Regex(@"\w{3}(?=@\w+?.\S+)", RegexOptions.None);
            email = re.Replace(email, "****");
            return email;
        }

        /// <summary>
        ///     字符串除首尾外*替换
        /// </summary>
        /// <param name="str"></param>
        /// <param name="exceptForeAftCount">首尾数量</param>
        /// <returns></returns>
        public static string ReturnStrExcept(string str, int exceptForeAftCount)
        {
            var sb = new StringBuilder();
            var lenth = str.Length - exceptForeAftCount * 2;
            while (lenth > 0)
            {
                sb.Append("*");
                lenth--;
            }

            str = str.Substring(0, exceptForeAftCount) + sb +
                  str.Substring(str.Length - exceptForeAftCount, exceptForeAftCount);
            return str;
        }

        /// <summary>
        ///     生成编号(不保证不重复,配合唯一约束使用)
        /// </summary>
        /// <returns></returns>
        public static string GenerateNo()
        {
            var strNo = $"{DateTime.Now.ToString("yyMMddHHmmssfff")}{GenerateRandomCode(3)}";
            return strNo;
        }

        /// <summary>
        ///     生成随机字母数字编号
        /// </summary>
        /// <param name="digit">位数</param>
        public static string CreateCode(int digit)
        {
            var list = new List<char>
            {
                'B', 'C', 'E', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R', 'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6',
                '7', '8', '9'
            };
            var code = string.Empty;
            for (var i = 0; i < digit; i++)
            {
                var newChar = list.OrderBy(p => NewGuid()).FirstOrDefault();
                code += newChar;
            }

            return code;
        }

       
        /// <summary>
        ///     生成指定位数的随机码（数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }

            return result.ToString();
        }

        /// <summary>
        ///     反射实现两个类的对象之间相同属性的值的复制
        ///     适用于初始化新实体
        /// </summary>
        /// <typeparam name="TReturn">返回的实体</typeparam>
        /// <typeparam name="TIn">数据源实体</typeparam>
        /// <param name="inObj">数据源实体</param>
        /// <returns>返回的新实体</returns>
        public static TReturn CopyMapper<TReturn, TIn>(TIn inObj)
        {
            var r = Activator.CreateInstance<TReturn>(); //构造新实例
            try
            {
                if (inObj == null)
                {
                    return r;
                }
                var typeIn = inObj.GetType(); //获得类型  
                var typeReturn = typeof(TReturn);
                foreach (var sp in typeIn.GetProperties()) //获得类型的属性字段  
                {
                    foreach (var dp in typeReturn.GetProperties())
                    {
                        if (dp.Name == sp.Name && dp.PropertyType == sp.PropertyType && dp.Name != "Error" &&
                            dp.Name != "Item") //判断属性名是否相同  
                        {
                            dp.SetValue(r, sp.GetValue(inObj, null), null); //获得s对象属性的值复制给d对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return r;
        }

        /// <summary>
        /// obj1有值赋值新对象,如无obj2复制给新对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="object1"></param>
        /// <param name="object2"></param>
        /// <returns></returns>
        public static T MixObjects<T>(T src, T dts)
        {
            var srcJToken = JToken.FromObject(src);
            var dtsJToken = JToken.FromObject(dts);
            RecursivelyChangeJtokenSubPropertiesValue(srcJToken, dtsJToken);
            return dtsJToken.ToObject<T>();
        }


        private static void RecursivelyChangeJtokenSubPropertiesValue(JToken src, JToken dts)
        {
            if (src.Type == JTokenType.Array)
            {
                if (dts.SelectToken(src.Path) as JArray == null)
                    dts.SelectToken(src.Path).Replace(src);
                else foreach (var item in src as JArray)
                {
                    if (item.Children().Count() == 0 && item.Type != JTokenType.Null)
                        dts.SelectToken(item.Path).Replace(item.Value<Object>() as JToken);
                    else
                        RecursivelyChangeJtokenSubPropertiesValue(item, dts);

                }
            }
            else
            {
                foreach (var itemSrc in src.Children())
                {
                    if (itemSrc.Children().Count() == 0 && itemSrc.Type != JTokenType.Null)
                    {
                        dts.SelectToken(itemSrc.Path).Replace(itemSrc.Value<Object>() as JToken);
                    }
                    else if (dts.SelectToken(itemSrc.Path) != null)
                    {
                        RecursivelyChangeJtokenSubPropertiesValue(itemSrc, dts);
                    }
                    // else
                    // {
                    //     dts.SelectToken(src.Path).Replace(src);
                    // }
                }
            }
        }



        /// <summary>
        ///获取Model的TableAttribute Name
        /// </summary>
        /// <typeparam name="T">Model类型</typeparam>
        /// <param name="obj">枚举值</param>
        /// <returns>特性的说明</returns>
        public static string GetTableName<T>(T obj)
        {
            var type = obj.GetType();
            if (!(type.GetCustomAttributes(typeof(TableAttribute), false)[0] is TableAttribute attrTable))
            {
                return string.Empty;
            }

            return attrTable.Name;
        }

        /// <summary>
        ///     显示千分数
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal MilliShow(decimal? obj)
        {
            return ToDecimal(obj) * 1000;
        }

        /// <summary>
        ///     千分数存数据库
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal MilliDb(decimal? obj)
        {
            return ToDecimal(obj) / 1000;
        }

        /// <summary>
        ///     获取属性Description
        /// </summary>
        /// <param name="type">类的类型</param>
        /// <param name="name">属性名称</param>
        /// <returns></returns>
        public static string GetDescription(Type type, string name)
        {
            var item = type.GetProperty(name);
            var des = ((DescriptionAttribute) Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute)))
                .Description; // 属性值
            return des;
        }

        /// <summary>
        ///     MD5盐值加密
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <param name="salt">盐值</param>
        /// <returns>加密后字符串</returns>
        public static string MD5Encoding(string rawPass, object salt)
        {
            if (salt == null)
            {
                return rawPass;
            }

            return MD5Encoding(rawPass + "{" + salt + "}");
        }

        /// <summary>
        ///     MD5 加密字符串
        /// </summary>
        /// <param name="rawPass">源字符串</param>
        /// <returns>加密后字符串</returns>
        private static string MD5Encoding(string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider 
            var md5 = MD5.Create();
            var bs = Encoding.UTF8.GetBytes(rawPass);
            var hs = md5.ComputeHash(bs);
            var sb = new StringBuilder();
            foreach (var b in hs)
                // 以十六进制格式格式化 
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        /// <summary>
        ///     编号
        /// </summary>
        /// <param name="lastNumber"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static string SerialNumber(int lastNumber, int digit)
        {
            if (lastNumber <= 0)
            {
                lastNumber = 0;
            }

            var str = ToString(lastNumber + 1).PadLeft(digit, '0');
            return str;
        }

        /// <summary>
        ///     ToHex
        /// </summary>
        /// <param name="strMsg">从字符串转换到16进制表示的字符串</param>
        /// <param name="charset"> 编码,如"utf-8","gb2312"</param>
        /// <param name="isSplit">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToHex(long strMsg, int len)
        {
            var msg = strMsg.ToString($"x{len}").ToUpper();
            if (msg.Length > 2)
            {
                msg = Regex.Replace(msg, @"(\w{2})", "$1,").Trim(',');
            }

            return msg;
        }

        /// <summary>
        ///     ToHex
        /// </summary>
        /// <param name="strMsg">从字符串转换到16进制表示的字符串</param>
        /// <param name="charset"> 编码,如"utf-8","gb2312"</param>
        /// <param name="isSplit">是否每字符用逗号分隔</param>
        /// <returns></returns>
        public static string ToMachicePwd(string strMsg, int len = 8)
        {
            if (strMsg.Length < 8)
            {
                strMsg = strMsg.PadRight(len, '0');
            }

            strMsg = Regex.Replace(strMsg, @"(\w{2})", "$1,").Trim(',');
            return strMsg;
        }

        /// <summary>
        ///     ToChk
        /// </summary>
        /// <param name="strMsg">从字符串转换到16进制表示的字符串</param>
        /// <returns></returns>
        public static string ToChk(string strMsg)
        {
            var arrChk = ToListString(strMsg);
            var chk = 0;
            var arrBy = arrChk.Select(c => (byte) Convert.ToSByte(c, 16)).ToArray();
            foreach (var item in arrBy)
            {
                chk += item;
            }

            return ToHex(chk, 4);
        }

        /// <summary>
        ///     ToChk
        /// </summary>
        /// <param name="strMsg">从字符串转换到16进制表示的字符串</param>
        /// <returns></returns>
        public static byte[] ToMsg(string strMsg)
        {
            var arrChk = ToListString(strMsg);
            var arrBy = arrChk.Select(c => (byte) Convert.ToSByte(c, 16)).ToArray();
            return arrBy;
        }

        /// <summary>
        ///     16进制(Hex)
        /// </summary>
        /// <param name="hex">从16进制转换成utf编码的字符串</param>
        /// <param name="charset">编码,如"utf-8","gb2312"</param>
        /// <returns></returns>
        public static int UnHex(string hex)
        {
            return Convert.ToInt32(hex, 16);
        }

        public static string PwdAddPrefix(string pwd)
        {
            // return "#" + pwd; 原来机器加#
            return pwd;
        }

        /// <summary>
        ///     将数据流转为byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            var bytes = new List<byte>();
            var temp = stream.ReadByte();
            while (temp != -1)
            {
                bytes.Add((byte) temp);
                temp = stream.ReadByte();
            }

            return bytes.ToArray();
        }

        /// <summary>
        ///     将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        ///     图片转换成字节流
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image img)
        {
            var imgconv = new ImageConverter();
            var b = (byte[]) imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }

        /// <summary>
        ///     去除空格
        /// </summary>
        /// <param name="date">数据</param>
        public static string RemoveSpaces(object date)

        {
            return date == null ? string.Empty : date.ToString().Replace(" ", "");
        }

        /// <summary>
        ///     获取短地址
        /// </summary>
        /// <param name="fullAddress"></param>
        /// <param name="divisionLevel"></param>
        /// <returns></returns>
        public static string GetShortAddress(string fullAddress,
            xEnum.DivisionLevel divisionLevel = xEnum.DivisionLevel.District)
        {
            var lenSort = 0;
            lenSort = fullAddress.IndexOf("县", StringComparison.Ordinal) + 1;
            if (lenSort <= 1)
            {
                lenSort = fullAddress.IndexOf("区", StringComparison.Ordinal) + 1;
            }

            //if (divisionLevel == XEnum.DivisionLevel.Village)
            //{
            //    lenSort = fullAddress.IndexOf("村", StringComparison.Ordinal) + 1;
            //    if (lenSort <= 1)
            //    {
            //        lenSort = fullAddress.IndexOf("社区", StringComparison.Ordinal) + 1;
            //    }
            //}

            var sortAddress = fullAddress.Remove(0, lenSort);
            return sortAddress;
        }
        //将list转换成DataTable
        public static DataTable ListToDt<T>(IList<T> list, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();
            if (propertyName != null)
            {
                propertyNameList.AddRange(propertyName);
            }
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        //if (DBNull.Value.Equals(pi.PropertyType))
                        //{
                        //   // pi.PropertyType = DateTime;
                        //}
                        Type colType = pi.PropertyType;
                        if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        result.Columns.Add(pi.Name, colType);
                        //result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            result.Columns.Add(pi.Name, pi.PropertyType);
                        }
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }
        // /// <summary>
        // ///     list转datatable
        // /// </summary>
        // /// <typeparam name="T"></typeparam>
        // /// <param name="collection"></param>
        // /// <returns></returns>
        // public static DataTable ListToDt<T>(IEnumerable<T> collection)
        // {
        //     var props = typeof(T).GetProperties();
        //     var dt = new DataTable();
        //     dt.TableName = typeof(T).Name;
        //     dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
        //     if (collection.Count() > 0)
        //     {
        //         for (var i = 0; i < collection.Count(); i++)
        //         {
        //             var tempList = new ArrayList();
        //             foreach (var pi in props)
        //             {
        //                 var obj = pi.GetValue(collection.ElementAt(i), null);
        //                 tempList.Add(obj);
        //             }
        //
        //             var array = tempList.ToArray();
        //             dt.LoadDataRow(array, true);
        //         }
        //     }
        //
        //     return dt;
        // }


        #region 数值转换

        /// <summary>
        ///     转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int ToInt(object data)
        {
            if (data == null)
            {
                return 0;
            }

            int result;
            var success = int.TryParse(data.ToString(), out result);
            if (success)
            {
                return result;
            }

            try
            {
                return Convert.ToInt32(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        ///     转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static long ToLong(object data)
        {
            if (data == null)
            {
                return 0;
            }

            long result;
            var success = long.TryParse(data.ToString(), out result);
            if (success)
            {
                return result;
            }

            try
            {
                return Convert.ToInt64(ToDouble(data, 0));
            }
            catch (Exception)
            {
                return 0;
            }
        }

//        /// <summary>
//        /// 转换为整型
//        /// </summary>
//        /// <param name="data">数据</param>
//        public static int ToInt(decimal data)
//        {
//            try
//            {
//                return (int)(data + (decimal)0.5);
//            }
//            catch (Exception)
//            {
//                return 0;
//            }
//        }

        /// <summary>
        ///     字符串转List<int>
        /// </summary>
        /// <param name="strs">逗号隔开的数字</param>
        /// <returns></returns>
        public static List<int> ToListInt(string strs)
        {
            var listInt = new List<int>();
            if (!string.IsNullOrEmpty(strs))
            {
                strs = strs.EndsWith(",") ? strs.Substring(0, strs.Length - 1) : strs;
                listInt = strs.Split(',').ToList().Select(x => ToInt(x)).ToList();
            }

            return listInt;
        }

       
        /// <summary>
        ///   转换为List （去除空值）
        /// </summary>
        /// <param name="strs">逗号隔开的数据</param>
        /// <returns></returns>
        public static List<string> ToListString(string strs, char splitChar=',')
        {
            if (strs == null)
            {
                return new List<string>();
            }
            var listStr = string.IsNullOrEmpty(strs) ? new List<string>() : strs.Split(new char[] { splitChar }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return listStr;
        }

        /// <summary>
        ///  转换为List
        /// </summary>
        /// <param name="arrStr">逗号隔开的ListString</param>
        /// <returns></returns>
        public static List<string> ToListString(List<string> arrStr)
        {
            List<string> listId = new List<string>();
            foreach (var item in arrStr)
            {
                listId.AddRange(xConv.ToListString(item));
            }
            return listId;
        }

        /// <summary>
        ///     转换为string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToString(List<int> list)
        {
            var str = string.Join(",", list.ToArray());
            return str;
        }

        /// <summary>
        ///     List字符串转string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToString(List<string> list)
        {
            var str = string.Join(",", list.ToArray());
            return str;
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrDay(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("dd").Trim();
        }
        /// <summary>
        /// 秒转换小时
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string SecondToHour(double time)
        {
            int hour = 0;
            int minute = 0;
            int second = 0;
            second = Convert.ToInt32(time);

            if (second > 60)
            {
                minute = second / 60;
                second = second % 60;
            }
            if (minute > 60)
            {
                hour = minute / 60;
                minute = minute % 60;
            }
            return ( hour.ToString().PadLeft(2, '0') + ":" + minute.ToString().PadLeft(2, '0'));
        }

        /// <summary>
        ///     decimal字符转两位小数string
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string ToStringAmount(decimal? amount)
        {
            var dAmount = amount ?? new decimal(0);
            var strs = dAmount.ToString("F2");
            return strs;
        }

        /// <summary>
        ///     转换为可空整型
        /// </summary>
        /// <param name="data">数据</param>
        public static int? ToIntOrNull(object data)
        {
            if (data == null)
            {
                return null;
            }

            int result;
            var isValid = int.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        ///     转换为双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double ToDouble(object data)
        {
            if (data == null)
            {
                return 0;
            }

            double result;
            return double.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        ///     转换为双精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static double ToDouble(object data, int digits)
        {
            return Math.Round(ToDouble(data), digits);
        }

        /// <summary>
        ///     转换为可空双精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static double? ToDoubleOrNull(object data)
        {
            if (data == null)
            {
                return null;
            }

            double result;
            var isValid = double.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        ///     转换为高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal ToDecimal(object data)
        {
            if (data == null)
            {
                return 0;
            }

            decimal result;
            return decimal.TryParse(data.ToString(), out result) ? result : 0;
        }

        /// <summary>
        ///     转换为高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal ToDecimal(object data, int digits)
        {
            return Math.Round(ToDecimal(data), digits);
        }

        /// <summary>
        ///     转换为可空高精度浮点数
        /// </summary>
        /// <param name="data">数据</param>
        public static decimal? ToDecimalOrNull(object data)
        {
            if (data == null)
            {
                return null;
            }

            decimal result;
            var isValid = decimal.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result;
            }

            return null;
        }

        /// <summary>
        ///     转换为可空高精度浮点数,并按指定的小数位4舍5入
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="digits">小数位数</param>
        public static decimal? ToDecimalOrNull(object data, int digits)
        {
            var result = ToDecimalOrNull(data);
            if (result == null)
            {
                return null;
            }

            return Math.Round(result.Value, digits);
        }

        /// <summary>
        ///     金额显示
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToMoneyShow(object obj)
        {
            var money = ToDouble(obj);
            if (Math.Abs(money) == 0)
            {
                return "0.00";
            }

            return $"{money:N}";
        }
        /// <summary>
        ///     金额显示(非四舍五入,2位小数点舍弃)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ToMoney(object obj)
        {
            var num = xConv.ToDecimal(obj);
            var money = xConv.ToDecimal((int)(num * 100) / 100.00);
            return money;
        }

        #endregion

        #region Guid转换

        /// <summary>
        ///     Guid 32位
        /// </summary>
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        ///     新的Token
        /// </summary>
        /// <returns></returns>
        public static string NewToken()
        {
            return NewGuid().GetHashCode().ToString("x");
        }

        /// <summary>
        ///     32位随机大小写数字串
        /// </summary>
        public static string RandomStr()
        {
            var guid = Guid.NewGuid().ToString("N");
            var result = new StringBuilder();
            foreach (var item in guid)
            {
                var tmp = item.ToString();
                if (new Random().Next(0, 2) == 0)
                {
                    tmp = item.ToString().ToUpper();
                }

                result.Append(tmp);
            }

            return result.ToString();
        }

        /// <summary>
        ///     转换为Guid
        /// </summary>
        /// <param name="data">数据</param>
        public static Guid ToGuid(object data)
        {
            if (data == null)
            {
                return Guid.Empty;
            }

            Guid result;
            return Guid.TryParse(data.ToString(), out result) ? result : Guid.Empty;
        }

        /// <summary>
        ///     转换为可空Guid
        /// </summary>
        /// <param name="data">数据</param>
        public static Guid? ToGuidOrNull(object data)
        {
            if (data == null)
            {
                return null;
            }

            Guid result;
            var isValid = Guid.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result;
            }

            return null;
        }

        #endregion

        #region 日期转换

        /// <summary>
        ///     获取时间戳(13位)
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(DateTime? time = null)
        {
            var ts = ConvertDateTimeToInt(time ?? DateTime.Now);
            return ts.ToString();
        }

        /// <summary>
        ///     时间戳(10位)
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampTen(DateTime? time = null)
        {
            return ToLong(GetTimeStamp(time ?? DateTime.Now)) / 1000; //获取10位
        }

        /// <summary>
        ///     将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeToInt(DateTime time)
        {
            var startTime =
                TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
            var t = (time.Ticks - startTime.Ticks) / 10000; //除10000调整为13位      
            return t;
        }

        /// <summary>
        ///     时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        private static DateTime TimeStampToDateTime(string timeStamp)
        {
            var dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            var lTime = long.Parse(timeStamp + "0000");
            var toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        ///     转换为日期时间
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime ToDateTime(object data)
        {
            if (data == null)
            {
                return DateTime.MinValue;
            }

            DateTime result;
            return DateTime.TryParse(data.ToString(), out result) ? result : DateTime.MinValue;
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStringDateTime(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss").Trim();
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrYearMonth(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("yyyy-MM").Trim();
        }
        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrDate(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("yyyy-MM-dd").Trim();
        }
        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrMonth(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("MM").Trim();
        }
        /// <summary>
        ///     转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime ToDate(object data)
        {
            if (data == null)
            {
                return DateTime.MinValue;
            }

            var temp = new DateTime();
            var result = DateTime.TryParse(data.ToString(), out temp) ? temp : DateTime.MinValue;
            result = ToDateTime(result.ToShortDateString());
            return result;
        }

        /// <summary>
        ///     转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        public static string ToShortDate(object data)
        {
            string str = "";
            DateTime dt ;
            if (data == null)
            {
                dt= DateTime.MinValue;
            }

            var temp = new DateTime();
             dt = DateTime.TryParse(data.ToString(), out temp) ? temp : DateTime.MinValue;
             str = dt.ToString("MM-dd");
            return str;
        }

        /// <summary>
        ///     转换为可空日期
        /// </summary>
        /// <param name="data">数据</param>
        public static DateTime? ToDateOrNull(object data)
        {
            if (data == null)
            {
                return null;
            }

            DateTime result;
            var isValid = DateTime.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result;
            }

            return null;
        }

        #endregion

        #region 布尔转换

        /// <summary>
        ///     转换为布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool ToBool(object data)
        {
            if (data == null)
            {
                return false;
            }

            var value = GetBool(data);
            if (value != null)
            {
                return value.Value;
            }

            bool result;
            return bool.TryParse(data.ToString(), out result) && result;
        }

        /// <summary>
        ///     获取布尔值
        /// </summary>
        private static bool? GetBool(object data)
        {
            switch (data.ToString().Trim().ToLower())
            {
                case "0":
                    return false;
                case "1":
                    return true;
                case "是":
                    return true;
                case "否":
                    return false;
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    return null;
            }
        }

        /// <summary>
        ///     转换为可空布尔值
        /// </summary>
        /// <param name="data">数据</param>
        public static bool? ToBoolOrNull(object data)
        {
            if (data == null)
            {
                return null;
            }

            var value = GetBool(data);
            if (value != null)
            {
                return value.Value;
            }

            bool result;
            var isValid = bool.TryParse(data.ToString(), out result);
            if (isValid)
            {
                return result;
            }

            return null;
        }

        #endregion

        #region 字符串转换

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="str">数据</param>
        public static string ToString(object str)
        {
            return str == null ? string.Empty : str.ToString().Trim();
        }

        /// <summary>
        ///     转换为日期
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="len">长度</param>
        public static string ToShortStr(object data,int len=10)
        {
            string str = xConv.ToString(data);
            str=str.Length>0&&str.Length>len? $"{str.Substring(0, len)}...":str;
            return str;
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStringDate(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("yyyy-MM-dd").Trim();
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrDateTime(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("yyyy-MM-dd HH:mm:ss").Trim();
        }
        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrTime(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("HH:mm:ss").Trim();
        }
        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrTimeMinute(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("yyyy-MM-dd HH:mm").Trim();
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrMMdd(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("MM-dd").Trim();
        }

        /// <summary>
        ///     转换为字符串
        /// </summary>
        /// <param name="date">数据</param>
        public static string ToStrHHmm(object date)
        {
            return date == null ? string.Empty : ToDateTime(date).ToString("HH:mm").Trim();
        }

        #endregion

        #region 通用转换

        /// <summary>
        ///     将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"});
        }

        /// <summary>
        ///     json转为对象
        /// </summary>
        /// <typeparam name="TObjType"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static TObjType JsonToObj<TObjType>(string jsonString) where TObjType : new()
        {
            try
            {
                if (string.IsNullOrEmpty(jsonString))
                {
                    return new TObjType();
                }
                var res = JsonConvert.DeserializeObject<TObjType>(jsonString, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Replace });
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(@"参数类型{0}格式不合法", typeof(TObjType)));
            }
        }

        #endregion
    }
}