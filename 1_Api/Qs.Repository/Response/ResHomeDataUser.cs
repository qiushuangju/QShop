using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 
    /// </summary>
    public  class ResHomeDataUser
    {
        /// <summary>
        /// 待付款
        /// </summary>
        public int CountNotPaid { get; set; }
        /// <summary>
        /// 待发货
        /// </summary>
        public int CountWaitDeliver { get; set; }
        /// <summary>
        /// 待收货
        /// </summary>
        public int CountWaitReceiving { get; set; }

        /// <summary>
        /// 已收货
        /// </summary>
        public int CountReceived { get; set; }
        /// <summary>
        /// 售后
        /// </summary>

        public int CountAfterSale { get; set; }
        /// <summary>
        /// 售后
        /// </summary>

        public int CountAfterSaleService { get; set; }

        /// <summary>
        /// 官方热线
        /// </summary>
        public string OfficialPhone { get; set; }

    }
}
