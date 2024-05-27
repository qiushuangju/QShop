using System;
using System.Collections.Generic;
using Qs.Comm;

namespace Qs.App.UserManager.Request
{
    public class ChangeProfileReq
    {
        /// <summary>
        /// 头像
        /// </summary>
        public string UrlAvater { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }   

        public void Check()
        {
            if (string.IsNullOrEmpty(UrlAvater)||string.IsNullOrEmpty(Name))
            {
                throw new Exception("参数不能为空!");
            }
        }

    }

    public class ReqChangeName
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 验证
        /// </summary>
        public void Check()
        {
           xValidation.CheckStrNull(new List<ValueTip>()
           {
               new ValueTip(Name,"姓名")
           });
        }

    }
}
