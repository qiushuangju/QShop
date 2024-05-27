// ***********************************************************************
// <summary>
// 超级管理员授权策略
// </summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App.AuthStrategies
{
    /// <summary>
    /// 领域服务
    /// <para>超级管理员权限</para>
    /// <para>超级管理员使用guid.empty为ID，可以根据需要修改</para>
    /// </summary>
    public class SystemAuthStrategy : AppBaseString<ModelUser, QsDBContext>, IAuthStrategy
    {
        protected ModelUser _user;
        private DbExtension _dbExtension;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitWork"></param>
        /// <param name="repository"></param>
        /// <param name="dbExtension"></param>
        public SystemAuthStrategy(IUnitWork<QsDBContext> unitWork, IRepository<ModelUser, QsDBContext> repository, DbExtension dbExtension ) : base(unitWork, repository, dbExtension, null)
        {
            _dbExtension = dbExtension;
            _user = new ModelUser
            {
                Account = Define.SystemUserName,
                NickName = "超级管理员",
                Id = xConv.NewGuid()
            };
        }
        

        public List<ModuleView> Modules
        {
            get {
                var modules = (from module in UnitWork.Find<Module>(p=>p.Status==(int)xEnum.ComStatus.Normal)
                    select new ModuleView
                    {
                        SortNo = module.SortNo,
                        Name = module.Name,
                        Id = module.Id,
                        CascadeId = module.CascadeId,
                        Code = module.Code,
                        IconName = module.IconName,
                        Url = module.Url,
                        ParentId = module.ParentId,
                        ParentName = module.ParentName,
                        IsSys = module.IsSys,
                        Status = module.Status
                    }).OrderBy(p=>p.SortNo).ToList();

                foreach (var module in modules)
                {
                    module.Elements = UnitWork.Find<ModuleElement>(u => u.ModuleId == module.Id).ToList();
                }

                return modules;
            }
        }

        public List<Role> Roles
        {
            get { return UnitWork.Find<Role>(null).ToList(); }
        }

        public List<ModuleElement> ModuleBtns
        {
            get { return UnitWork.Find<ModuleElement>(null).ToList(); }
        }

        public ModelUser User
        {
            get { return _user; }
            set   //禁止外部设置
            {
                throw new Exception("超级管理员，禁止设置用户");
            }  
        }         
    }
}