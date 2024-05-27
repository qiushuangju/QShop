using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Vm
{
    public class VmGetFreighMoney
    {
        /// <summary>
        /// 运费
        /// </summary>
        public decimal FreighMoney { get; set; } = 0;
        /// <summary>
        /// 是否在配送范围
        /// </summary>
        public bool IsIntraRegion { get; set; } = true;
        /// <summary>
        /// 不配送商品
        /// </summary>
        public string UndeliveredGoods { get; set; }
    }
}
