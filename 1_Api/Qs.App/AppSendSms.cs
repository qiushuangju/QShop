using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Cache;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Vm;

namespace Qs.App
{
    /// <summary>
    /// 手机短信
    /// </summary>
    public class AppSendSms : AppBaseString<ModelUser, QsDBContext>
    {
        private AppStoreSetting _appSetting;
        private VmSettingSms smsSetting;
        private ISmsHelper sms;
        private ICacheContext _cacheContext;

        public AppSendSms(IUnitWork<QsDBContext> unitWork, IRepository<ModelUser, QsDBContext> repository,
            AppStoreSetting appSetting, ICacheContext cacheContext, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _appSetting = appSetting;
            _cacheContext = cacheContext;
             smsSetting = _appSetting.GetDetail("sms");
            switch (smsSetting.Default)
            {
                case "aliyun"://阿里云
                  
                    break;
                case "qcloud"://腾讯云
                    sms = new SmsTx(smsSetting);
                    break;
                case "qiniu":  //七牛云

                    break;
                default:
                   
                    break;
            }
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        public ResPhoneCode SendCode(string phone)
        {
            
            string code = xConv.GenerateRandomCode(4); 
            ResPhoneCode res = sms.SendPhoneCode(phone, code);
            string verifyCodeId = xConv.NewGuid();
            _cacheContext.Set(verifyCodeId, code, DateTime.Now.AddMinutes(5));
            res.VerifyCodeId = verifyCodeId;
            return res; 
        }

        /// <summary>
        /// 发送新订单(到商户)
        /// </summary>
        /// <param name="phone"></param>
        public void SendNewOrderToStore(string phone)
        {
            ResPhoneCode res = sms.SendNewOrderToStore(phone);
        }

    }
}
