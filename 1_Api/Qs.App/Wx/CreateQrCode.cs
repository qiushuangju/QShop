using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using   System.DrawingCore;
using System.DrawingCore.Imaging;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Repository.Vm;

namespace App.Wx
{
    public class CreateQrCode
    {
        /// <summary>
        /// 获取小程序页面二维码
        /// </summary>
        /// <param name="wxAppConfig"></param>
        /// <param name="basePath"></param>
        /// <param name="pagePath"></param>
        /// <param name="scene">小程序码参数</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ResultData GetQrCode(VmSettingBasicWxApp wxAppConfig, string basePath, string pagePath, string scene = "", int width = 168, int height = 165)
        {
            if (pagePath.Length > 128)
            {
                throw new Exception("pagePath长度过长!");
            }
            string fileName = "";
            var accessToken = Code2Session.GetAccessToken(wxAppConfig);
            var resultData = new ResultData();
            string url = $"https://api.weixin.qq.com/cgi-bin/wxaapp/createwxaqrcode?access_token={accessToken.access_token}";
            if (!string.IsNullOrEmpty(scene))
            {
                url = $"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={accessToken.access_token}";
            }
            byte[] byteImg = Post(url,
                new postData()
                {
                    page = pagePath,
                    scene= scene,
                    check_path=false
                });
            using (MemoryStream ms = new MemoryStream(byteImg))
            {
                ms.Position = 0;
                Image img = Image.FromStream(ms);
                Image imgQrCore = Img.MakeThumbnail(img, width, height, Img.ThumbnailModel.CutBottom);
                string dir = $"{basePath}/QrCodePage";
                if (System.IO.Directory.Exists(dir) == false)
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                fileName = $"/QrCodePage/{xConv.NewGuid()}.png";
                imgQrCore.Save($@"{basePath}/{fileName}");

                fileName = $"/QrCodePage/xx{xConv.NewGuid()}.png";
                imgQrCore.Save($@"{basePath}/{fileName}");


                resultData.QrCodeRelativePath = fileName;
            }
            return resultData;
        }


        public static byte[] Post(string url, postData postdata)
        {
            byte[] bytes;
            Task<byte[]> res;
            string post = xConv.ToJson(postdata);
            try
            {
                HttpContent httpContent = new StringContent(post);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    Task<byte[]> t = response.Content.ReadAsByteArrayAsync();
                    bytes = t.Result;
                    return bytes;
                }

            }
            catch (Exception em)
            {
                throw em;
            }

            return null;
        }

        public static byte[] Get(string url)
        {
            byte[] bytes;
            Task<byte[]> res;
            // string post = xConv.ToJson(postdata);
            try
            {
                // HttpContent httpContent = new StringContent(post);
                // httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                Task<byte[]> t = response.Content.ReadAsByteArrayAsync();
                bytes = t.Result;
                return bytes;


            }
            catch (Exception em)
            {
                throw em;
            }

            return null;
        }

        #region ResultData

        public class ResultData : BaseResultData
        {
            /// <summary>
            /// 相对路径
            /// </summary>
            public string QrCodeRelativePath { set; get; }
        }

        #endregion
    }

    public class postData
    {
        // public string access_token { get; set; }
        public string page { get; set; }
        public string scene { get; set; }
        public bool check_path { get; set; }
    }

    public class Item
    {
        public int Id { set; get; }
        public string Key { set; get; }
        public string Value { set; get; }
    }
}