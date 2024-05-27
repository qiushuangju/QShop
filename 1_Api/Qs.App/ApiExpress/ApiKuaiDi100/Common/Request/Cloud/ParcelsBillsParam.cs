using Newtonsoft.Json;
using System.Collections.Generic;

namespace Common.Request.cloud
{
    public class ParcelsBillsParam<T> {
        /// <summary>
        /// 打印设备，通过打印机输出的设备码进行获取
        /// </summary>
        public string siid {get; set;}
        
        /// <summary>
        /// 通过管理后台的打印发货单模板配置信息获取
        /// </summary>
        public string tempid {get; set;}
        
        /// <summary>
        /// 设备码
        /// </summary>
        public List<T> tb0 {get; set;}
        
        /// <summary>
        /// 设备码
        /// </summary>
        public ImageInfo img0 {get; set;}
        
        /// <summary>
        /// 打印状态回调地址，默认仅支持http
        /// </summary>
        public string callBackUrl {get; set;}

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}