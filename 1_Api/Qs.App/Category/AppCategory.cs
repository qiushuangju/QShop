using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qs.App.Base;
using Qs.Comm;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Response;


namespace Qs.App
{
    /// <summary>
    /// 字典
    /// </summary>
    public class AppCategory : AppBaseString<Category,QsDBContext>
    {

        public AppCategory(IUnitWork<QsDBContext> unitWork, IRepository<Category, QsDBContext> repository, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
        }
        /// <summary>
        /// 加载列表
        /// </summary>
        public async Task<TableData> Load(QueryCategoryListReq request)
        {
            var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            var objs = UnitWork.Find<Category>(null);
            if (!string.IsNullOrEmpty(request.TypeId))
            {
                objs = objs.Where(u => u.TypeId == request.TypeId);
            }
            
            if (!string.IsNullOrEmpty(request.Key))
            {
                objs = objs.Where(u => u.Id.Contains(request.Key) || u.Name.Contains(request.Key));
            }

            result.Result = objs.OrderBy(u => u.DtCode)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit).ToList();
            result.Count = objs.Count();
            return result;
        }

        public void Add(AddOrUpdateCategoryReq req)
        {
            var obj = req.MapTo<Category>();
            obj.CreateTime = DateTime.Now;
            var user = _auth.GetCurrentContext().User;
            obj.CreateUserId = user.Id;
            obj.CreateUserName = user.NickName;
            Repository.Add(obj);
        }
        
        public void Update(AddOrUpdateCategoryReq obj)
        {
            var user = _auth.GetCurrentContext().User;
            UnitWork.Update<Category>(u => u.Id == obj.Id, u => new Category
            {
                Enable = obj.Enable,
                DtValue = obj.DtValue,
                DtCode = obj.DtCode,
                TypeId = obj.TypeId,
                UpdateTime = DateTime.Now,
                UpdateUserId = user.Id,
                UpdateUserName = user.NickName
            });

        }

        /// <summary>
        /// 加载一个分类类型里面的所有值，即字典的所有值
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<Category> LoadByTypeId(string typeId)
        {
            return Repository.Find(u => u.TypeId == typeId).ToList();
        }

        /// <summary>
        /// 加载一个分类类型里面的所有值，即字典的所有值
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="addAll">是否插入请选择</param>
        /// <returns></returns>
        public List<ResDic> ListDicByTypeId(string typeId,bool addAll=false)
        {
            List<ResDic> list = new List<ResDic>();
            if (addAll)
            {
                list.Add(new ResDic(){DtValue = "",Name = "全部"});
            }
            var listCate = Repository.Find(u => u.TypeId == typeId).OrderBy(p=>p.SortNo).ToList();
            foreach (var item in listCate)
            {
                list.Add(ResDic.ToDic(item));
            }  
            return list;
        }
        
        /// <summary>
        /// 加载一个分类类型里面的所有值，即字典的所有值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ResDic GetDicByCode(string code)
        {
            var model = Repository.FirstOrDefault(u => u.DtCode == code||u.DtValue==code);
            var vm = ResDic.ToDic(model);
        
            return vm;
        }

    }
}