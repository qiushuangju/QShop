using Qs.Repository.Domain;

namespace Qs.App.Response
{
    public class CategoryTypeResp : CategoryType
    {
        public string ParentId { get; set; }
        
    }
}