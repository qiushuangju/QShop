using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 分类页模板
    /// </summary>
    public class VmSettingCategoryTemplate
    {
        /// <summary>
        /// 分享标题
        /// </summary>
        public string ShareTitle { get; set; }

        /// <summary>
        ///  分类页样式
        /// </summary>
        public int Style { get; set; } = 30;

        /// <summary>
        /// 显示加入购物车
        /// </summary>
        public bool ShowAddCart { get; set; } = true;

        /// <summary>
        ///加购物车按钮样式
        /// </summary>
        public int CartStyle { get; set; } = 1;

    }


}

