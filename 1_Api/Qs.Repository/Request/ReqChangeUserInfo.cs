namespace Qs.Repository.Request
{
    /// <summary>
    /// 修改头像昵称
    /// </summary>
    public class ReqChangeUserInfo
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string UrlAvater { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
    }
}
