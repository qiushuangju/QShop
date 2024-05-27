using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// tb_Coupon操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppCoupon _app;
        
        /// <summary>
        /// /构造函数
        /// </summary>
        public CouponController(AppCoupon app) 
        {
            _app = app;
        }
        
        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableData Load([FromQuery]ReqQuCoupon req)
        {
            return _app.Load(req);
        }
        
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public Response<List<ResCoupon>> ListByWhere([FromQuery]ReqQuCoupon req)
        {
            Response<List<ResCoupon>> res = new Response<List<ResCoupon>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }

        /// <summary>
        /// 领取优惠券
        /// </summary>
        [HttpPost]
        public Response<bool> ReceiveCoupon([FromBody]ReqReceiveCoupon req)
        {
            var result = new Response<bool>();
            _app.ReceiveCoupon(req);
            result.Result =true;
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelCoupon> Get(string id)
        {
            var result = new Response<ModelCoupon>();
            result.Result = _app.Get(id);
            return result;
        }
        
        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody]ReqAuCoupon req)
        {
            var result = new Response();
            try 
            {
                _app.AddOrUpdate(req);
            }
            catch (Exception e) 
            {

                result.Code = 500;
                result.Message = e.Message;
            }
            
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody]string[] ids)
        {
            var result = new Response();
            _app.Delete(ids);
            return result;
        }  
    }
}
