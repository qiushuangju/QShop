using System.ComponentModel;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    public class ResDic
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Description("代码")]
        public string DtCode { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Description("值")]
        public string DtValue { get; set; }

        /// <summary>
        ///  ToDic
        /// </summary>
        /// <param name="cate"></param>
        /// <returns></returns>
        public static ResDic ToDic(Category cate)
        {
            ResDic res = xConv.CopyMapper<ResDic, Category>(cate);
            return res;
        }
    }
}
