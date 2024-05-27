using System;
using System.Collections.Generic;
using System.Linq;
using Qs.Comm;
using Microsoft.Extensions.Logging;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App
{
    public class AppRevelanceManager : AppBaseString<Relevance,QsDBContext>
    {
        private readonly ILogger<AppRevelanceManager> _logger;
        public AppRevelanceManager(IUnitWork<QsDBContext> unitWork, IRepository<Relevance,QsDBContext> repository, ILogger<AppRevelanceManager> logger, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _logger = logger;
        }

        /// <summary>
        /// 添加关联
        /// <para>比如给用户分配资源，那么firstId就是用户ID，secIds就是资源ID列表</para>
        /// </summary>
        /// <param name="type">关联的类型，如Define.USERRESOURCE</param>
        public void Assign(AssignReq req)
        {

            UnAssign(req);
            List<Relevance> list = (from sameVals in req.secIds           
                select new Relevance
                {
                    Key = req.type,
                    FirstId = req.firstId,
                    SecondId = sameVals,
                    OperateTime = DateTime.Now
                }).ToList();
            UnitWork.BatchAdd(list);
            UnitWork.Save();

        }

     

        /// <summary>
        /// 取消关联
        /// </summary>
        /// <param name="type">关联的类型，如Define.USERRESOURCE</param>
        /// <param name="firstId">The first identifier.</param>
        /// <param name="secIds">The sec ids.</param>
        public void UnAssign(AssignReq req)
        {
            if (req.secIds == null || req.secIds.Length == 0)
            {
                DeleteBy(req.type, req.firstId);
            }
            else
            {
                DeleteBy(req.type, req.secIds.ToLookup(u => req.firstId));
            }
        }

        /// <summary>
        /// 删除关联
        /// </summary>
        /// <param name="key">关联标识</param>
        /// <param name="idMaps">关联的&lt;firstId, secondId&gt;数组</param>
        private void DeleteBy(string key, ILookup<string, string> idMaps)
        {
            foreach (var sameVals in idMaps)
            {
                foreach (var value in sameVals)
                {                                                                                      
                    try
                    {
                        UnitWork.Delete<Relevance>(u => u.Key == key && u.FirstId == sameVals.Key && u.SecondId == value);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e,e.Message);
                    }                                                                      
                }
            }
        }

        public void DeleteBy(string key, params string[] firstIds)
        {
            UnitWork.Delete<Relevance>(u => firstIds.Contains(u.FirstId) && u.Key == key);
        }


        /// <summary>
        /// 根据关联表的一个键获取另外键的值
        /// </summary>
        /// <param name="key">映射标识</param>
        /// <param name="returnSecondIds">返回的是否为映射表的第二列，如果不是则返回第一列</param>
        /// <param name="ids">已知的ID列表</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> Get(string key, bool returnSecondIds, params string[] ids)
        {
            if (returnSecondIds)
            {
                return Repository.Find(u => u.Key == key
                                            && ids.Contains(u.FirstId)).Select(u => u.SecondId).ToList();
            }
            else
            {
                return Repository.Find(u => u.Key == key
                                            && ids.Contains(u.SecondId)).Select(u => u.FirstId).ToList();
            }
        }

        /// <summary>
        /// 根据key ,firstId,secondId获取thirdId
        /// </summary>
        /// <param name="key"></param>
        /// <param name="firstId"></param>
        /// <param name="secondId"></param>
        /// <returns></returns>
        public List<string> Get(string key, string firstId, string secondId)
        {
            return Repository.Find(u => u.Key == key && u.FirstId == firstId && u.SecondId == secondId)
                .Select(u => u.ThirdId).ToList();
        }

        /// <summary>
        /// 分配数据字段权限
        /// </summary>
        /// <param name="request"></param>
        public void AssignData(AssignDataReq request)
        {
            if (!request.Properties.Any())
            {
                return;
            }

            var relevances = new List<Relevance>();
            foreach (var requestProperty in request.Properties)
            {
                relevances.Add(new Relevance
                {
                    Key = Define.RoleDataProperty,
                    FirstId = request.RoleId,
                    SecondId = request.ModuleCode,
                    ThirdId = requestProperty,
                    OperateTime = DateTime.Now
                });
            }

            UnitWork.BatchAdd(relevances.ToList());
            UnitWork.Save();
        }

        /// <summary>
        /// 取消数据字段分配
        /// </summary>
        /// <param name="request"></param>
        public void UnAssignData(AssignDataReq request)
        {
            if (request.Properties == null || request.Properties.Length == 0)
            {
                if (string.IsNullOrEmpty(request.ModuleCode)) //模块为空，直接把角色的所有授权删除
                {
                    DeleteBy(Define.RoleDataProperty, request.RoleId);
                }
                else //把角色的某一个模块权限全部删除
                {
                    DeleteBy(Define.RoleDataProperty, new[] {request.ModuleCode}.ToLookup(u => request.RoleId));
                }
            }
            else //按具体的id删除
            {
                foreach (var property in request.Properties)
                {
                    UnitWork.Delete<Relevance>(u => u.Key == Define.RoleDataProperty
                                                    && u.FirstId == request.RoleId
                                                    && u.SecondId == request.ModuleCode
                                                    && u.ThirdId == property);
                }
            }
        }

        /// <summary>
        /// 为角色分配用户，需要统一提交，会删除以前该角色的所有用户
        /// </summary>
        /// <param name="request"></param>
        public void AssignRoleUsers(AssignRoleUsers request)
        {
            UnitWork.ExecuteWithTransaction(() =>
            {
                //删除以前的所有用户
                UnitWork.Delete<Relevance>(u => u.SecondId == request.RoleId && u.Key == Define.UserRole);
                //批量分配用户角色
                UnitWork.BatchAdd((from firstId in request.UserIds
                    select new Relevance
                    {
                        Key = Define.UserRole,
                        FirstId = firstId,
                        SecondId = request.RoleId,
                        OperateTime = DateTime.Now
                    }).ToList());
                UnitWork.Save();
            });
        }

    }
}