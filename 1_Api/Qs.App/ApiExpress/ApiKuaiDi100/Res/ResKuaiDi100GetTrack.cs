using System;
using System.Collections.Generic;

namespace Qs.App.ApiExpress.ApiKuaiDi100.Res
{
    /// <summary>
    /// 快递100 返回参数
    /// </summary>
    public class ResKuaiDi100GetTrack  : ResBaseKuaiDi100GetTrack
    {
        public List<KuaiDi100TrackInfo> data { get; set; }
    }
    /// <summary>
    /// 快递100 返回公共参数
    /// </summary>
    public class ResBaseKuaiDi100GetTrack
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 通讯状态  200:正常
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 物流状态 (0:在途，1:揽收，2:疑难，3:签收，4:退签，5:派件，8:清关，14:拒签)
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// 快递公司编号
        /// </summary>
        public string com { get; set; }
        /// <summary>
        /// 物流单号
        /// </summary>
        public string nu { get; set; }

    }

    /// <summary>
    /// 快递100 返回轨迹
    /// </summary>
    public class KuaiDi100TrackInfo
    {
        /// <summary>
        /// 快递时间
        /// </summary>
          public DateTime ftime { get; set; }
          /// <summary>
          /// 快递信息
          /// </summary>
          public string context { get; set; }
    }
}
