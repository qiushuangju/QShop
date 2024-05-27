using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Request.Label
{
    public class CustomPrintParam
    {
        /// <summary>
        ///  自定义参数，优先级高于系统生成值，即出现相同key时，使用该参数的value
        /// </summary>
        /// <value></value>
        public Dictionary<string, object> customParam;
        /// <summary>
        ///  打印方向（默认0） 0-正方向 1-反方向
        /// </summary>
        /// <value></value>
        public string direction;
        /// <summary>
        ///  打印设备，通过打印机输出的设备码进行获取
        /// </summary>
        /// <value></value>
        public string siid;
        /// <summary>
        ///  打印状态回调地址
        /// </summary>
        /// <value></value>
        public string callBackUrl;
        /// <summary>
        ///  快递100模板url
        /// </summary>
        /// <value></value>
        public string tempId;
        /// <summary>
        ///  打印类型（HTML,IMAGE,CMD,CLOUD,NON）
        ///  NON:只下单不打印（默认）
        ///  HTML:生成html短链
        ///  IMAGE:生成图片短链
        ///  CMD:生成打印指令
        ///  CLOUD:使用快递100云打印机打印，使用CLOUD时siid必填
        /// </summary>
        /// <value></value>
        public string printType;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}