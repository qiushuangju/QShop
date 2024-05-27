using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Comm.Model
{
    /// <summary>
    /// 日志
    /// </summary>
    public class ModelLog
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ModelLog()
        {
            this.Title = string.Empty;
            this.TypeName = string.Empty;
            this.Href = string.Empty;
            this.CreateName = string.Empty;
            this.ApiInContent = string.Empty;
            this.ApiOutContent = string.Empty;
        }


        /// <summary>
        /// 日志内容
        /// </summary>
        [Description("日志内容")]
        public string Title { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        [Description("分类名称")]
        public string TypeName { get; set; }
        /// <summary>
        /// 操作所属模块地址
        /// </summary>
        [Description("操作所属模块地址")]
        public string Href { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        [Description("操作人")]
        public string CreateName { get; set; }
        /// <summary>
        /// 输入参数
        /// </summary>
        [Description("输入参数")]
        public string ApiInContent { get; set; }
        /// <summary>
        /// 输出参数
        /// </summary>
        [Description("输出参数")]
        public string ApiOutContent { get; set; }
       
    }
}
