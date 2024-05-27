using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    /// <summary>
    /// 经营统计
    /// </summary>
   public class VmStatisticOperate
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public int? UserType { get; set; }
        /// <summary>
        /// 父节点Id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 归属路径
        /// </summary>
        public string SourceUserId { get; set; }
        /// <summary>
        ///用户创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 下级人数
        /// </summary>
        public int SonCount { get; set; }
        /// <summary>
        /// 团队人数
        /// </summary>
        public int TeamCount { get; set; }
        /// <summary>
        /// 推荐经销商
        /// </summary>
        public int RecommendDealers { get; set; }
        /// <summary>
        /// 推荐经销商人数
        /// </summary>
        public int RecommendCustomer { get; set; }

        // /// <summary>
        // /// 下级购物金额
        // /// </summary>
        // public decimal? SonGoodsPrice { get; set; }
        // /// <summary>
        // /// 下级运费金额
        // /// </summary>
        // public decimal? SonTotalFreightPrice { get; set; }
        // /// <summary>
        // /// 下级订单金额
        // /// </summary>
        // public decimal? SonOrderPrice { get; set; }
        // /// <summary>
        // /// 下级购物金额(完成)
        // /// </summary>
        // public decimal? SonDoneGoodsPrice { get; set; }
        // /// <summary>
        // /// 下级运费金额 (完成)
        // /// </summary>
        // public decimal? SonDoneTotalFreightPrice { get; set; }
        // /// <summary>
        // /// 下级订单金额  (完成)
        // /// </summary>
        // public decimal? SonDoneOrderPrice { get; set; }

        /// <summary>
        /// 团队购物金额
        /// </summary>
        public decimal? TeamGoodsPrice { get; set; }
        /// <summary>
        /// 团队运费金额
        /// </summary>
        public decimal? TeamTotalFreightPrice { get; set; }
        /// <summary>
        /// 团队订单金额
        /// </summary>
        public decimal? TeamOrderPrice { get; set; }
        /// <summary>
        /// 团队购物金额 (完成)
        /// </summary>
        public decimal? TeamDoneGoodsPrice { get; set; }
        /// <summary>
        /// 团队运费金额  (完成)
        /// </summary>
        public decimal? TeamDoneTotalFreightPrice { get; set; }
        /// <summary>
        /// 团队订单金额  (完成)
        /// </summary>
        public decimal? TeamDoneOrderPrice { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

    }
}
