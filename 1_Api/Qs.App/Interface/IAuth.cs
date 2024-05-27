/*
 *单独提取这个接口，为了以下几点：
 * 1、可以方便的实现webapi 和本地登录相互切换
 * 2、可以方便的使用mock进行单元测试
 */

using Qs.App.AuthStrategies;
using Qs.App.SSO;

namespace Qs.App.Interface
{
    public interface IAuth
    {
        /// <summary>
        /// 检验token是否有效
        /// </summary>
        /// <param name="token">token值</param>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        bool CheckLogin(string token="", string otherInfo = "");
        //获取当前上下文
        AuthStrategyContext GetCurrentContext();
        /// <summary>
        /// 获取小程序店铺Id
        /// </summary>
        /// <returns></returns>
        string GetStoreId();
        /// <summary>
        /// 获取用户平台来源
        /// </summary>
        /// <returns></returns>
        string GetPlatform();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="otherInfo"></param>
        /// <returns></returns>
        string GetUserName(string otherInfo = "");
        /// <summary>
        /// 微信登录接口
        /// </summary>
        /// <param name="appKey">登录的应用appkey</param>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        LoginResult LoginWx(string appKey, string openId);
        /// <summary>
        /// 登录接口-手机验证
        /// </summary>
        /// <param name="appKey">登录的应用appkey</param>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        LoginResult LoginByPhone(string appKey, string phone);
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="appKey">登录的应用appkey</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="phoneCode">验证码</param>
        /// <returns></returns>
        LoginResult Login(string appKey, string username, string pwd,string phoneCode="");
        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        bool Logout();
    }
}
