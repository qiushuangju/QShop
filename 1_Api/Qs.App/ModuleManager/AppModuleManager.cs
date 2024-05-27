using System.Collections.Generic;
using System.Linq;
using Qs.Comm;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App
{
    /// <summary>
    /// 模块
    /// </summary>
    public class AppModule : AppBaseTree<Module,QsDBContext>
    {
        private AppRevelanceManager _appRevelance;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitWork"></param>
        /// <param name="repository"></param>
        /// <param name="app"></param>
        /// <param name="dbExtension"></param>
        /// <param name="auth"></param>
        public AppModule(IUnitWork<QsDBContext> unitWork, IRepository<Module, QsDBContext> repository
           , AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _appRevelance = app;
        }
        public void Add(Module model)
        {
            var loginContext = _auth.GetCurrentContext();
            CaculateCascade(model);
            Repository.Add(model);

            AddDefaultBtns(model);
           
        }

        /// <summary>
        /// 编辑模块
        /// </summary>
        /// <param name="obj"></param>
        public void Update(Module obj)
        {
            UpdateTreeObj(obj);
        }

        /// <summary>
        /// 删除模块(一并删除按钮,角色权限)
        /// </summary>
        /// <param name="ids"></param>
        public override void Delete(string[] ids)
        {
            var listEleId = UnitWork.Find<ModuleElement>(p => ids.Contains(p.ModuleId)).Select(p => p.Id);
            UnitWork.Delete<Module>(p => ids.Contains(p.Id));
            UnitWork.Delete<ModuleElement>(p => ids.Contains(p.ModuleId));
            UnitWork.Delete<Relevance>(p => listEleId.Contains(p.FirstId));
            UnitWork.Delete<Relevance>(p => ids.Contains(p.FirstId));
            UnitWork.Save();
        }

        #region 用户/角色分配模块

        /// <summary>
        /// 加载特定角色的模块
        /// </summary>
        /// <param name="roleId">The role unique identifier.</param>
        public IEnumerable<Module> LoadForRole(string roleId)
        {
            var moduleIds = UnitWork.Find<Relevance>(u => u.FirstId == roleId && u.Key == Define.RoleModule)
                .Select(u => u.SecondId);
            return UnitWork.Find<Module>(u => moduleIds.Contains(u.Id)&&u.Status==(int)xEnum.ComStatus.Normal).OrderBy(u => u.SortNo);
        }

        /// <summary>
        /// 根据某角色ID获取可访问某模块的按钮项
        /// </summary>
        public IEnumerable<ModuleElement> LoadBtnsForRole(string moduleId, string roleId)
        {
            var elementIds = _appRevelance.Get(Define.RoleElement, true, roleId);
            var query = UnitWork.Find<ModuleElement>(u => elementIds.Contains(u.Id));
            if (!string.IsNullOrEmpty(moduleId))
            {
                query = query.Where(u => u.ModuleId == moduleId);
            }
            return query;
        }

        #endregion 用户/角色分配模块


        #region 按钮操作

        /// <summary>
        /// 删除指定的按钮
        /// </summary>
        /// <param name="ids"></param>
        public void DelBtn(string[] ids)
        {
            UnitWork.Delete<ModuleElement>(u => ids.Contains(u.Id));
            UnitWork.Delete<ModuleElement>(u => ids.Contains(u.Id));
            UnitWork.Save();
        }


        /// <summary>
        /// 新增按钮
        /// <para>当前登录用户的所有角色会自动分配按钮</para>
        /// </summary>
        public void AddBtn(ModuleElement model)
        {
            var loginContext = _auth.GetCurrentContext();
            UnitWork.ExecuteWithTransaction(() =>
            {
                UnitWork.Add(model);
                UnitWork.Save();
            });
        }
         /// <summary>
         /// 修改按钮
         /// </summary>
         /// <param name="model"></param>
        public void UpdateBtn(ModuleElement model)
        {
            UnitWork.Update<ModuleElement>(model);
            UnitWork.Save();
        }

        //添加默认按钮
        private void AddDefaultBtns(Module module)
        {
            AddBtn(new ModuleElement
            {
                ModuleId = module.Id,
                BtnCode = "btnAdd",
                
                Name = "添加",
                Sort = 1,
                Remark = "新增" + module.Name
            });
            AddBtn(new ModuleElement
            {
                ModuleId = module.Id,
                BtnCode = "btnEdit",
                Name = "编辑",
                Sort = 2,
                Remark = "修改" + module.Name
            });
            AddBtn(new ModuleElement
            {
                ModuleId = module.Id,
                BtnCode = "btnDel",
               
                Name = "删除",
                Sort = 3,
                Remark = "删除" + module.Name
            });

            //todo:可以自己添加更多默认按钮
        }

        #endregion


        
    }
}