using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Qs.Repository.Request;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 满额包邮设置
    /// </summary>
    public class VmSettingFullFree
    {
        /// <summary>
        /// 是否开启
        /// </summary>
        public int IsOpen { get; set; } = -10;
        /// <summary>
        /// 单笔订单满额
        /// </summary>
        public decimal? Money { get; set; } 
        /// <summary>
        /// 不参与包邮商品
        /// </summary>
        public List<string> ExcludedGoodsIds { get; set; } =new List<string>();

        /// <summary>
        ///满额包邮说明
        /// </summary>
        public string Describe { get; set; }

        /// <summary>
        ///不参与包邮地区
        /// </summary>
        public FullFreeRegion ExcludedRegions { get; set; } =new FullFreeRegion();
    }

    /// <summary>
    /// 不参与包邮地区
    /// </summary>
    public class FullFreeRegion
    {
        /// <summary>
        /// 地区Ids
        /// </summary>
        public List<string> CityIds { get; set; } =new List<string>();
        /// <summary>
        /// 选中省
        /// </summary>
        public List<FullFreeProvince> SelectedText { get; set; } =new List<FullFreeProvince>();
    }

    /// <summary>
    /// 选中省
    /// </summary>
    public class FullFreeProvince
    {
        /// <summary>
        /// 省
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public List<CityDelivery> Chilren { get; set; }
    }


}
