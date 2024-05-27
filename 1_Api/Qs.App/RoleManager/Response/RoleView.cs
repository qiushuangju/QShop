// ***********************************************************************
// Assembly         : Qs.App
// Author           : Qs
// Created          : 11-29-2015
//
// Last Modified By : Qs
// Last Modified On : 11-29-2015
// ***********************************************************************
// <copyright file="RoleVM.cs" company="www.cnblogs.com/Qs">
//     Copyright (c) www.cnblogs.com/Qs. All rights reserved.
// </copyright>
// <summary>角色模型视图</summary>
// ***********************************************************************

using Qs.Comm;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.App.Response
{
    public partial class RoleView
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
	    /// 角色类型
	    /// </summary>
        public int Type { get; set; }


        /// <summary>
        ///是否属于某用户 
        /// </summary>
        public bool Checked { get; set; }

        public static implicit operator RoleView(Role role)
        {
            return role.MapTo<RoleView>();
        }

        public static implicit operator Role(RoleView rolevm)
        {
            return rolevm.MapTo<Role>();
        }

    }
}
