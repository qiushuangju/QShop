using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.App.Wx
{
   public class AppTestPayAmount
    {
        /// <summary>
        /// 测试用户支付0.01元
        /// </summary>
        /// <param name="user"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static decimal GetPayAmount(ModelUser user, decimal? amount)
        {
            decimal payAmout = xConv.ToDecimal(amount);
            if (user.Phone == "17773121044" || user.Phone == "18684768205" || user.Phone == "19976958653"
                || user.Phone == "18390960143" || user.Phone == "16680812636"
                || user.Phone == "13055169744" || user.Phone == "18613985498" || user.Phone == "15674958413"
                || user.Phone == "19898811679" || user.Phone == "18570049981" || user.Phone == "18711136198"
                || user.Phone == "15616193717")
            {
                payAmout = 0.01M;
            }
            return payAmout;
        }
    }
}
