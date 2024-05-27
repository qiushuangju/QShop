using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Request;

namespace Qs.Repository.Response
{

    public class ResUserAddress : ModelUserAddress
    {
        /// <summary>
        /// 省市区地址
        /// </summary>
        public string StrProvince { get; set; }

        public static ResUserAddress ToView(ModelUserAddress model)
        {
            if (model==null)
            {
                return null;
            }
            ResUserAddress res = xConv.CopyMapper<ResUserAddress, ModelUserAddress>(model);
            res.StrProvince = $"{model.Province}{model.City}{model.Region}";
            return res;
        }
    }

   
}
