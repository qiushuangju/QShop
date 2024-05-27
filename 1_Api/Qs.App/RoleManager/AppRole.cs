using System;
using System.Collections.Generic;
using Qs.App.Interface;
using Qs.App.Response;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using System.Linq;
using System.Threading.Tasks;
using Qs.App.Base;
using Qs.Comm;
using Qs.App.Request;
using Qs.Comm;
using Qs.Repository;

namespace Qs.App
{

    public class AppRole : AppBaseString<Role,QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;

        public AppRole(IUnitWork<QsDBContext> unitWork, IRepository<Role, QsDBContext> repository,
           AppRevelanceManager app, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }


        /// <summary>
        /// 加载当前登录用户可访问的全部角色
        /// </summary>
        public List<Role> Load(QueryRoleListReq request)
        {
            var loginUser = _auth.GetCurrentContext();
            var roles = loginUser.Roles;
            if (!string.IsNullOrEmpty(request.Key))
            {
                roles = roles.Where(u => u.Name.Contains(request.Key)).ToList();
            }

            return roles;
        }
        
        /// <summary>
        /// 获取所有的角色
        /// 为了控制权限，通常只用于流程实例选择执行角色，其他地方请使用Load
        /// </summary>
        public async Task<TableResp<Role>> LoadAll(QueryRoleListReq request)
        {
            var result = new TableResp<Role>();
            var objs = UnitWork.Find<Role>(null);
            if (!string.IsNullOrEmpty(request.Key))
            {
                objs = objs.Where(u => u.Name.Contains(request.Key));
            }

            result.Result = objs.OrderBy(u => u.Name)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit).ToList();
            // result.Count = objs.Count();
            return result;
        }


        /// <summary>
        /// 添加角色，如果当前登录用户不是System，则直接把新角色分配给当前登录用户
        /// </summary>
        public void Add(RoleView obj)
        {
           UnitWork.ExecuteWithTransaction(() =>
           {
               Role role = obj;
               role.CreateTime = DateTime.Now;
               UnitWork.Add(role);
               UnitWork.Save();
               obj.Id = role.Id;   //要把保存后的ID存入view
           
               //如果当前账号不是SYSTEM，则直接分配
               var loginUser = _auth.GetCurrentContext();
               if (loginUser.User.Account != Define.SystemUserName)
               {
                   _revelanceApp.Assign(new AssignReq
                   {
                       type = Define.UserRole,
                       firstId = loginUser.User.Id,
                       secIds = new[] {role.Id}
                   });
               }
           });
        }

        /// <summary>
        /// 删除角色时，需要删除角色对应的权限
        /// </summary>
        /// <param name="ids"></param>
        public override void Delete(string[] ids)
        {
            UnitWork.ExecuteWithTransaction(() =>
            {
                UnitWork.Delete<Relevance>(u => (u.Key == Define.RoleModule || u.Key == Define.RoleElement) && ids.Contains(u.FirstId));
                UnitWork.Delete<Relevance>(u => u.Key == Define.UserRole && ids.Contains(u.SecondId));
                UnitWork.Delete<Role>(u => ids.Contains(u.Id));
                UnitWork.Save();
            });
        }

        /// <summary>
        /// 更新角色属性
        /// </summary>
        /// <param name="obj"></param>
        public void Update(RoleView obj)
        {
            Role role = obj;

            UnitWork.Update<Role>(u => u.Id == obj.Id, u => new Role
            {
                Name = role.Name,
                Status = role.Status
            });

        }


       
    }
}