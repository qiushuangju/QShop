using System.Linq;
using Microsoft.EntityFrameworkCore;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Repository.Core;
using Qs.Repository.Interface;

namespace Qs.App
{
    /// <summary>
    /// 树状结构处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AppBaseTree<T,TDbContext> :AppBaseString<T,TDbContext> where T : TreeEntity where TDbContext :DbContext
    {


        public AppBaseTree(IUnitWork<TDbContext> unitWork, IRepository<T, TDbContext> repository,  DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
        }

       
        /// <summary>
        /// 更新树状结构实体
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="U"></typeparam>
        public void UpdateTreeObj<U>(U obj) where U : TreeEntity
        {
            CaculateCascade(obj);

            //获取旧的的CascadeId
            var cascadeId = Repository.FirstOrDefault(o => o.Id == obj.Id).CascadeId;
            //根据CascadeId查询子部门
            var objs = Repository.Find(u => u.CascadeId.Contains(cascadeId) && u.Id != obj.Id)
                .OrderBy(u => u.CascadeId).ToList();

            //更新操作
            UnitWork.Update(obj);

            //更新子模块的CascadeId
            foreach (var a in objs)
            {
                a.CascadeId = a.CascadeId.Replace(cascadeId, obj.CascadeId);
                if (a.ParentId == obj.Id)
                {
                    a.ParentName = obj.Name;
                }

                UnitWork.Update(a);
            }

            UnitWork.Save();
        }

       
    }
}