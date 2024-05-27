using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Vm
{
    public class VmPage
    {
        /// <summary>
        /// 好像没用
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 页面参数
        /// </summary>
        public ParamsObj paramsData { get; set; }

        /// <summary>
        ///标题栏样式
        /// </summary>
        public TitleStyle style { get; set; }


        /// <summary>
        /// 页面标题栏默认数据
        /// </summary>
        /// <returns></returns>
        public static dynamic GetDefaultPage()
        {
            var xx = new
            {
                Page = new VmPage
                {
                    name = "页面设置",
                    type = "page",
                    paramsData = new ParamsObj
                    {
                        name = "页面名称",
                        title = "页面标题",
                        shareTitle = "分享标题",
                    },
                    style = new TitleStyle
                    {
                        titleTextColor = "black",
                        titleBackgroundColor = "#ffffff"
                    }
                },
                Items = GetDefaultItems()
            };
            return xx;

        }

        /// <summary>
        /// 页面diy元素默认数据
        /// </summary>
        /// <returns></returns>
        public static dynamic GetDefaultItems()
        {
            var items = new
            {
                search = new
                {
                    Name = "搜索框",
                    Type = "search",
                    Style = new
                    {
                        TextAlign = "left",
                        SearchStyle = "square"
                    },
                    paramsData = new
                    {
                        placeholder = "请输入关键字进行搜索"
                    }

                },
                searchAddr = new
                {
                    Name = "搜索框-带定位",
                    Type = "searchAddr",
                    Style = new
                    {
                        TextAlign = "left",
                        SearchStyle = "square"
                    },
                    paramsData = new
                    {
                        placeholder = "请输入关键字进行搜索"
                    }

                },
                banner = new
                {
                    Name = "图片轮播",
                    Type = "banner",
                    Style = new
                    {
                        btnColor = "#ffffff",
                        btnShape = "round",
                        interval = 2.5
                    },
                    paramsData = new[]
                    {
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/banner/01.png",
                            link = ""
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/banner/01.png",
                            link = ""
                        }
                    } ,
                    
                },
                fullBannerSearchAddr = new
                {
                    Name = "到顶轮播-搜索带定位",
                    Type = "fullBannerSearchAddr",
                    Style = new
                    {
                        searchTextAlign = "left",
                        searchStyle = "square",
                        btnColor = "#ffffff",
                        btnShape = "round",
                        interval = 2.5,
                       
                    },
                    paramsData = new[]
                    {
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/banner/01.png",
                            link = ""
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/banner/01.png",
                            link = ""
                        }
                    }   ,
                    paramsData1 = new
                    {
                        placeholder = "请输入关键字进行搜索"
                    }
                },
                image = new
                {
                    Name = "图片",
                    Type = "image",
                    Style = new
                    {
                        paddingTop = 0,
                        paddingLeft = 0,
                        background = "#ffffff"
                    },
                    paramsData = new[]
                    {
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/banner/01.png",
                            imgName = "image-1.jpg",
                            link = "",
                        }
                    }
                },
                navBar = new
                {
                    Name = "导航组",
                    Type = "navBar",
                    Style = new
                    {
                        rowsNum = 4,
                        background = "#ffffff",
                        paddingTop = 0,
                        textColor = "#666666"
                    },
                    paramsData = new[]
                    {
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/navbar/01.png",
                            imgName = "icon-1.png",
                            text = "按钮文字1",
                            link = "",
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/navbar/01.png",
                            imgName = "icon-2.jpg",
                            text = "按钮文字2",
                            link = "",
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/navbar/01.png",
                            imgName = "icon-3.jpg",
                            text = "按钮文字3",
                            link = "",
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/navbar/01.png",
                            imgName = "icon-4.jpg",
                            text = "按钮文字4",
                            link = "",
                        }

                    }
                },
                blank = new
                {
                    Name = "辅助空白",
                    Type = "blank",
                    Style = new
                    {
                        height = 20,
                        background = "#ffffff"
                    }
                },
                guide = new
                {
                    Name = "辅助线",
                    Type = "guide",
                    Style = new
                    {
                        background = "#ffffff",
                        lineStyle = "solid",
                        lineHeight = 1,
                        lineColor = "#000000",
                        paddingTop = 10
                    }
                },
                video = new
                {
                    name = "视频组",
                    type = "video",
                    paramsData = new
                    {
                        videoUrl =
                            "http=//wxsnsdy.tc.qq.com/105/20210/snsdyvideodownload?filekey=30280201010421301f0201690402534804102ca905ce620b1241b726bc41dcff44e00204012882540400.mp4",
                        poster = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/video_poster.png",
                        autoplay = 0,
                    },
                    style = new
                    {
                        paddingTop = 0,
                        height = 190
                    }
                },
                article = new
                {
                    name = "文章组",
                    type = "article",
                    paramsData = new
                    {
                        source = "auto", //数据来源 (choice指定  auto自动获取)
                        auto = new
                        {
                            category = -1, // -1全部
                            showNum = 6
                        }
                    },
                    defaultData = new[]
                    {
                        new
                        {
                            title = "此处显示文章标题",
                            show_type = 10,
                            image = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/article/01.png",
                            views_num = 309

                        },
                        new
                        {
                            title = "此处显示文章标题",
                            show_type = 10,
                            image = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/article/01.png",
                            views_num = 309

                        }
                    },
                    data = new ArrayList()
                },
                notice = new
                {
                    Name = "店铺公告",
                    Type = "notice",
                    paramsData = new
                    {
                        text = "这里是第一条自定义公告的标题", // 公告内容
                        // "icon" = base_url() . "wwwroot/ImgDefault/diy/notice.png",
                        link = "", // 链接
                        showIcon = true, // 是否显示图标
                        scrollable = true // 是否滚动 files
                    },
                    style = new
                    {
                        paddingTop = 0,
                        background = "#fffbe8",
                        textColor = "#de8c17"
                    }
                },
                richText = new
                {
                    name = "富文本",
                    type = "richText",
                    paramsData = new { content = "<p>这里是文本的内容</p>" },
                    style =
                        new
                        {
                            paddingTop = 0,
                            paddingLeft = 0,
                            background = "#ffffff"
                        },

                },
                window = new
                {
                    name = "富文本",
                    type = "window",
                    dataNum = 4,
                    style =
                        new
                        {
                            paddingTop = 0,
                            paddingLeft = 0,
                            background = "#ffffff",
                            layout = 2
                        },
                    paramsData = new[]
                    {
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/window/01.jpg",
                            link = ""
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/window/02.jpg",
                            link = ""
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/window/03.jpg",
                            link = ""
                        },
                        new
                        {
                            imgUrl = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/window/04.jpg",
                            link = ""
                        },
                    }
                },
              
                goods = new
                {
                    name = "商品组",
                    type = "goods",
                    paramsData = new
                    {
                        source = "auto", // 数据来源 (choice手动选择 auto自动获取)
                        auto = new
                        {
                            category = 0, // 商品分类 0为全部
                            goodsSort = "all", // 商品排序 (all默认 sales销量 price价格)
                            showNum = 6 // 显示数量
                        }
                    },
                    style = new
                    {
                        titleText= "商品模块标题",
                        titleShow = true,
                        titleMoreText = "更多",
                        titleMoreLink = "",
                        titlebackground = "F6F6F6",
                        background = "#F6F6F6",
                        display = "list", // 显示类型 (list列表平铺 slide横向滑动)
                        column = 2, // 分列数量
                        show = new[]
                        { // 显示内容
                            "goodsName", "salePrice", "linePrice", "subTitle", "goodsSales"
                        }
                    },
                    defaultData = new[] //"自动获取" = 默认数据
                    {
                        new
                        {
                            GoodsName = "此处显示商品名称",
                            UrlImageMain = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/goods/01.png",
                            SalePrice = "99.00",
                            LinePrice = "139.00",
                            SubTitle = "此款商品美观大方 不容错过",
                            GoodsSales = 100
                        },
                        new
                        {
                            GoodsName = "此处显示商品名称",
                            UrlImageMain = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/goods/01.png",
                            SalePrice = "99.00",
                            LinePrice = "139.00",
                            SubTitle = "此款商品美观大方 不容错过",
                            GoodsSales = 100
                        },
                        new
                        {
                            GoodsName = "此处显示商品名称",
                            UrlImageMain = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/goods/01.png",
                            SalePrice = "99.00",
                            LinePrice = "139.00",
                            SubTitle = "此款商品美观大方 不容错过",
                            GoodsSales = 100
                        },
                        new
                        {
                            GoodsName = "此处显示商品名称",
                            UrlImageMain = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/goods/01.png",
                            SalePrice = "99.00",
                            LinePrice = "139.00",
                            SubTitle = "此款商品美观大方 不容错过",
                            GoodsSales = 100
                        }
                    },   
                },

                service = new
                {
                    name = "在线客服",
                    type = "service",
                    paramsData = new
                    {
                        type = "chat", // "客服类型" = chat在线聊天，phone拨打电话
                        image = $"{Define.HttpBaseApi()}/wwwroot/ImgDefault/diy/service.png",
                        tel = "" // 电话号吗
                    },
                    style =
                        new
                        {
                            right = 1,
                            bottom = 10,
                            opacity = 100
                        },
                }
            };
            return items;
        }

    }

    /// <summary>
    /// 页面
    /// </summary>
    public class ParamsObj
    {
        /// <summary>
        /// 页面名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        ///标题栏标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 分享标题
        /// </summary>
        public string shareTitle { get; set; }
    }

    /// <summary>
    /// 标题栏样式
    /// </summary>
    public class TitleStyle
    {
        /// <summary>
        /// 标题栏背景色
        /// </summary>
        public string titleBackgroundColor { get; set; }
        /// <summary>
        ///标题栏字体颜色
        /// </summary>
        public string titleTextColor { get; set; }

    }
}
