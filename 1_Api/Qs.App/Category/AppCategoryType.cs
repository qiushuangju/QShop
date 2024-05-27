using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qs.App.Base;
using Qs.Comm;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App
{
    public class AppCategoryType : AppBaseString<CategoryType,QsDBContext>
    {
        public AppCategoryType(IUnitWork<QsDBContext> unitWork, IRepository<CategoryType, QsDBContext> repository,
          AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        public async Task<TableData> Load(QueryCategoryTypeListReq request)
        {
            var result = new TableData();
            var objs = UnitWork.Find<CategoryType>(null);
            if (!string.IsNullOrEmpty(request.Key))
            {
                objs = objs.Where(u => u.Id.Contains(request.Key) || u.Name.Contains(request.Key));
            }
            
            result.Result = objs.OrderBy(u => u.Name)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit);
            result.Count = objs.Count();
            return result;
        }

        public void Add(AddOrUpdateCategoryTypeReq req)
        {
            var obj = req.MapTo<CategoryType>();
            
            obj.CreateTime = DateTime.Now;
            Repository.Add(obj);
        }

         public void Update(AddOrUpdateCategoryTypeReq obj)
        {
            var user = _auth.GetCurrentContext().User;
            UnitWork.Update<CategoryType>(u => u.Id == obj.Id, u => new CategoryType
            {
                Name = obj.Name,
                CreateTime = DateTime.Now
                
            });

        }
         
         public new void Delete(string[] ids)
         {
             UnitWork.ExecuteWithTransaction(() =>
             {
                 UnitWork.Delete<CategoryType>(u=>ids.Contains(u.Id));
                 UnitWork.Delete<Category>(u=>ids.Contains(u.TypeId));
                 UnitWork.Save();
             });
          
         }
         
         public List<CategoryType> AllTypes()
         {
             return UnitWork.Find<CategoryType>(null).ToList();
         }

    }
}