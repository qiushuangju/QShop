using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 推广明细
    /// </summary>
    public class ResListInviteDetail
    {
        /// <summary>
        /// 被邀请人
        /// </summary>
        public string InviteeName { get; set; }

        /// <summary>
        /// 被邀请人手机号码
        /// </summary>
        public string InviteePhone { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态名
        /// </summary>
        public string StrStatus { get; set; }

        public static ResListInviteDetail ToView(ModelInviteLinkRecord model)
        {
            ResListInviteDetail res = xConv.CopyMapper<ResListInviteDetail, ModelInviteLinkRecord>(model);
            res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.InviteRecordStatus), model.Status);
            return res;
        }
    }
}
