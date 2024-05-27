using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Repository.Base;

namespace Qs.Repository.Request
{
    public class ReqOrderCar : BaseReqPage
    {
        /// <summary>
        /// carId
        /// </summary>
        public string UserCarId { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
