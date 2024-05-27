using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppStoreSetting : AppBaseString<ModelStoreSetting, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStoreSetting(IUnitWork<QsDBContext> unitWork, IRepository<ModelStoreSetting, QsDBContext> repository,
            AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension,
            auth)
        {
            _revelanceApp = app;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuStoreSetting req)
        {
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelStoreSetting> ListByWhere(ReqQuStoreSetting req, bool isPage = false)
        {
            IQueryable<ModelStoreSetting> linq = ListLinq(req);
            List<ModelStoreSetting> list =
                isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelStoreSetting> ListLinq(ReqQuStoreSetting req)
        {
            var linq = UnitWork.Find<ModelStoreSetting>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Key.Contains(req.Key));
            }

            if (req.ListKey!=null&&req.ListKey.Count>0)
            {
                linq = linq.Where(p => req.ListKey.Contains(p.Key));
            }

            string storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId== storeId);
            }
            return linq;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public Dictionary<string, dynamic> ListCommSettingData()
        {

            List<string> listKey = new List<string>()
            {
                "register",
                "pageCategoryTemplate",
                "points",
                "recharge",
                "store",
                "_other"
            };
            Dictionary<string, dynamic> listVm = new Dictionary<string, dynamic>();
            foreach (var item in listKey)
            {
                listVm.Add( item, GetDetail(item));
            }
            return listVm;
        }
    

      /// <summary>
      /// 获取设置
      /// </summary>
      /// <param name="key"></param>
      /// <param name="storeId">为空则自动获取</param>
      /// <returns></returns>
        public dynamic GetDetail(string key,string storeId="")
        {
            if (string.IsNullOrEmpty(storeId))
            {
                storeId = _auth.GetStoreId();
            }
            ModelStoreSetting modelDb = Repository.FirstOrDefault(p => p.Key == key && p.StoreId == storeId);
            if (key ==DefineSetting.Points) //积分设置
            {
                VmSettingPoints model = new VmSettingPoints();
                if (modelDb != null)
                {
                    model = xConv.JsonToObj<VmSettingPoints>(modelDb.Values);
                }
                return model;
            }

            if (key == DefineSetting.FullFree) //满额包邮设置
            {
                VmSettingFullFree model = new VmSettingFullFree();
                if (modelDb != null)
                {
                    model = xConv.JsonToObj<VmSettingFullFree>(modelDb.Values);
                }
                return model; ;
            }
            if (key == DefineSetting.Register) //账户注册设置
            {
                VmSettingRegister model = new VmSettingRegister();
                if (modelDb != null)
                {
                    model = xConv.JsonToObj<VmSettingRegister>(modelDb.Values);
                } 
                return model;
            }
            if (key == DefineSetting.PageCategoryTemplate)   //分类页模板
            {
                VmSettingCategoryTemplate model = modelDb == null ? new VmSettingCategoryTemplate() : xConv.JsonToObj<VmSettingCategoryTemplate>(modelDb.Values);
                return model;
            }
            if (key == DefineSetting.BasicWxApp) //小程序设置
            {
                VmSettingBasicWxApp model = modelDb == null ? new VmSettingBasicWxApp() : xConv.JsonToObj<VmSettingBasicWxApp>(modelDb.Values);
                model.StoreId = storeId;
                return model;
            }
            if (key == DefineSetting.BasicH5) //H5站点设置
            {
                VmSettingBasicH5 model = modelDb == null ? new VmSettingBasicH5() : xConv.JsonToObj<VmSettingBasicH5>(modelDb.Values);
                return model;
            }
            if (key == DefineSetting.Trade) //交易设置
            {
                VmSettingTrade model = new VmSettingTrade();
                if (modelDb != null)
                {
                    model = xConv.JsonToObj<VmSettingTrade>(modelDb.Values);
                }

                return model;
            }

            if (key == DefineSetting.Storage) //上传设置
            {
                VmSettingStorage model = modelDb == null
                    ? new VmSettingStorage()
                    : xConv.JsonToObj<VmSettingStorage>(modelDb.Values);
                return model;
            }

            if (key == DefineSetting.Delivery) //配送方式设置
            {
                VmSettingDelivery model = modelDb == null
                    ? new VmSettingDelivery()
                    : xConv.JsonToObj<VmSettingDelivery>(modelDb.Values);
                return model;
            }

            if (key==DefineSetting.Recharge)//充值设置
            {
                VmSettingRecharge model =modelDb == null
                    ? new VmSettingRecharge()
                    : xConv.JsonToObj<VmSettingRecharge>(modelDb.Values);
                return model;
            }

            if (key == DefineSetting.Sms) //短信通知设置
            {
                VmSettingSms model =
                    modelDb == null ? new VmSettingSms() : xConv.JsonToObj<VmSettingSms>(modelDb.Values);
                EngineInfo engine = model.Engine ?? new EngineInfo();
                AliSdkInfo aliInfo = engine.Aliyun ?? new AliSdkInfo();
                aliInfo.Name = $"阿里云短信";
                aliInfo.Website = $"https://dysms.console.aliyun.com/dysms.htm";
                aliInfo.Sign = string.IsNullOrEmpty(aliInfo.Sign) ? $"QShop" : aliInfo.Sign;
                engine.Aliyun = aliInfo;

                TengXunSdkInfo qcloudInfo = engine.Qcloud ?? new TengXunSdkInfo();
                qcloudInfo.Name = $"腾讯云短信";
                qcloudInfo.Website = $"https://console.cloud.tencent.com/smsv2";
                qcloudInfo.Sign = string.IsNullOrEmpty(qcloudInfo.Sign) ? $"QShop" : qcloudInfo.Sign;

                engine.Qcloud = qcloudInfo;


                QiNiu qiniuInfo = engine.Qiniu ?? new QiNiu();
                qiniuInfo.Name = $"七牛云短信";
                qiniuInfo.Website = $"https://portal.qiniu.com/sms/dashboard";
                qiniuInfo.Sign = string.IsNullOrEmpty(qiniuInfo.Sign) ? $"QShop" : qiniuInfo.Sign;
                engine.Qiniu = qiniuInfo;

                SceneInfo scene = model.Scene ?? new SceneInfo();
                TemplateSms captcha = scene.captcha ?? new TemplateSms();
                captcha.Name = $"短信验证码(通知用户)";
                captcha.IsEnable = scene.captcha == null ? false : captcha.IsEnable;
                captcha.Content = $"%s为您的验证码,请于5分钟内填写,如非本人操作,请忽略本短信.";

                captcha.Variables = new TemplateVariables()
                {
                    Aliyun = new List<string>() {"${code}"},
                    Qcloud = new List<string>() {"${code}"},
                    Qiniu = new List<string>() {"${1}"}
                };
                scene.captcha = captcha;

                TemplateSmsPayOrder orderPaySmsTemp = scene.order_pay ?? new TemplateSmsPayOrder();
                orderPaySmsTemp.Name = $"新付款订单(通知商家)";
                orderPaySmsTemp.IsEnable = scene.order_pay == null ? false : orderPaySmsTemp.IsEnable;
                orderPaySmsTemp.Content = $"您有一个新订单，订单号为：%s，请注意查看！";
                orderPaySmsTemp.Variables = new TemplateVariables()
                {
                    Aliyun = new List<string>() {"${order_no}"},
                    Qcloud = new List<string>() {"${order_no}"},
                    Qiniu = new List<string>() {"${1}"}
                };
                scene.order_pay = orderPaySmsTemp;

                model.Scene = scene;
                model.Engine = engine;
                return model;
            }

            return null;
        }


        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuStoreSetting req)
        {
            var storeId = _auth.GetCurrentContext().User.StoreId;
            var modelDb = Repository.FirstOrDefault(p => p.Key == req.Key && p.StoreId == storeId) ??
                          new ModelStoreSetting();
            string strJsonValue = xConv.ToJson(req.Data);
            // modelDb.Describe = GetDescription(typeof(DefineSetting), req.Key);

            if (req.Key == "points") //积分设置
            {
                modelDb.Describe = $"积分设置";
            }
            if (req.Key == "fullFree") //满额包邮设置
            {
                modelDb.Describe = $"满额包邮设置";
            }
            if (req.Key == "register")
            {
                modelDb.Describe = $"账户注册设置";
            }
            if (req.Key == "pageCategoryTemplate")   //分类页模板
            {
                modelDb.Describe = $"分类页模板";
            }
            if (req.Key == "trade")
            {
                modelDb.Describe = $"交易设置";
            }
            if (req.Key == "sms")
            {
                modelDb.Describe = $"短信通知";
            }
            if (req.Key == "delivery")
            {
                modelDb.Describe = $"配送方式设置";
            }
            if (req.Key == "storage")
            {
                modelDb.Describe = $"上传设置";
            }
            if (req.Key == "basicWxApp") //小程序设置
            {
                modelDb.Describe = $"小程序设置";
            }
            if (req.Key == "basicH5") //H5站点设置
            {
                modelDb.Describe = $"H5站点设置";
            }
            if (req.Key == "recharge") //充值设置
            {
                modelDb.Describe = $"充值设置";
            }
            

            modelDb.Key = req.Key;
            modelDb.Values = strJsonValue;
            modelDb.StoreId = storeId;

            UnitWork.AddOrUpdate(modelDb);
            UnitWork.Save();
        }


      

    }
}