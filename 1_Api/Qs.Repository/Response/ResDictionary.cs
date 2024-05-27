using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Qs.Repository.Domain;

namespace Qs.App.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class ResDictionary
    {
        public string Id { get; set; }
        [Description("")]
        public string Title { get; set; }

        /// <summary>
        /// 子项
        /// </summary>
        public List<SubDic> ListSon { get; set; }
    }
    /// <summary>
    /// 子项
    /// </summary>
    public class SubDic
    {
        public string Id { get; set; }
        [Description("")]
        public string Title { get; set; }
    }
}
