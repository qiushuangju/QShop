using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Domain;
using Qs.Repository.Response;

namespace Qs.Repository.Vm
{
   public class OrderSkuCreate
    {
        /// <summary>
        /// Sku
        /// </summary>
        public ResGoodSku Sku { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
