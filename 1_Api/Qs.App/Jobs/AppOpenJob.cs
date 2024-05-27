using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qs.Comm;
using Microsoft.Extensions.Logging;
using Qs.App.Base;
using Qs.App.Extensions;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm.Model;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Quartz;


namespace Qs.App
{
    /// <summary>
    /// 系统定时任务管理
    /// </summary>
    public class AppOpenJob : AppBaseString<OpenJob, QsDBContext>
    {
        private IScheduler _scheduler;
        private ILogger<AppOpenJob> _logger;
        public AppOpenJob(IUnitWork<QsDBContext> unitWork, IRepository<OpenJob, QsDBContext> repository,IScheduler scheduler, ILogger<AppOpenJob> logger, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _scheduler = scheduler;
            _logger = logger;
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        public async Task<TableData> Load(QueryOpenJobListReq request)
        {
            var result = new TableData();
            var objs = Repository.Find(null);
            if (!string.IsNullOrEmpty(request.Key))
            {
                objs = objs.Where(u => u.Id.Contains(request.Key));
            }

            result.Result = objs.OrderBy(u => u.Id)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit);
            result.Count = objs.Count();
            return result;
        }

        /// <summary>
        /// 启动所有状态为正在运行的任务
        /// <para>通常应用在系统加载的时候</para>
        /// </summary>
        /// <returns></returns>
        public async Task StartAll()
        {
            var jobs = Repository.Find(u => u.Status == (int) JobStatus.Running);
            foreach (var job in jobs)
            {
                job.Start(_scheduler);
            }
            _logger.LogInformation("所有状态为正在运行的任务已启动");

        }

        public void Add(AddOrUpdateOpenJobReq req)
        {
            var obj = req.MapTo<OpenJob>();
            obj.CreateTime = DateTime.Now;
            var user = _auth.GetCurrentContext().User;
            obj.CreateUserId = user.Id;
            obj.CreateUserName = user.NickName;
            Repository.Add(obj);
        }

        public void Update(AddOrUpdateOpenJobReq obj)
        {
            var user = _auth.GetCurrentContext().User;
            UnitWork.Update<OpenJob>(u => u.Id == obj.Id, u => new OpenJob
            {
                JobName = obj.JobName,
                JobType = obj.JobType,
                JobCall = obj.JobCall,
                JobCallParams = obj.JobCallParams,
                Cron = obj.Cron,
                Status = obj.Status,
                Remark = obj.Remark,
                UpdateTime = DateTime.Now,
                UpdateUserId = user.Id,
                UpdateUserName = user.NickName
            });
        }

        #region 定时任务运行相关操作

        /// <summary>
        /// 返回系统的job接口
        /// </summary>
        /// <returns></returns>
        public List<string> QueryLocalHandlers()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces()
                    .Contains(typeof(IJob))))
                .ToArray();
            return types.Select(u => u.FullName).ToList();
        }

        public void ChangeJobStatus(ChangeJobStatusReq req)
        {
            var job = Repository.FirstOrDefault(u => u.Id == req.Id);
            if (job == null)
            {
                throw new Exception("任务不存在");
            }


            if (req.Status == (int) JobStatus.NotRun) //停止
            {
                job.Stop(_scheduler);
            }
            else //启动
            {
                job.Start(_scheduler);
            }


            var user = _auth.GetCurrentContext().User;

            job.Status = req.Status;
            job.UpdateTime = DateTime.Now;
            job.UpdateUserId = user.Id;
            job.UpdateUserName = user.NickName;
            Repository.Update(job);
        }
        /// <summary>
        /// 记录任务运行结果
        /// </summary>
        /// <param name="jobId"></param>
        public void RecordRun(string jobId)
        {
            var job = Repository.FirstOrDefault(u => u.Id == jobId);
            if (job == null)
            {
                xLog.Add(new ModelLog()
                {
                    Title = $"未能找到定时任务：{jobId}",
                    Href = $"OpenJobApp.RecordRun",
                    CreateName = _auth.GetUserName(),
                    // CreateId = _auth.GetCurrentUser().User.Id,
                    TypeName = "错误日志",
                    ApiInContent = "",
                    ApiOutContent = ""
                }, xEnum.LogLevel.Error);
                return;
            }

            job.RunCount++;
            job.LastRunTime = DateTime.Now;
            Repository.Update(job);
        }

        #endregion


       
    }
}