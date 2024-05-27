using System;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.IO;
using System.Net;

namespace Qs.Comm
{
   public class Img
    {
        /// <summary>  
        /// 生成图片缩略文件  
        /// </summary>  
        /// <param name="originalImage">图片源文件</param>  
        /// <param name="width">缩略图宽度</param>  
        /// <param name="height">缩略图高度</param>  
        /// <param name="mode">生成缩略图的方式</param>  
        /// <returns>缩率处理后图片文件</returns> 
        public static Image MakeThumbnail(Image originalImage, int width, int height, ThumbnailModel mode)
        {
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumbnailModel.HighWidth: //指定高宽缩放（可能变形）   
                    break;
                case ThumbnailModel.Width: //指定宽，高按比例   
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailModel.Hight: //指定高，宽按比例  
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailModel.Default: //指定高，宽按比例  
                    if (ow <= towidth && oh <= toheight)
                    {
                        x = -(towidth - ow) / 2;
                        y = -(toheight - oh) / 2;
                        ow = towidth;
                        oh = toheight;
                    }
                    else
                    {
                        if (ow > oh)//宽大于高
                        {
                            x = 0;
                            y = -(ow - oh) / 2;
                            oh = ow;
                        }
                        else//高大于宽
                        {
                            y = 0;
                            x = -(oh - ow) / 2;
                            ow = oh;
                        }
                    }
                    break;
                case ThumbnailModel.Auto:
                    if (originalImage.Width / originalImage.Height >= width / height)
                    {
                        if (originalImage.Width > width)
                        {
                            towidth = width;
                            toheight = (originalImage.Height * width) / originalImage.Width;
                        }
                        else
                        {
                            towidth = towidth;
                            toheight = toheight;
                        }
                    }
                    else
                    {
                        if (originalImage.Height > height)
                        {
                            toheight = height;
                            towidth = (originalImage.Width * height) / originalImage.Height;
                        }
                        else
                        {
                            towidth = originalImage.Width;
                            toheight = originalImage.Height;
                        }
                    }
                    break;
                case ThumbnailModel.CutBottom: //指定高宽裁减（不变形）   
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)//切宽
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else//切高
                    {
                        ow = originalImage.Width;
                        oh = (originalImage.Width * height / towidth)- (originalImage.Height - oh);
                        x = 0;
                        y = 0 ;
                    }
                    break;
                case ThumbnailModel.Cut: //均匀切四边
                  
                        ow = originalImage.Width- ((originalImage.Width - towidth) );
                        oh = originalImage.Height - ((originalImage.Height - toheight) );
                         x = (originalImage.Height - toheight) / 2;
                        y = (originalImage.Height - toheight) / 2;
                    break;
                default:

                    break;
            }

            //新建一个bmp图片  
            Image bitmap = new Bitmap(towidth, toheight);

            //新建一个画板  
        Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法  
            g.InterpolationMode = InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度  
            g.SmoothingMode = SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充  
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分  
            g.DrawImage(originalImage, new System.DrawingCore.Rectangle(0, 0, towidth, toheight),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);

            return bitmap;
        }

        /// <summary>
        /// 缩率图处理模式
        /// </summary>
        public enum ThumbnailModel
        {
            /// <summary>
            /// 指定高宽缩放（可能变形）   
            /// </summary>
            HighWidth,

            /// <summary>
            /// 指定宽，高按比例   
            /// </summary>
            Width,

            /// <summary>
            /// 默认  全图不变形   
            /// </summary>
            Default,

            /// <summary>
            /// 指定高，宽按比例
            /// </summary>
            Hight,

            /// <summary>
            /// 均匀切四边
            /// </summary>
            Cut,
            /// <summary>
            /// 指定高宽裁减（不变形）？？指定裁剪下边
            /// </summary>
            CutBottom,
            /// <summary>
            /// 自动 原始图片按比例缩放
            /// </summary>
            Auto
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePath">保存地址</param>
        /// <param name="source">源背景图</param>
        /// <param name="qrCode">二维码图片</param>
        /// <param name="x">二维码坐标放置位置x</param>
        /// <param name="y">二维码坐标放置位置y</param>
        /// <param name="qrWidth">二维码宽</param>
        /// <param name="qrHeight">二维码高</param>
        public static string CombinImageQrCode(string basePath,Image source, Image qrCode,int x,int y,int qrWidth, int qrHeight)
        {
            string path = "";
            Image imgQrCore = qrCode;
            Graphics g = Graphics.FromImage(source);
            g.DrawImage(imgQrCore, x, y, qrWidth, qrHeight);
            string dir = $"{basePath}/Poster";
            GC.Collect();

            if (Directory.Exists(dir) == false)
            {
                Directory.CreateDirectory(dir);
            }
            string fileName = $"{xConv.NewGuid()}.png";
            source.Save($@"{dir}/{fileName}");
            return $"{Define.HttpBaseApi()}/wwwroot/Poster/{fileName}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="basePath">保存地址</param>
        /// <param name="source">源背景图</param>
        /// <param name="qrCode">二维码图片</param>
        /// <param name="x">二维码坐标放置位置x</param>
        /// <param name="y">二维码坐标放置位置y</param>
        /// <param name="qrWidth">二维码宽</param>
        /// <param name="qrHeight">二维码高</param>
        public static Image CombinImageQrCode( Image source, Image qrCode, int x, int y, int qrWidth, int qrHeight)
        {
            string path = "";
            Image imgQrCore = qrCode;
            Graphics g = Graphics.FromImage(source);
            g.DrawImage(imgQrCore, x, y, qrWidth, qrHeight);
            GC.Collect();
            return source;
        }


        public static Image BitmapToImage(Bitmap bitMap)
        {
            Image image = Image.FromHbitmap(bitMap.GetHbitmap());
            return image;
        }

        public static Image UrlToImage(string url)
        {
            WebClient mywebclient = new WebClient();
            byte[] Bytes = mywebclient.DownloadData(url);

            // using (MemoryStream ms = new MemoryStream(Bytes))
            // {
            MemoryStream ms = new MemoryStream(Bytes);
                Image outputImg = Image.FromStream(ms);
                return outputImg;
            // }
        }
    }
}
