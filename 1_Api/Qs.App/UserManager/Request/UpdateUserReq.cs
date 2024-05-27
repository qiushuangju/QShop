using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Qs.Comm;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.App.Request
{
    /// <summary>
    /// 添加或修改用户信息的请求
    /// </summary>
    public  class UpdateUserReq
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage = "账号不能为空~~")]
        public string Account { get; set; }
        public string WxOpenId { get; set; }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BalancePwd { get; set; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string Phone { get; set; }

        /// <summary>
        /// 是否内部业务员
        /// </summary>
        public bool IsInsideBusiness { get; set; }


        /// <summary>
        /// 用户姓名
        /// </summary>
        /// <returns></returns>
        [Required(ErrorMessage="姓名不能为空")]
        public string Name { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        /// <returns></returns>
        public int? UserType { get; set; }
     
        /// <summary>
        /// 
        /// </summary>
        public int Sex { get; set; }


        /// <summary>
        /// 当前状态
        /// </summary>
        /// <returns></returns>
        public int Status { get; set; }
        public void Check()
        {
            xValidation.CheckStrNull(new List<ValueTip>()
            {
                new ValueTip(Phone,"手机号不能为空")
            });
        }
        /// <summary>
        /// 所属组织Id，多个可用，分隔
        /// </summary>
        /// <value>The organizations.</value>
        [Required(ErrorMessage = "请为用户分配机构")]
        public string OrganizationIds { get; set; }

        public static implicit operator UpdateUserReq(ModelUser user)
        {
            return user.MapTo<UpdateUserReq>();
        }

        public static implicit operator ModelUser(UpdateUserReq view)
        {
            return view.MapTo<ModelUser>();
        }

        public UpdateUserReq()
        {
            OrganizationIds = string.Empty;
        }
    }
}
