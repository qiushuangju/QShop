using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qs.Comm
{
    /// <summary>
    /// 验证
    /// </summary>
   public static class xValidation
    {

        /// <summary>
        /// 检查不能为空
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void CheckStrNull(List<ValueTip> list)
        {
            string msg = "";
            bool check = true;

            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    check = false;
                    msg += $"{item.Tip},";
                }
            }
            msg = xConv.RemoveLastChar(msg);
            if (!check)
            {
                throw new Exception($"{msg},不能为空");
            }
        }

        /// <summary>
        ///  检查不能为0
        /// </summary>
        /// <param name="val"></param>
        /// <param name="tip"></param>
        public static void CheckDecimal(decimal? val,string tip)
        {
            if (xConv.ToDecimal(val)==0)
            {
                throw new Exception($"{tip},不能为0");
            }
        }

        /// <summary>
        /// 检查不能为0
        /// </summary>
        /// <param name="value"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        public static void CheckIntNull(int? value, string tip)
        {
            if (xConv.ToInt(value)==0)
            {
                throw new Exception($"{tip},不能为0");
            }
        }

        /// <summary>
        /// 检查不能为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="tip"></param>
        public static void CheckListNull<T>(this List<T> list, string tip)
        {
            if (list == null || list.Count <= 0)
            {  
              throw new Exception($"{tip},不能为空");
            }
        }

        /// <summary>
        /// 检查不能为空
        /// </summary>
        /// <param name="str"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        public static void CheckStrNull(string str,string tip)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception($"{tip},不能为空");
            }
        }
        /// <summary>
        /// 手机号码
        /// </summary>
        /// <param name="strPhone"></param>
        /// <returns></returns>
        public static void CheckPhone(string strPhone)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(strPhone, @"^1[3456789]\d{9}$"))
            {
                throw new Exception("请输入正确的手机号码");
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static void CheckPassWord(string str)
        {
            // if (str.Length < 8 || str.Length > 16)
            // {
            //     throw new Exception("密码必须大于等于8位,少于等于16位");
            // }

            //    var regex = new Regex(@"
            // (?=.*[0-9])                     #必须包含数字
            // (?=.*[a-zA-Z])                  #必须包含小写或大写字母
            // (?=([\x21-\x7e]+)[^a-zA-Z0-9])  #必须包含特殊符号
            // .{8,30}                         #至少8个字符，最多30个字符
            // ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);


                var regex = new Regex(@"
            (?=.*[0-9])                     #必须包含数字
            (?=.*[a-zA-Z])                  #必须包含小写或大写字母
            .{8,16}                         #至少8个字符，最多30个字符
            ", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            if (!regex.IsMatch(str.Trim()))
            {
                throw new Exception("密码必须包含数字,字母,并且至少8位长度");
            }




        }

        /// <summary>  
        /// 验证身份证合理性  
        /// </summary>  
        /// <param name="idNumber"></param>
        /// <returns></returns>  
        public static bool CheckIdCard(string idNumber)
        {
            if (idNumber.Length == 18)
            {
                bool check = CheckIdCard18(idNumber);
                return check;
            }
            else if (idNumber.Length == 15)
            {
                bool check = CheckIdCard15(idNumber);
                return check;
            }
            else
            {
                throw new Exception("请输入正确的身份证号码");
            }
        }


        /// <summary>  
        /// 18位身份证号码验证  
        /// </summary>  
        private static  bool CheckIdCard18(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false
                || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证  
            }
            return true;//符合GB11643-1999标准  
        }


        /// <summary>  
        /// 16位身份证号码验证  
        /// </summary>  
        private static bool CheckIdCard15(string idNumber)
        {
            long n = 0;
            if (long.TryParse(idNumber, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证  
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证  
            }
            string birth = idNumber.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证  
            }
            return true;
        }
    }

    public class ValueTip
    {
        public ValueTip(string value,string tip)
        {
            Value = value;
            Tip = tip;
        }
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 提示
        /// </summary>
        public string Tip { get; set; }
    }
}
