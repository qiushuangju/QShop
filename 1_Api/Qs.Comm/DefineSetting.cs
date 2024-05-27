using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Comm
{
    /// <summary>
    /// 设置
    /// </summary>
    public static class DefineSetting
    {
        /// <summary>
        /// 积分设置
        /// </summary>
        [Description("积分设置")]
        public const string Points = "points";
        /// <summary>
        /// 满额包邮设置
        /// </summary>
        [Description("满额包邮设置")]
        public const string FullFree = "fullFree";
        /// <summary>
        /// 账户注册设置
        /// </summary>
       [Description("账户注册设置")]
        public const string Register = "register";
        /// <summary>
        /// 分类页模板设置
        /// </summary>
        [Description("分类页模板")]
        public const string PageCategoryTemplate = "pageCategoryTemplate";
        /// <summary>
        /// 小程序设置
        /// </summary>
        [Description("小程序设置")]
        public const string BasicWxApp = "basicWxApp";
        /// <summary>
        /// H5站点设置
        /// </summary>
        [Description("H5站点设置")]
        public const string BasicH5 = "basicH5";
        /// <summary>
        /// 交易设置
        /// </summary>
        [Description("交易设置")]
        public const string Trade = "trade";
        /// <summary>
        /// 上传设置
        /// </summary>
        [Description("上传设置")] 
        public const string Storage = "storage";
        /// <summary>
        /// 配送方式设置
        /// </summary>
        [Description("配送方式设置")]
        public const string Delivery = "delivery";
        /// <summary>
        /// 短信通知设置
        /// </summary>
        [Description("短信通知设置")]
        public const string Sms = "sms";
        /// <summary>
        /// 充值设置
        /// </summary>
        [Description("充值设置")]
        public const string Recharge = "recharge";
    }
}
