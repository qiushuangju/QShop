using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Repository.Core;
using Qs.Repository.Interface;

namespace Qs.App.Base
{
    /// <summary>
    /// 业务层基类，UnitWork用于事务操作，Repository用于普通的数据库操作
    /// <para>如用户管理：Class UserManagerApp:BaseApp<User></para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppBaseString<T, TDbContext> :AppBase<T,TDbContext> where T : StringEntity where TDbContext: DbContext
    {
        protected DbExtension _dbExtension;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitWork"></param>
        /// <param name="repository"></param>
        /// <param name="auth"></param>
        public AppBaseString(IUnitWork<TDbContext> unitWork, IRepository<T,TDbContext> repository, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, auth)
        {
            _dbExtension = dbExtension;

        }

        /// <summary>
        /// 按id批量删除
        /// </summary>
        /// <param name="ids"></param>
        public virtual void Delete(string[] ids)
        {
            Repository.Delete(u => ids.Contains(u.Id));
        }

        public T Get(string id)
        {
            return Repository.FirstOrDefault(u => u.Id == id);
        }
        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> exp)
        {
            return Repository.FirstOrDefault(exp);
        }
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        public List<KeyDescription> GetProperties(string moduleCode)
        {
            return _dbExtension.GetProperties(moduleCode);
        }
    }
}