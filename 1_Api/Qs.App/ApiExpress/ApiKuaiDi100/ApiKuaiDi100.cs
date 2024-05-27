using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Request;
using Qs.App.ApiExpress.ApiKuaiDi100.Res;
using Qs.App.ApiKuaiDiNiao.Res;
using Qs.Comm;
using Utils;

namespace Qs.App.ApiExpress.ApiKuaiDi100
{
    /// <summary>
    /// 快递100接口
    /// </summary>
    public class ApiKuaiDi100 : IApiExpress
    {
        private  KuaiDi100Config config = new KuaiDi100Config()
        {
            key = "qYZuPWnS2084",
            customer = "0203C7C02218662E0D8E06D013FCEAA2",
            secret = "99298d6bb8314725ba5ebf6be9f18bf8",
            userid = "4415030fdd8c4493a820fffdc94a89bc",
            siid = "",
            tid = "",
        };

        /// <summary>
        /// 查询快递信息
        /// </summary>
        /// <param name="comCode">快递公司编码</param>
        /// <param name="expressNo">物流单号</param>
        /// <param name="phone">手机号(顺丰要,寄件人/收件人都可)</param>
        public List<TrackInfo> GetTrack(string comCode, string expressNo, string phone)
        {
           
            List<TrackInfo> list = new List<TrackInfo>();

            var queryTrackParam = new QueryTrackParam()
            {                                                                                                                                                                              
                com = $"{comCode}",
                num = $"{expressNo}",
                phone = $"{phone}"
            };

           string strResult = QueryTrack.query(new QueryTrackReq()
            {
                customer = config.customer,
                sign = SignUtils.GetMD5(queryTrackParam.ToString() + config.key + config.customer),
                param = queryTrackParam
            });
           ResBaseKuaiDi100GetTrack baseResult = xConv.JsonToObj<ResBaseKuaiDi100GetTrack>(strResult);
           if (baseResult.status!=200)
           {
               list.Add(new TrackInfo() { AcceptStation = "暂无轨迹信息", AcceptTime = DateTime.Now });
            }
           else
           {
               ResKuaiDi100GetTrack result = xConv.JsonToObj<ResKuaiDi100GetTrack>(strResult);
                foreach (var item in result.data.OrderByDescending(p => p.ftime))
               {
                   list.Add(new TrackInfo() { AcceptTime = item.ftime, AcceptStation = item.context });
               }
            }
           
           return list;
        }

    }
}
