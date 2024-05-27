// ***********************************************************************
// Assembly         : Qs.App
// Author           : Qs
// Created          : 07-05-2018
//
// Last Modified By : Qs
// Last Modified On : 07-05-2018
// ***********************************************************************
// <copyright file="AuthContextFactory.cs" company="Qs.App">
//     Copyright (c) http://www.Qs.net.cn. All rights reserved.
// </copyright>
// <summary>
// 用户权限策略工厂
//</summary>
// ***********************************************************************

using Qs.App.AuthStrategies;
using Qs.Comm;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;

namespace Qs.App
{
    /// <summary>
    ///  加载用户所有可访问的资源/机构/模块
    /// <para>Qs新增于2021-07-19 10:53:30</para>
    /// </summary>
    public class AuthContextFactory
    {
        private SystemAuthStrategy _systemAuth;
        private NormalAuthStrategy _normalAuthStrategy;
        private readonly IUnitWork<QsDBContext> _unitWork;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="sysStrategy"></param>
        /// <param name="normalAuthStrategy"></param>
        /// <param name="unitWork"></param>
        public AuthContextFactory(SystemAuthStrategy sysStrategy
            , NormalAuthStrategy normalAuthStrategy
            , IUnitWork<QsDBContext> unitWork)
        {
            _systemAuth = sysStrategy;
            _normalAuthStrategy = normalAuthStrategy;
            _unitWork = unitWork;
        }

        /// <summary>
        /// 获取登录上下文
        /// </summary>
        /// <param name="uersNameOrPhone"></param>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public AuthStrategyContext GetAuthStrategyContext(string uersNameOrPhone,string appKey)
        {
            if (string.IsNullOrEmpty(uersNameOrPhone)) return null;

            IAuthStrategy service = null;
             if (uersNameOrPhone == Define.SystemUserName)
            {
                service= _systemAuth;
            }
            else
            {
                
                service = _normalAuthStrategy;
                if (appKey==Define.AppWebStore)//如管理端则只查询管理用户
                {
                    service.User = _unitWork.FirstOrDefault<ModelUser>(u => (u.Account == uersNameOrPhone || u.Phone == uersNameOrPhone)&&u.UserType<=(int)xEnum.UserType.SysAdmin);
                }
                else //其他用户
                {
                    service.User = _unitWork.FirstOrDefault<ModelUser>(u => (u.Account == uersNameOrPhone || u.Phone == uersNameOrPhone) && u.UserType >= (int)xEnum.UserType.Customer);
                }
               
            }

         return new AuthStrategyContext(service);
        }
    }
}