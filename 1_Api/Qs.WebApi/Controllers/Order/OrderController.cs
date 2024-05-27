using System;
using System.Collections.Generic;
using App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qs.App;
using Qs.App.ApiKuaiDiNiao.Res;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// tb_Order操作
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppOrder _app;
        private readonly AppExpress _appExpress;
        protected IAuth _auth;

        /// <summary>
        /// /构造函数
        /// </summary>
        public OrderController(AppOrder app,  AppExpress appExpress, IAuth auth)
        {
            _app = app;
            _appExpress = appExpress;
        }

        /// <summary>
        /// 列表查询(分页)
        /// </summary>
        [HttpGet]
        public TableDataOrder Load([FromQuery] ReqQuOrder req)
        {
            return _app.Load(req);
        }   

       
        /// <summary>
        /// 列表查询(不分页,page,limit不需要传)
        /// </summary>
        [HttpGet]
        public Response<List<ResOrder>> ListByWhere([FromQuery] ReqQuOrder req)
        {
            Response<List<ResOrder>> res = new Response<List<ResOrder>>();
            res.Result = _app.ListByWhere(req);
            return res;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        [HttpGet]
        public Response<ModelOrder> Get(string id)
        {
            var result = new Response<ModelOrder>();
            result.Result = _app.Get(id);
            return result;
        }

        /// <summary>
        /// 获取订单结算信息
        /// </summary>
        [HttpGet]
        public Response<ResOrderSettlement> GetOrderSettlement([FromQuery] ReqOrderSettlement req)
        {
            var result = new Response<ResOrderSettlement>();
            result.Result = _app.GetOrderSettlement(req);
            return result;
        }

        /// <summary>
        /// 创建订单
        /// </summary>
        [HttpPost]
        public Response<dynamic> CreateOrder([FromBody] ReqOrderSettlement req)
        {
            var result = new Response<dynamic>();
            result.Result = _app.CreateOrder(req);
            return result;
        }

        /// <summary>
        /// 支付前
        /// </summary>
        [HttpPost]
        public Response<dynamic> PayOrderBefore([FromBody] ReqPayOrderBefore req)
        {
            var result = new Response<dynamic>();
            result.Result = _app.PayOrderBefore(req.OrderId,req.PayType);
            return result;
        }
        /// <summary>
        /// 获取订单状态数量
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]                                     
        public Response<ResGetOrderGroupCount> GetOrderGroupCount([FromQuery] ReqGetOrderGroupCount req)
        {
            Response<ResGetOrderGroupCount> res = new Response<ResGetOrderGroupCount>();
            res.Result= _app.GetOrderGroupCount(req);
            return res;
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        [HttpGet]
        public Response<ResOrder> GetDetail(string id)
        {
            var result = new Response<ResOrder>();
            result.Result = _app.GetDetail(id);
            return result;
        }
     

        /// <summary>
        /// 取消订单
        /// </summary>
        [HttpPost]
        public Response<bool> CancelOrder([FromBody] ReqCancelOrder req)
        {
            req.Check();
            var result = new Response<bool>();
            _app.CancelOrder(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 申请取消订单
        /// </summary>
        [HttpPost]
        public Response<bool> ApplyCancelOrder([FromBody] ReqCancelOrder req)
        {
            req.Check();
            var result = new Response<bool>();
            _app.ApplyCancelOrder(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 申请取消订单-审核
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> AuditCancel(ReqAuditCancel req)
        {
            req.Check();
            var result = new Response<bool>();
            _app.AuditCancel(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 订单发货
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> Delivery(ReqDelivery req)
        {
            req.Check();
            var result = new Response<bool>();
            _app.Delivery(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<bool> Receipt(ReqReceipt req)
        {
            req.Check();
            var result = new Response<bool>();
            _app.Receipt(req);
            result.Result = true;
            return result;
        }

        /// <summary>
        /// 获取支付参数(他人代付)
        /// </summary>
        [HttpPost]
        public Response<object> GetPayParams([FromBody] ReqGetPayParam req)
        {
            req.Check();
            var result = new Response<object>();
            result.Result = _app.GetPayParams(req);
            return result;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        [HttpPost]
        public Response AddOrUpdate([FromBody] ReqAuOrder req)
        {
            var result = new Response();
            _app.AddOrUpdate(req);
            return result;
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        [HttpPost]
        public Response Delete([FromBody] string[] ids)
        {
            var result = new Response();
            _app.Delete(ids);
            return result;
        }

        /// <summary>
        /// 快递查询
        /// </summary>
        /// <param name="orderSkuId"></param>
        [AllowAnonymous]
        [HttpGet]
        public Response<ResListTrack> ListTrack(string orderSkuId)
        {
            Response<ResListTrack> res = new Response<ResListTrack>();
            res.Result= _appExpress.ListTrack(orderSkuId);
            return res;
        }

        /// <summary>
        /// 快递查询最后轨迹
        /// </summary>
        /// <param name="orderSkuId"></param>
        [AllowAnonymous]
        [HttpGet]
        public Response<TrackInfo> GetLasetTrack(string orderSkuId)
        {
            Response<TrackInfo> res = new Response<TrackInfo>();
            res.Result = _appExpress.GetLasetTrack(orderSkuId);
            return res;
        }

        /// <summary>
        /// 导出订单详情
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExportOrder([FromQuery] ReqQuOrder req)
        {
            var excelHeper = new ExcelHelper();
            var config = new List<ExcelGridModel>
            {
                new ExcelGridModel{name = "StoreName", label = "地区", align = "left",},
                new ExcelGridModel{name = "StorageName", label = "运营仓", align = "left",},
                new ExcelGridModel{name = "PayTime", label = "付款时间", align = "left",},
                new ExcelGridModel{name = "OrderTypeName", label = "订单类型", align = "left",},
                new ExcelGridModel{name = "StrDeliveryType", label = "配送类型", align = "left",},
                new ExcelGridModel{name = "OrderNo", label = "订单编号", align = "left",},
                new ExcelGridModel{name = "StrUserType", label = "用户类型", align = "left",},
                new ExcelGridModel{name = "LinkMan", label = "收货人", align = "left",},
                new ExcelGridModel{name = "Phone", label = "收货号码", align = "left",},
                new ExcelGridModel{name = "FullAddress", label = "收货地址", align = "left",},
                new ExcelGridModel{name = "GoodsName", label = "商品名称", align = "left",},
                new ExcelGridModel{name = "SkuName", label = "规格", align = "left",},
                new ExcelGridModel{name = "Unit", label = "单位", align = "left",},            
                new ExcelGridModel{name = "PayPrice", label = "实付单价", align = "left",},
                new ExcelGridModel{name = "TotalNum", label = "数量", align = "left",},
                new ExcelGridModel{name = "TotalCommission", label = "提成", align = "left",},
                new ExcelGridModel{name = "ExpectTimeSlot", label = "期望时间", align = "left",},
                new ExcelGridModel{name = "OutBoundTime", label = "出库时间", align = "left",},
                new ExcelGridModel{name = "ArrivedTime", label = "送达时间", align = "left",},
                new ExcelGridModel{name = "StrExportStatus", label = "完成状态", align = "left",},
                new ExcelGridModel{name = "StrOrderStatus", label = "订单状态", align = "left",},
                new ExcelGridModel{name = "DriverName", label = "司机", align = "left",},
                new ExcelGridModel{name = "FreightPrice", label = "运费", align = "left",},
                new ExcelGridModel{name = "FloorPrice", label = "上楼费", align = "left",},
                new ExcelGridModel{name = "EntryPrice", label = "入户费", align = "left",},
                new ExcelGridModel{name = "BuyerRemark", label = "备注", align = "left",},
            };
            List<ReqExportOrder> listDb = _app.ExportOrderListByWhere(req);
            var fileName = $"订单明细";
            return excelHeper.ExcelDownload(listDb, config, fileName);
        }

        /// <summary>
        /// 确认收货
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
       [HttpPost]
        public Response Received([FromBody] ReqOrderReceived req)
        {
            var result = new Response();
            try
            {
                _app.Received(req);
            }
            catch (Exception e)
            {
                result.Code = 500;
                result.Message = e.Message;
            }
            return result;
        }
    }
}
