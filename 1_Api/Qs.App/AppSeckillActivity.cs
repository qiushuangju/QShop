using Microsoft.EntityFrameworkCore;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Repository;
using Qs.Repository.Core;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qs.App
{
    /// <summary>
    /// 秒杀活动服务
    /// </summary>
    public class AppSeckillActivity : AppBaseLong<ModelSeckillActivity, QsDBContext>
    {
        public AppSeckillActivity(IUnitWork<QsDBContext> unitWork, IRepository<ModelSeckillActivity, QsDBContext> repository, IAuth auth)
            : base(unitWork, repository, auth)
        { }

        /// <summary>
        /// 创建秒杀活动
        /// </summary>
        public async Task<decimal> CreateActivity(ModelSeckillActivity activity)
        {
            activity.CreateTime = DateTime.Now;
            activity.CreateUserId = _auth.GetUserId();
            activity.UpdateTime = DateTime.Now;
            activity.UpdateUserId = _auth.GetUserId();

            // 根据活动时间设置初始状态
            if (activity.StartTime > DateTime.Now)
            {
                activity.Status = 1; // 待开始
            }
            else if (activity.StartTime <= DateTime.Now && activity.EndTime > DateTime.Now)
            {
                activity.Status = 2; // 进行中
            }
            else
            {
                activity.Status = 3; // 已结束
            }

            await Repository.AddAsync(activity);
            await UnitWork.SaveAsync();

            return activity.Id;
        }

        /// <summary>
        /// 更新秒杀活动
        /// </summary>
        public async Task UpdateActivity(ModelSeckillActivity activity)
        {
            var existingActivity = await Repository.FirstOrDefaultAsync(o => o.Id == activity.Id);
            if (existingActivity == null)
            {
                throw new Exception("秒杀活动不存在");
            }

            existingActivity.Title = activity.Title;
            existingActivity.Description = activity.Description;
            existingActivity.StartTime = activity.StartTime;
            existingActivity.EndTime = activity.EndTime;
            existingActivity.UpdateTime = DateTime.Now;
            existingActivity.UpdateUserId = _auth.GetUserId();

            // 根据活动时间更新状态
            if (existingActivity.StartTime > DateTime.Now)
            {
                existingActivity.Status = 1; // 待开始
            }
            else if (existingActivity.StartTime <= DateTime.Now && existingActivity.EndTime > DateTime.Now)
            {
                existingActivity.Status = 2; // 进行中
            }
            else
            {
                existingActivity.Status = 3; // 已结束
            }

            await Repository.UpdateAsync(existingActivity);
            await UnitWork.SaveAsync();
        }

        /// <summary>
        /// 获取秒杀活动列表
        /// </summary>
        public async Task<List<ModelSeckillActivity>> GetActivityList(int? status = null)
        {
            var query = Repository.FindAll();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            // 实时更新活动状态
            var activities = await query.ToListAsync();
            foreach (var activity in activities)
            {
                if (activity.StartTime > DateTime.Now && activity.Status != 1)
                {
                    activity.Status = 1; // 待开始
                    await Repository.UpdateAsync(activity);
                }
                else if (activity.StartTime <= DateTime.Now && activity.EndTime > DateTime.Now && activity.Status != 2)
                {
                    activity.Status = 2; // 进行中
                    await Repository.UpdateAsync(activity);
                }
                else if (activity.EndTime <= DateTime.Now && activity.Status != 3)
                {
                    activity.Status = 3; // 已结束
                    await Repository.UpdateAsync(activity);
                }
            }

            await UnitWork.SaveAsync();

            return activities.OrderBy(o => o.StartTime).ToList();
        }

        /// <summary>
        /// 获取秒杀活动详情
        /// </summary>
        public async Task<ModelSeckillActivity> GetActivityDetail(decimal id)
        {
            var activity = await Repository.FirstOrDefaultAsync(o => o.Id == id);
            if (activity == null)
            {
                throw new Exception("秒杀活动不存在");
            }

            // 实时更新活动状态
            if (activity.StartTime > DateTime.Now && activity.Status != 1)
            {
                activity.Status = 1; // 待开始
                await Repository.UpdateAsync(activity);
                await UnitWork.SaveAsync();
            }
            else if (activity.StartTime <= DateTime.Now && activity.EndTime > DateTime.Now && activity.Status != 2)
            {
                activity.Status = 2; // 进行中
                await Repository.UpdateAsync(activity);
                await UnitWork.SaveAsync();
            }
            else if (activity.EndTime <= DateTime.Now && activity.Status != 3)
            {
                activity.Status = 3; // 已结束
                await Repository.UpdateAsync(activity);
                await UnitWork.SaveAsync();
            }

            return activity;
        }

        /// <summary>
        /// 取消秒杀活动
        /// </summary>
        public async Task CancelActivity(decimal id)
        {
            var activity = await Repository.FirstOrDefaultAsync(o => o.Id == id);
            if (activity == null)
            {
                throw new Exception("秒杀活动不存在");
            }

            activity.Status = 4; // 已取消
            activity.UpdateTime = DateTime.Now;
            activity.UpdateUserId = _auth.GetUserId();

            await Repository.UpdateAsync(activity);
            await UnitWork.SaveAsync();
        }
    }
}
