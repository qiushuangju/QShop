namespace Qs.App.UserManager.Request
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ReqChangePwd
    {
        public string AppKey { get; set; }
        public string Account { get; set; }
        public string Phone { get; set; }
        public int? UserType { get; set; }
        public string Password { get; set; }
    }
}
