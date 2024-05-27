namespace Qs.Repository.Base
{
    /// <summary>
    /// 分页父类
    /// </summary>
    public class BaseReqPage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseReqPage()
        {
            Page = 1;
            Limit = 10;
        }
        /// <summary>
        ///系统
        /// </summary>
        /// <example>1</example>
        public string AppKey { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        /// <example>1</example>
        public int Page { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        /// <example>10</example>
        public int Limit { get; set; }
        /// <summary>
        ///搜索Key
        /// </summary>                            
        public string Key { get; set; }

        // /// <summary>
        // /// 只查询当前商户
        // /// </summary>
        // public bool OnlyStore
        // {
        //     get
        //     {
        //         return AppKey== "MP-WEIXIN";
        //     }
        //     set
        //     {
        //         OnlyStore = value;
        //     }
        // }
        //
        // /// <summary>
        // /// 是否只查自己记录 (默认是)
        // /// </summary>
        // public bool OnlyMy { get; set; } = true;  
    }


}
