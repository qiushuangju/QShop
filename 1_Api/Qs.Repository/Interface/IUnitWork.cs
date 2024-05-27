
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qs.Repository.Core;

namespace Qs.Repository.Interface
{
    /// <summary>
    /// 工作单元接口
    /// 使用详见：
    /// <para> 适合在一下情况使用:</para>
    /// <para>1 在同一事务中进行多表操作</para>
    /// <para>2 需要多表联合查询</para>
    /// <para>因为架构采用的是EF访问数据库，暂时可以不用考虑采用传统Unit Work的注册机制</para>
    /// </summary>
    public interface IUnitWork<TDbContext> where TDbContext : DbContext
    {
        /// <summary>
        /// EF默认情况下，每调用一次SaveChanges()都会执行一个单独的事务
        /// 本接口实现在一个事务中可以多次执行SaveChanges()方法
        /// </summary>
        void ExecuteWithTransaction(Action action);
        /// <summary>
        /// ExecuteWithTransaction方法的异步方式
        /// </summary>
        Task ExecuteWithTransactionAsync(Func<Task> action);
        /// <summary>
        /// 返回DbContext,用于多线程等极端情况
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
        /// <summary>
        /// 返回一个单独的实体，如果记录多于1个则取第一个
        /// </summary>
        T FirstOrDefault<T>(Expression<Func<T, bool>> exp = null) where T : class;
        /// <summary>
        /// 判断指定条件的记录是否存在
        /// </summary>
        bool Any<T>(Expression<Func<T, bool>> exp) where T : class;
        IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null) where T : class;

        IQueryable<T> Find<T>(Expression<Func<T, bool>> exp = null, string orderby = "", int pageindex = 1, int pagesize = 10) where T : class;
        int Sum<T>(Expression<Func<T, bool>> where, Func<T, int> selector) where T : class;
        decimal Sum<T>(Expression<Func<T, bool>> where, Func<T, decimal> selector) where T : class;
        int Count<T>(Expression<Func<T, bool>> exp = null) where T : class;

        /// <summary>
        /// 新增对象，如果Id为空，则会自动创建默认Id
        /// </summary>
        void Add<T>(T entity) where T : BaseEntity;

        /// <summary>
        /// 批量新增对象，如果对象Id为空，则会自动创建默认Id
        /// </summary>
        void BatchAdd<T>(List<T> entities) where T : BaseEntity;
        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        void AddOrUpdate<T>(T entity) where T : StringEntity;
        /// <summary>
        /// 更新一个实体的所有属性
        /// </summary>
        void Update<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;


        /// <summary>
        /// 实现按需要只更新部分更新
        /// <para>如：Update&lt;User&gt;(u =>u.Id==1,u =>new User{Name="ok"})</para>
        /// <para>该方法内部自动调用了SaveChanges()，需要ExecuteWithTransaction配合才能实现事务控制</para>
        /// </summary>
        /// <param name="where">更新条件</param>
        /// <param name="entity">更新后的实体</param>
        void Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> entity) where T : class;
        /// <summary>
        /// 批量删除
        /// <para>该方法内部自动调用了SaveChanges()，需要ExecuteWithTransaction配合才能实现事务控制</para>
        /// </summary>
        void Delete<T>(Expression<Func<T, bool>> exp) where T : class;

        void Save();

        /// <summary>
        /// 该方法不支持EF自带的事务,需要ExecuteWithTransaction配合才能实现事务控制,详见:
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExecuteSql(string sql);

        /// <summary>
        /// 使用SQL脚本查询
        /// </summary>
        /// <typeparam name="T"> T为数据库实体</typeparam>
        /// <returns></returns>
        IQueryable<T> FromSql<T>(string sql, params object[] parameters) where T : class;
        /// <summary>
        /// 使用SQL脚本查询
        /// </summary>
        /// <typeparam name="T"> T为非数据库实体，需要在DbContext中增加对应的DbQuery</typeparam>
        /// <returns></returns>
        IQueryable<T> Query<T>(string sql, params object[] parameters) where T : class;

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sqlParams">存储过程参数</param>
        List<T> ExecProcedure<T>(string procName, params DbParameter[] sqlParams) where T : class;

        #region 异步接口

        Task<int> ExecuteSqlRawAsync(string sql);
        Task<int> SaveAsync();
        Task<int> CountAsync<T>(Expression<Func<T, bool>> exp = null) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> exp) where T : class;
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> exp) where T : class;

        #endregion
    }
}
