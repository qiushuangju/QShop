using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    public class ResListTotalCart
    {
        /// <summary>
        /// 总计数
        /// </summary>
        public int SumNum { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<ResCart> List { get; set; }
    }
}
