using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Comm.Helpers;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Live.V20180801;
using TencentCloud.Live.V20180801.Models;

namespace Qs.App.Wx
{
    /// <summary>
    ///腾讯IM接口
    /// </summary>
    public class TencentImApi
    {
        ///  <summary>
        /// IM单账号导入
        ///  </summary>
        ///  <param name="imUserId">Im用户Id</param>
        ///  <param name="nickName"></param>
        ///  <param name="urlAvater"></param>
        ///  <returns></returns>
        public string ImAccountImport(string imUserId,string nickName,string urlAvater)
        {
            
            TencentUserSigApIv2 api = new TencentUserSigApIv2(Define.ImSdkAppId, Define.ImSdkKey);
            
            string random=   xConv.GenerateRandomCode(9);
                  https://console.tim.qq.com/
            string userSig = api.GenUserSig(Define.ImAdminUserId);  
            string url =
                $" https://console.tim.qq.com/v4/im_open_login_svc/account_import?sdkappid={Define.ImSdkAppId}&identifier={Define.ImAdminUserId}&usersig={userSig}&random={random}&contenttype=json";
            var reqData = new
            {
                Identifier = imUserId,
                Nick = nickName,
                FaceUrl = urlAvater
            };
            string result = HttpHelper.HttpSend(HttpHelper.HttpSendType.PostJson, url,xConv.ToJson(reqData));
            return result;
        }


        /// <summary>
        /// 直播中数据
        /// </summary>
        /// <returns></returns>
        public List<StreamOnlineInfo> ListLiveOnline()
        {
            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = ("live.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;      
            LiveClient client = new LiveClient(Define.TengXunCred, "", clientProfile);
            DescribeLiveStreamOnlineListRequest req = new DescribeLiveStreamOnlineListRequest()
            {
                PageNum = 1,
                PageSize = 100
            };        
            DescribeLiveStreamOnlineListResponse resp = client.DescribeLiveStreamOnlineListSync(req);
            List<StreamOnlineInfo> list = new List<StreamOnlineInfo>();
            for (int i = 0; i < resp.OnlineInfo.Length; i++)
            {
                StreamOnlineInfo online=  resp.OnlineInfo[i];
                list.Add(online);
            }

            //流名称
            //1400570989_s2112142203588620016341
            //1400570989_u2112082321284608096755
            List<string> listStreamName = list.Select(p => p.StreamName).ToList();
            return list;
        }
    }
}
