using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class VmStoreStorage
    {
        [Browsable(false)]
        public string Id { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        [Description("仓库名称")]
        public string Name { get; set; }
        /// <summary>
        /// 状态(-10:禁用 10:正常)
        /// </summary>
        [Description("状态(-10:禁用 10:正常)")]
        public int Status { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [Description("联系人")]
        public string LinkMan { get; set; }
        /// <summary>
        /// 联系人手机号
        /// </summary>
        [Description("联系人手机号")]
        public string Phone { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        [Description("详细地址")]
        public string Address { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [Description("城市")]
        public string City { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        [Description("经度")]
        public decimal Longitude { get; set; }
        /// <summary>
        /// 纬度
        /// </summary>
        [Description("纬度")]
        public decimal Latitude { get; set; }
        /// <summary>
        /// 店铺Id
        /// </summary>
        [Description("店铺Id")]
        [Browsable(false)]
        public string StoreId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public System.DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public System.DateTime UpdateTime { get; set; }

        /// <summary>
        /// 营业时间
        /// </summary>
        [Description("营业时间")]
        public string OpeningTime { get; set; }

        /// <summary>
        /// 距离(KM)
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// 格式化距离(Km)
        /// </summary>
        public string DistanceStr { get; set; }
    }
}
