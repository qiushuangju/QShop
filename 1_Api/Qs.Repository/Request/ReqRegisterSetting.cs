using Qs.Repository.Vm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qs.Repository.Request
{
   public class ReqRegisterSetting
    {
        public string Key { get; set; }

        public VmSettingRegister Data { get; set; }
    }
}
