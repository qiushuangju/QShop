using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// H5站点设置
    /// </summary>
    public class VmSettingBasicH5
    {
        /// <summary>
        /// 是否开启访问
        /// </summary>
        public int Enabled { get; set; } = (int) xEnum.ComStatus.Disable;

        /// <summary>
        ///H5站点地址
        /// </summary>
        public string BaseUrl { get; set; }
    }


}

