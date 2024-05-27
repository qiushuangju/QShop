using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

namespace Qs.WebApi.Controllers.Sys
{     
    /// <summary>
    /// 公共接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommController :  ControllerBase
    {
       
        private readonly AppComm _app;
        private readonly AppCommDistance _appDistance; 
        private readonly AppCategory _appDic;
        private readonly AppRefundReason _appRefundReason;
        private readonly AppExpress _appExpress;
        private AppStoreSetting _appSetting;
        /// <summary>
        /// /构造函数
        /// </summary>
        public CommController(AppRefundReason appRefundReason, AppCategory appDic, AppStoreSetting appSetting, AppComm app, AppCommDistance appDistance, AppExpress appExpress)
        {

            _appRefundReason = appRefundReason;
            _appDic = appDic;
            _app = app;
            _appExpress = appExpress;
            _appDistance = appDistance;
            _appSetting=appSetting;
        }
    
        /// <summary>
        /// 获取微信Token
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public Response<ReqWxJsParam> GetWxJsApiParam(ReqWxAccessToken req)
        {
            Response<ReqWxJsParam> res = new Response<ReqWxJsParam>();


            VmSettingBasicWxApp setting = _appSetting.GetDetail(DefineSetting.BasicWxApp);
            ReqWxJsParam param= WxAccessToken.GetWxJsParam(req.Url, setting);
            res.Result = param;
            return res;
        }
        /// <summary>
        /// 获取用户首页数据
        /// </summary>
        [HttpGet]
        public Response<ResHomeDataUser> GetHomeDataUser()
        {
            Response<ResHomeDataUser> res = new Response<ResHomeDataUser>();
            res.Result = _app.GetHomeDataUser();
            return res;
        }
        /// <summary>
        /// pc端首页数据
        /// </summary>
        [HttpGet]
        public Response<ResPcHomeData> GetPcHomeData()
        {    
            DateTime startDt=xConv.ToDate(DateTime.Now.AddDays(-6));
            Response<ResPcHomeData> res = new Response<ResPcHomeData>();
            res.Result = _app.GetPcHomeData(startDt);
            return res;
        }

        /// <summary>
        /// 快递公司
        /// </summary>
        [HttpGet]
        public Response<List<ModelExpressCodeKuaiDi100>> ListExpressCompany([FromQuery] ReqQuExpress req)
        {            
            Response<List<ModelExpressCodeKuaiDi100>> res = new Response<List<ModelExpressCodeKuaiDi100>>();
            res.Result = _appExpress.ListByWhere(req);
            return res;
        }

        /// <summary>
        /// 根据枚举名称获取枚举列表
        /// </summary>
        [HttpGet]
        public Response<List<EnumEntity>> ListEnum(string name, bool isAddAll = false)
        {
            Response<List<EnumEntity>> res = new Response<List<EnumEntity>>();
            res.Result = xEnum.GetEnumListByName(name, isAddAll);
            return res;
        }

        /// <summary>
        /// 门店类型
        /// </summary>
        [HttpGet]
        public Response<List<EnumEntity>> DdlGoodsLabel()
        {
            Response<List<EnumEntity>> res = new Response<List<EnumEntity>>();
            res.Result = xEnum.ListEnumModel(typeof(xEnum.GoodsLabel),true);
            return res;
        }
        // /// <summary>
        // /// 售后类型
        // /// </summary>
        // /// <returns></returns>
        // [HttpGet]
        // [AllowAnonymous]
        // public Response<List<resultType>> ListRefundType(string subOrderNo11)
        // {
        //     var result = new Response<List<resultType>>();
        //     OrderApi api = new OrderApi();
        //     result.Result=api.GetIsRefund(subOrderNo11);
        //     return result;
        // }

        /// <summary>
        /// 售后类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<EnumEntity>> ListRefundType(string subOrderNo)
        {
            var result = new Response<List<EnumEntity>>();
            List<EnumEntity> listShow = new List<EnumEntity>();
            List<EnumEntity> listDb = xEnum.ListEnumModel(typeof(xEnum.RefundType));
         
            // foreach (var itemDb in listDb)
            // {
            //     if (listApiRefundType.Count(p=>p.code== itemDb.Value)>0)
            //     {
            //         listShow.Add(itemDb);
            //     } 
            // }
            result.Result = listShow;
            return result;
        }

        /// <summary>
        ///售后原因
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResDictionary>> ListRefundReason()
        {
            var result = new Response<List<ResDictionary>>();
            result.Result = _appRefundReason.ListTree("RefundReason");
            return result;
        }

        /// <summary>
        /// 退货退款类型
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<ResRefundType> DdlRefundType()
        {
            Response<ResRefundType> res = new Response<ResRefundType>();
            res.Result = new ResRefundType()
            {
                ListType = xEnum.ListEnumModel(typeof(xEnum.RefundType)),
                RefundExplain = _appDic.GetDicByCode("RefundExplain").Name
            };
            return res;
        }

        /// <summary>
        /// 根据Code获取字典
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<ResDic> GetDicByCode([FromQuery] ReqGetDicByCode req)
        {
            Response<ResDic> res = new Response<ResDic>();
            res.Result = _appDic.GetDicByCode(req.Code);
            return res;
        }

        /// <summary>
        /// 退货退款类型
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<EnumEntity>> DdlRefundReturnWay()
        {
            Response<List<EnumEntity>> res = new Response<List<EnumEntity>>();
            res.Result = xEnum.ListEnumModel(typeof(xEnum.RefundReturnWay));
            return res;
        }


        /// <summary>
        /// 车型
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResDic>> DdlCarType()
        {
            Response<List<ResDic>> res = new Response<List<ResDic>>();
            res.Result = _appDic.ListDicByTypeId("CarType",true);
            return res;
        }
        /// <summary>
        /// 退货退款理由
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResDic>> DdlRefundReason()
        {
            Response<List<ResDic>> res = new Response<List<ResDic>>();
            res.Result= _appDic.ListDicByTypeId("RefundReason");
            return res;
        }
        /// <summary>
        /// 服务售后理由
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResDic>> DdlServiceRefundReason()
        {
            Response<List<ResDic>> res = new Response<List<ResDic>>();
            res.Result = _appDic.ListDicByTypeId("ServiceRefundReason");
            return res;
        }

        /// <summary>
        /// 省市区
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<dynamic> AnalysisProvinceCity([FromQuery]ReqProvince req)
        {
            Response<dynamic> res = new Response<dynamic>();
            res.Result = xConv.AnalysisProvinceCity(req.StrProvince);
            return res;
        }

       

       
    }
}
