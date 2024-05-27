using Qs.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.App.UserManager.Request
{
    /// <summary>
    /// 
    /// </summary>
    public  class ChangeChildBusinessUserReq
    {
        public ChangeChildBusinessUserReq(List<string> listChildUserId)
        {
            ListChildUserId = listChildUserId;
        }

        /// <summary>
        /// 原始业务员id
        /// </summary>
        public string OldBusinessUserId { get; set; }

        /// <summary>
        /// 新业务员id
        /// </summary>
        public string NewBusinessUserId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public List<string> ListChildUserId { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(OldBusinessUserId,"原始业务员不能为空"),
                new ValueTip(NewBusinessUserId,"新业务员不能为空")
            });
            // ListChildUser.CheckListNull("客户");
        }
    }
}
