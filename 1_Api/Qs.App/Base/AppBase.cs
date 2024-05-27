using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Qs.App.Interface;
using Qs.Repository.Core;
using Qs.Repository.Interface;

namespace Qs.App
{
    public abstract class AppBase<T, TDbContext> where T : class where TDbContext: DbContext
    {
        /// <summary>
        /// 用于普通的数据库操作
        /// </summary>
        protected IRepository<T, TDbContext> Repository;

        /// <summary>
        /// 用于事务操作
        /// <para>使用详见：http://doc.Qs.net.cn/core/unitwork.html</para>
        /// </summary>
        protected IUnitWork<TDbContext> UnitWork;

        protected IAuth _auth;

        public AppBase(IUnitWork<TDbContext> unitWork, IRepository<T,TDbContext> repository, IAuth auth)
        {
            UnitWork = unitWork;
            Repository = repository;
            _auth = auth;
        }

       
        
        /// <summary>
        /// 计算实体更新的层级信息
        /// </summary>
        /// <typeparam name="U">U必须是一个继承TreeEntity的结构</typeparam>
        /// <param name="entity"></param>
        public void CaculateCascade<U>(U entity) where U : TreeEntity
        {
            if (entity.ParentId == "") entity.ParentId = null;
            string cascadeId;
            int currentCascadeId = 1; //当前结点的级联节点最后一位
            var sameLevels = UnitWork.Find<U>(o => o.ParentId == entity.ParentId && o.Id != entity.Id);
            foreach (var obj in sameLevels)
            {
                int objCascadeId = int.Parse(obj.CascadeId.TrimEnd('.').Split('.').Last());
                if (currentCascadeId <= objCascadeId) currentCascadeId = objCascadeId + 1;
            }

            if (!string.IsNullOrEmpty(entity.ParentId))
            {
                var parentOrg = UnitWork.FirstOrDefault<U>(o => o.Id == entity.ParentId);
                if (parentOrg != null)
                {
                    cascadeId = parentOrg.CascadeId + currentCascadeId + ".";
                    entity.ParentName = parentOrg.Name;
                }
                else
                {
                    throw new Exception("未能找到该组织的父节点信息");
                }
            }
            else
            {
                cascadeId = ".0." + currentCascadeId + ".";
                entity.ParentName = "根节点";
            }

            entity.CascadeId = cascadeId;
        }
    }
}