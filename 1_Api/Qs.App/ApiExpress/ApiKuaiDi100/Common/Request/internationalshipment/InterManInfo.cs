using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class InterManInfo
    {
        /// <summary>
        ///  姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        ///  手机号，手机号和电话号二者其一必填
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        ///  电话号，手机号和电话号二者其一必填
        /// </summary>
        public string tel { get; set; }
        /// <summary>
        ///  城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        ///  收件人所在完整地址
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        ///  州|省,可作为收件地址补充
        /// </summary>
        public string province { get; set; }
        /// <summary>
        ///  郡|县,可作为收件地址补充
        /// </summary>
        public string district { get; set; }
        /// <summary>
        ///  公司名称
        /// </summary>
        public string company { get; set; }
        /// <summary>
        ///  邮编
        /// </summary>
        public string zipcode { get; set; }
        /// <summary>
        ///  国家代号 CN-中国 ,US-美国等, 详见字典表
        /// </summary>
        public string countryCode { get; set; }
        /// <summary>
        ///  邮箱
        /// </summary>
        public string email { get; set; }
        /// <summary>
        ///  州或省代号,美国必填,例如纽约州-NY
        /// </summary>
        public string stateOrProvinceCode { get; set; }
        /// <summary>
        ///  税号
        /// </summary>
        public string taxId { get; set; }
        /// <summary>
        ///  纳税人类型
        /// </summary>
        public string taxType { get; set; }
        /// <summary>
        ///  VAT税号(数字或字母)；欧盟国家(含英国)使用的增值税号；
        /// </summary>
        public string vatNum { get; set; }
        /// <summary>
        ///  EORI号码(数字或字母)；欧盟入关时需要EORI号码，用于商品货物的清关
        /// </summary>
        public string eoriNum { get; set; }
        /// <summary>
        ///  IOSS号码
        /// </summary>
        public string iossNum { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}