using Qs.App.UserManager;
using Qs.Comm;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.App.Jobs
{
    /// <summary>
    /// 检测会员 经销商 代理 是否到期
    /// </summary>
    public class JobCheckVipDue : IJob
    {
        private AppOpenJob _openJobApp;
        private AppUserManager _appUserManager;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="openJobApp"></param>
        /// <param name="appUserManager"></param>
        public JobCheckVipDue(AppOpenJob openJobApp, AppUserManager appUserManager)
        {
            _openJobApp = openJobApp;
            _appUserManager = appUserManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            var jobId = context.MergedJobDataMap.GetString(Define.JobMapKey);
            _appUserManager.JobCheckVipDue();
            _openJobApp.RecordRun(jobId);
            return Task.Delay(1);
        }
    }
}
