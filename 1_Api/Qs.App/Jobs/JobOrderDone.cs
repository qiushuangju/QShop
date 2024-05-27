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
    ///订单定时已完成（15天）
    /// </summary>
    public class JobOrderDone : IJob
    {
        private AppOpenJob _openJobApp;
        private AppOrder _appOrder;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="openJobApp"></param>
        /// <param name="appOrder"></param>
        public JobOrderDone(AppOpenJob openJobApp, AppOrder appOrder)
        {
            _openJobApp = openJobApp;
            _appOrder = appOrder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Execute(IJobExecutionContext context)
        {
            var jobId = context.MergedJobDataMap.GetString(Define.JobMapKey);
            _appOrder.OrderReceipt();
            _appOrder.OrderDone();
            _openJobApp.RecordRun(jobId);
            return Task.Delay(1);
        }

    }
}
