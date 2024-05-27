using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 配送方式设置
    /// </summary>
    public class VmSettingDelivery
    {
        /// <summary>
        /// 默认短信渠道
        /// </summary>
        public string Default { get; set; } = "kuaiDiNiao";

        /// <summary>
        /// 短信渠道/平台
        /// </summary>
        public EngineDeliveryInfo Engine { get; set; } 

     
    }

    public class EngineDeliveryInfo
    {
        /// <summary>
        /// 快递100
        /// </summary>
        public Kuaidi100Info KuaiDi100 { get; set; }

        /// <summary>
        /// 快递鸟
        /// </summary>
        public KuaiDiNiaoInfo KuaiDiNiao { get; set; }
    }


    /// <summary>
    /// 快递100
    /// </summary>
    public class Kuaidi100Info
    {
        /// <summary>
        /// 快递100 Customer
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// 快递100 Key
        /// </summary>
        public string Key { get; set; }

    }

    /// <summary>
    /// 快递鸟
    /// </summary>
    public class KuaiDiNiaoInfo  
    {
        /// <summary>
        /// 快递鸟店铺Id
        /// </summary>
        public string EBusinessID { get; set; }
        /// <summary>
        ///快递鸟 API key
        /// </summary>
        public string ApiKey { get; set; }
    }
}

