using Qs.App.Base;
using Qs.Repository.Base;

namespace Qs.App.Request
{
    public class QueryCategoryListReq : BaseReqPage
    {

        /// <summary>
        /// TypeID
        /// </summary>
        public string TypeId { get; set; }
    }
}