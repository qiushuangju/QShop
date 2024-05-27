using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResInviteUser:ModelInviteLinkRecord
    {
        /// <summary>
        /// 被邀请人头像
        /// </summary>
        public string InviteeAvater { get; set; }

        /// <summary>
        /// 状态名
        /// </summary>
        public string StrStatus { get; set; }
        /// <summary>
        /// 开通类型
        /// </summary>
        public string StrOpenType { get; set; }

        /// <summary>
        /// 转View对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ResInviteUser ToView(ResInviteUser model)
        {
            ResInviteUser res = xConv.CopyMapper<ResInviteUser, ModelInviteLinkRecord>(model);
            res.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.InviteRecordStatus), model.Status);
            res.StrOpenType = xConv.ToInt(model.OpenType)==0?"未开通会员": xEnum.GetEnumDescription(typeof(xEnum.UserType), model.OpenType);
            return res;
        }
    }
}
