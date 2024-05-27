using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 修改用户状态
    /// </summary>
    public class ReqChangeUserStatus
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int Status { get; set; }
    }

    /// <summary>
    /// 审核用户
    /// </summary>

    public class ReqCheckUser
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 商城
        /// </summary>
        public string StoreId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string StoreStorageId { get; set; }
    }
}
