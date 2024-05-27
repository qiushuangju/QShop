using Qs.App.Base;
using Qs.Repository.Base;

namespace Qs.App.Request
{
    public class QueryUserListByRoleReq : BaseReqPage
    {
        public string roleId { get; set; }
    }
}
