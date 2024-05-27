using System;
using System.Collections.Generic;

namespace Qs.App.UserManager.Response
{

    public class ResTeamStatistics
    {
        /// <summary>
        /// 经销商人数
        /// </summary>
        public int TotalDealers { get; set; }
        /// <summary>
        /// 用户人数
        /// </summary>
        public int TotalCustomer { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalGoodsAmount { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<VmTeamStatistics> ListData { get; set; }
    }


    /// <summary>
    /// 我的团队统计
    /// </summary>
    public  class VmTeamStatistics
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string UserAvater { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string StrUserType { get; set; }
        
        /// <summary>
        /// 总人数
        /// </summary>
        public  int TotalTeam { get; set; }
        /// <summary>
        /// 经销商人数
        /// </summary>
        public  int TotalDealers { get; set; }
        /// <summary>
        /// 用户人数
        /// </summary>
        public int TotalCustomer { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int TotalOrder { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal TotalGoodsAmount { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
