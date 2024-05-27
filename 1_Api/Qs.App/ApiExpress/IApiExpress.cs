using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.App.ApiKuaiDiNiao.Res;

namespace Qs.App.ApiExpress
{
    /// <summary>
    ///快递信息接口
    /// </summary>
    public interface IApiExpress
    {
        /// <summary>
        /// 查询快递轨迹信息
        /// </summary>
        /// <param name="comCode">快递公司编码</param>
        /// <param name="expressNo">物流单号</param>
        /// <param name="phone">手机号(顺丰要,寄件人/收件人都可)</param>
        public List<TrackInfo> GetTrack(string comCode, string expressNo, string phone);

        // /// <summary>
        // ///获取快递最后轨迹
        // /// </summary>
        // /// <param name="orderSkuId">订单SkuId</param>
        // /// <returns></returns>
        // public TrackInfo GetLasetTrack(string orderSkuId);
    }
}
