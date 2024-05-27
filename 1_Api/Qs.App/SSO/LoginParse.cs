/*
 * 登录解析
 * 处理登录逻辑，验证客户段提交的账号密码，保存登录信息
 */
using System;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

using TencentCloud.Iot.V20180123.Models;


namespace Qs.App.SSO
{
    public class LoginParse
    {

        //这个地方使用IRepository<User> 而不使用UserManagerApp是防止循环依赖
        public IRepository<ModelUser, QsDBContext> _app;            
        private ICacheContext _cacheContext;
        private IUnitWork<QsDBContext> _unitWork;
        public LoginParse(  ICacheContext cacheContext, IRepository<ModelUser, QsDBContext> userApp,
             IUnitWork<QsDBContext> unitWork)
        {
            _cacheContext = cacheContext;
            _unitWork = unitWork;
            _app = userApp;
        }

        public  LoginResult Do(PassportLoginRequest model)
        {
            var result = new LoginResult();
            try
            {
                model.Trim();
                //获取用户信息
                ModelUser userInfo = null;
                userInfo = _app.FirstOrDefault(p => (p.Phone == model.Account || p.Account == model.Account)&& p.UserType<(int)xEnum.UserType.Customer);
                if (userInfo == null)
                {
                    throw new Exception("用户不存在");
                }

                if (!string.IsNullOrEmpty(model.Password))
                {
                    var pwd = xConv.MD5Encoding(model.Password, xConv.ToStrDateTime(userInfo.CreateTime));
                    if (userInfo.Password != pwd)
                    {
                        throw new Exception("密码错误");
                    }
                }
               
                if (userInfo.Status == (int)xEnum.ComStatus.Disable)
                {
                    throw new Exception("账号状态异常，可能已停用");
                }

                userInfo=UpdateToken(userInfo);

                result.Code = 200;
                result.UserId = userInfo.Id;
                result.NickName = userInfo.NickName;
                result.UserType = xConv.ToInt(userInfo.UserType);
                result.UrlAvatar = userInfo.UrlAvater;
                result.Token = userInfo.Token;
            }
            catch (Exception ex)
            {
                result.Code = 500;
                result.Message = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public LoginResult DoWxOrPhone(ReqLoginWx.ReqLoginWxDo model)
        {
            var result = new LoginResult();
            //获取用户信息
            ModelUser userInfo = null;
            userInfo = _unitWork.FirstOrDefault<ModelUser>(u => u.WxOpenId == model.WxOpenIdOrPhone||u.Phone==model.WxOpenIdOrPhone);
            if (userInfo == null)
            {
                throw new Exception("用户不存在");
            }
            userInfo = UpdateToken(userInfo);
            result.Code = 200;
            result.Token = userInfo.Token;
            return result;
        }

        public ModelUser UpdateToken(ModelUser userInfo)
        {
            // 同一用户多端登录挤下线
            // var token = Guid.NewGuid().ToString().GetHashCode().ToString("x");
            // userInfo.Token = token;
            // userInfo.TokenTime = DateTime.Now;
            // _app.Update(userInfo); 

            // 同一用户多端登录不挤下线
            var token = userInfo.Token;
            if (userInfo.TokenTime < DateTime.Now.AddDays(-Define.TokenDays)||string.IsNullOrEmpty(token))
            {
                token = Guid.NewGuid().ToString().GetHashCode().ToString("x");
                userInfo.Token = token;
                userInfo.TokenTime = DateTime.Now;
                _unitWork.AddOrUpdate(userInfo);
                _unitWork.Save();
            }

            return userInfo;
        }
    }
}