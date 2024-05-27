using System;
using System.Linq;
using Qs.Comm;
using Microsoft.EntityFrameworkCore;
using Qs.App.Interface;
using Qs.Repository.Core;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App
{
    /// <summary>
    /// ⭐⭐数据库Id为numberic类型的数据表相关业务使用该基类⭐⭐
    /// 业务层基类，UnitWork用于事务操作，Repository用于普通的数据库操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppBaseLong<T, TDbContext> :AppBase<T,TDbContext> where T : LongEntity where TDbContext: DbContext
    {


        public AppBaseLong(IUnitWork<TDbContext> unitWork, IRepository<T,TDbContext> repository, IAuth auth) : base(unitWork, repository,auth)
        {
        }
        

        /// <summary>
        /// 按id批量删除
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(decimal[] ids)
        {
            Repository.Delete(u => ids.Contains(u.Id));
        }

        public T Get(decimal id)
        {
            return Repository.FirstOrDefault(u => u.Id == id);
        }
    }
}