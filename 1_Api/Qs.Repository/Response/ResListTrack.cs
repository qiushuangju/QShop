using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.App.ApiKuaiDiNiao.Res;

namespace Qs.Repository.Response
{
   public class ResListTrack
    {
        /// <summary>
        /// 快递公司
        /// </summary>
        public string ExpressName { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string ExpressNo { get; set; }

        /// <summary>
        /// 物流信息
        /// </summary>
        public List<TrackInfo> ListInfo { get; set; }
    }
}
