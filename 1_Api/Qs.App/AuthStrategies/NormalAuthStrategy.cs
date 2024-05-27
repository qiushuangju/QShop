// ***********************************************************************
// <summary>
// 普通用户授权策略
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.Comm;
using Qs.App.Response;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App
{
    /// <summary>
    /// 普通用户授权策略
    /// </summary>
    public class NormalAuthStrategy :AppBaseString<ModelUser, QsDBContext>, IAuthStrategy
    {
        
        protected ModelUser _user;

        private List<string> _userRoleIds;    //用户角色GUID
        private DbExtension _dbExtension;
        public NormalAuthStrategy(IUnitWork<QsDBContext> unitWork, IRepository<ModelUser, QsDBContext> repository, DbExtension dbExtension) : base(unitWork, repository, dbExtension, null)
        {
            _dbExtension = dbExtension;
        }
        public List<ModuleView> Modules
        {
            get {
                var moduleIds = UnitWork.Find<Relevance>(
                    u =>(u.Key == Define.RoleModule && _userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId);

                var modules = (from module in UnitWork.Find<Module>(u =>moduleIds.Contains(u.Id)
                                                                        &&u.Status==(int)xEnum.ComStatus.Normal)
                    select new ModuleView
                    {
                        SortNo = module.SortNo,
                        Name = module.Name,
                        Code = module.Code,
                        CascadeId = module.CascadeId,
                        Id = module.Id,
                        IconName = module.IconName,
                        Url = module.Url,
                        ParentId = module.ParentId,
                        ParentName = module.ParentName,
                        IsSys = module.IsSys,
                        Status = module.Status
                    }).OrderBy(p => p.SortNo).ToList();

                var userElements = ModuleBtns;

                foreach (var module in modules)
                {
                    module.Elements = userElements.Where(u => u.ModuleId == module.Id).ToList();
                }

                return modules;
            }
        }

        /// <summary>
        /// 小程序店铺Id
        /// </summary>
        public string StoreId { get; set; }

        public List<ModuleElement> ModuleBtns
        {
            get
            {
                var elementIds = UnitWork.Find<Relevance>(u =>(u.Key == Define.RoleElement && _userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId);
                var userElements = UnitWork.Find<ModuleElement>(u => elementIds.Contains(u.Id));
                return userElements.ToList();
            }
        }

        public List<Role> Roles
        {
            get { return UnitWork.Find<Role>(u => _userRoleIds.Contains(u.Id)).ToList(); }
        }

        public ModelUser User
        {
            get { return _user; }
            set
            {
                _user = value;
                _userRoleIds = UnitWork.Find<Relevance>(u => u.FirstId == _user.Id && u.Key == Define.UserRole).Select(u => u.SecondId).ToList();
            }
        }
    }
}