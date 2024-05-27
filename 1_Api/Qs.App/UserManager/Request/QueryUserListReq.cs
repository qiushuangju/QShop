using Qs.App.Base;
using System;
using System.Collections.Generic;
using Qs.Repository.Base;

namespace Qs.App.Request
{
    public class QueryUserListReq : BaseReqPage
    {
        /// <summary>
        /// 组织Id
        /// </summary>
        public string orgId { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 业务员Id
        /// </summary>
        public string BusinessUserId { get; set; }

        /// <summary>
        /// 是否统计用户订单
        /// </summary>
        public bool IsTotalOrder { get; set; } = false;

        /// <summary>
        /// 最小用户类型
        /// </summary>
        public int? MinUserType { get; set; }

        /// <summary>
        /// 最大用户类型
        /// </summary>
        public int? MaxUserType { get; set; }

        /// <summary>
        ///父级 （最后一个上级）
        /// </summary>
        public string LastSourceUserId { get; set; }
        /// <summary>
        /// 只查询当前用户下级
        /// </summary>    
        public bool MySon { get; set; } = false;
    }
}
