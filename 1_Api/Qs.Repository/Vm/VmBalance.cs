using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Request
{
    /// <summary>
    /// 余额操作
    /// </summary>
    public class VmBalance
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public xEnum.BalanceType OpType { get;set; }
      
        /// <summary>
        /// 变动金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 运费（订单）
        /// </summary>
        public decimal? FreightPrice { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public string OrderId { get; set; } = "";
        /// <summary>
        /// 操作备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 余额变动用户Id(如变动用户即当前用户可不传)
        /// </summary>
        public string UserId { get; set; } = "";
    }

    public class ConsumptionMoneyVm
    {
        /// <summary>
        /// 变动场景
        /// </summary>

        public xEnum.ConsumptionMoneyType ConsumptionType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ModelUser User { get; set; }

        /// <summary>
        /// 变动金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 运费（订单）
        /// </summary>
        public decimal? FreightPrice { get; set; }
    }

    /// <summary>
    /// 佣金
    /// </summary>
    public class CalUserIncomeVm
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 下单用户
        /// </summary>
        public string OrderUserId { get; set; }

        /// <summary>
        /// 下单用户上级
        /// </summary>
        public string SourceUserId { get; set; }

        /// <summary>
        /// 变动场景
        /// </summary>
        public xEnum.ConsumptionMoneyType ConsumptionType { get; set; }
        /// <summary>
        /// 佣金类型
        /// </summary>
        public xEnum.IncomeType IncomeType { get; set; }
       

        /// <summary>
        /// 订单总提成
        /// </summary>
        public decimal OrderCommissionMoney { get; set; }
    }
}
