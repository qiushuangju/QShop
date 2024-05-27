using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.App.ApiKuaiDiNiao.Res
{
    /// <summary>
    ///查询轨迹接口返回值
    /// </summary>
    public class ResGetTrack : ResBaseKdnBase
    {
        
        /// <summary>
        ///轨迹信息
        /// </summary>
        public List<TrackInfo> Traces { get; set; }
    }

    /// <summary>
    /// 快递鸟基础返回值
    /// </summary>
    public class ResBaseKdnBase
    {
        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticCode { get; set; }
        /// <summary>
        /// 物流公司编号
        /// </summary>
        public string ShipperCode { get; set; }
        /// <summary>
        /// 状态 (0:暂无轨迹信息,1:已揽收,2:在途中,3:签收,4:问题件,5:转寄,6:清关  )
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
    }

       /// <summary>
       /// 轨迹信息
       /// </summary>
   public class TrackInfo
   {
        /// <summary>
        /// 轨迹描述
        /// </summary>
        public string AcceptStation { get; set; }
        /// <summary>
        /// 轨迹时间
        /// </summary>
        public DateTime AcceptTime { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        public string Location { get; set; }
    }
}
