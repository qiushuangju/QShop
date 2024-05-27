using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResStorePage:ModelStorePage
    {
        public string StrPageType { get; set; }
        public  dynamic PageDataObj { get; set; }

        public static ResStorePage ToView(ModelStorePage model)
        {
            ResStorePage res = xConv.CopyMapper<ResStorePage, ModelStorePage>(model);
            res.PageDataObj = xConv.JsonToObj<dynamic>(model.PageData);
            res.StrPageType = xEnum.GetEnumDescription(typeof(xEnum.PageType),model.PageType);
            return res;
        }
    }
}
