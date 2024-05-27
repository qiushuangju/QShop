// ***********************************************************************
// </copyright>
// <summary>
// 授权策略接口
// </summary>
// ***********************************************************************


using System.Collections.Generic;
using Qs.Comm;
using Qs.App.Response;
using Qs.Repository.Domain;

namespace Qs.App
{
    public interface IAuthStrategy 
    {
         List<ModuleView> Modules { get; }

        List<ModuleElement> ModuleBtns { get; }

        List<Role> Roles { get; }

        ModelUser User
        {
            get;set;
        }
    }
}