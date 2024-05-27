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
    /// 数据库Id为numberic且为数据库自动生成的业务类型
    /// <para>该场景通常为SqlServer的自动增长类型和Oracle自带的Sequence</para>
    /// 业务层基类，UnitWork用于事务操作，Repository用于普通的数据库操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppBaseIntAutoGen<T, TDbContext> :AppBase<T,TDbContext> where T : IntAutoGenEntity where TDbContext: DbContext
    {
        public AppBaseIntAutoGen(IUnitWork<TDbContext> unitWork, IRepository<T,TDbContext> repository, IAuth auth) : base(unitWork, repository, auth)
        {
        }

        /// <summary>
        /// 按id批量删除
        /// </summary>
        /// <param name="ids"></param>
        public void Delete(int[] ids)
        {
            Repository.Delete(u => ids.Contains(u.Id));
        }

        public T Get(int id)
        {
            return Repository.FirstOrDefault(u => u.Id == id);
        }
    }
}