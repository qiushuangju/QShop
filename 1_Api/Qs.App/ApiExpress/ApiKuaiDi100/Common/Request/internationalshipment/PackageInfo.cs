using Newtonsoft.Json;

namespace Common.Request.internationalshipment
{
    public class PackageInfo
    {
        /// <summary>
        ///  高度 {get; set;}单位厘米,默认1.0
        /// </summary>
        public double height { get; set; }
        /// <summary>
        ///  宽度 {get; set;}单位厘米, 默认10.0
        /// </summary>
        public double width { get; set; }
        /// <summary>
        ///  长度 {get; set;}单位厘米默认10.0
        /// </summary>
        public double length { get; set; }
        /// <summary>
        ///  重量 {get; set;} 单位千克,默认0.1
        /// </summary>
        public double weight { get; set; }
        /// <summary>
        ///  该包裹的备注信息之类
        /// </summary>
        public string packageReference { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
    }

}