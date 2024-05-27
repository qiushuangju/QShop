using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using Microsoft.AspNetCore.Authorization;
using Qs.App;
using Qs.App.Response;
using Qs.Repository.Base;
using Qs.Repository.Request;
using Qs.Comm;
using Qs.Repository.Response;
using Qs.App.UserManager;
using Qs.App.UserManager.Response;
using Qs.Repository.Vm;

namespace Qs.WebApi.Controllers
{
    /// <summary>
    /// 统计
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
       
        private readonly AppInviteLinkRecord _appInviteLinkRecord;
        private readonly AppStatistic _app;

        /// <summary>
        /// /构造函数
        /// </summary>
        public StatisticController(AppStatistic app,AppInviteLinkRecord appInviteLinkRecord)
        {
            _app = app;
            _appInviteLinkRecord = appInviteLinkRecord;
        }

        /// <summary>
        /// 我的团队
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ResTeamStatistics> StatisticTeam([FromQuery]ReqTeamStatistics req)
        {
            Response<ResTeamStatistics> res = new Response<ResTeamStatistics>();
            res.Result = _app.StatisticTeam(req);
            return res;
        }
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<ResPcHomePage> LoadPcHomePageData()
        {
            Response<ResPcHomePage> res = new Response<ResPcHomePage>();
            res.Result = _app.LoadPcHomePageData();
            return res;
        }
        

        /// <summary>
        /// 售后-按日统计（已送达）
        /// </summary>
        [HttpGet]
        public TableData LoadRefundOrderByDay([FromQuery] ReqQuArticleCategory req)
        {
            string start = req.StartDate?.Date.ToString();
            string end = req.EndDate?.Date.ToString();
            var result = new TableData();
            // var linq = _appOrderRefund.ListLinqRefundOrderByDay(start, end, (int)xEnum.RefundStatus.WaitInStock).ToList();
            // var listVm = linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit);
            // result.Result = listVm;
            // result.Count = linq.Count();
            return result;
        }

        /// <summary>
        /// 运营统计(列表)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Response<List<VmStatisticOperate>> StatisticOperate([FromQuery] ReqStatisticOperate req)
        {
            List<VmStatisticOperate> list = _app.ListStatisticOperate(req);
            Response<List<VmStatisticOperate>> res = new Response<List<VmStatisticOperate>>();
            res.Result = list;
            return res;
        }

        /// <summary>
        /// 我的邀请统计（已邀请，已办卡）
        /// </summary>
        [HttpGet]
        public Response<ResInviteLinkRecordStatistics> MyInviteLinkRecordStatistics([FromQuery] ReqStatistic req)
        {
            var result = new Response<ResInviteLinkRecordStatistics>();
            result.Result = _appInviteLinkRecord.MyInviteLinkRecordStatistics(req);
            return result;
        }

      
    }
}
