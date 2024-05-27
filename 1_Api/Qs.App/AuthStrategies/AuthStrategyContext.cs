// ***********************************************************************
// <summary>
// 授权策略上下文，一个典型的策略模式
// 根据用户账号的不同，采用不同的授权模式，以后可以扩展更多的授权方式
// </summary>
// ***********************************************************************

using System.Collections.Generic;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.App.AuthStrategies
{
    /// <summary>
    ///  授权策略上下文，一个典型的策略模式
    /// </summary>
    public class AuthStrategyContext
    {
        private readonly IAuthStrategy _strategy;
        public AuthStrategyContext(IAuthStrategy strategy)
        {
            this._strategy = strategy;
        }

        public ModelUser User
        {
            get { return _strategy.User; }
        }

        public List<ModuleView> Modules
        {
            get { return _strategy.Modules; }
        }

        public List<ModuleElement> ModuleElements
        {
            get { return _strategy.ModuleBtns; }
        }

        public List<Role> Roles
        {
            get { return _strategy.Roles; }
        }

    }

}
