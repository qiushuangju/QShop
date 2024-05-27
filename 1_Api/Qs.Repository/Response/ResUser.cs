using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qs.Comm;
using Qs.Repository.Domain;

namespace Qs.Repository.Response
{
    /// <summary>
    /// 用户展示对外
    /// </summary>
    public class ResUser
    {

        /// <summary>
        /// 账号
        /// </summary>
        [Description("账号")]
        public string Account { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        [Description("微信OpenId")]
        [Browsable(false)]
        public string WxOpenId { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Description("手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 用户类型 (10:商城管理员 20:平台管理员 100:用户 150:分销代理)
        /// </summary>
        [Description("用户类型 (10:商城管理员 20:平台管理员 100:用户 150:分销代理)")]
        public int? UserType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [Description("状态")]
        public int Status { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Description("真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Description("昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Description("登录账号")]
        public string Password { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Description("用户头像")]
        public string UrlAvater { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [Description("Token")]
        public string Token { get; set; }

        /// <summary>
        /// TokenTime
        /// </summary>
        [Description("TokenTime")]
        public System.DateTime? TokenTime { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Description("性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        [Description("余额")]
        public decimal Balance { get; set; }

        /// <summary>
        /// 余额密码
        /// </summary>
        [Description("余额密码")]
        public string BalancePwd { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        [Description("积分")]
        public decimal Points { get; set; }

        /// <summary>
        /// 用户总支付的金额
        /// </summary>
        [Description("用户总支付的金额")]
        public decimal PayMoney { get; set; }

        /// <summary>
        /// 实际消费的金额(不含退款)
        /// </summary>
        [Description("实际消费的金额(不含退款)")]
        public decimal ExpendMoney { get; set; }

        /// <summary>
        /// 推广收入(分销)
        /// </summary>
        [Description("推广收入(分销)")]
        public decimal IncomeMoney { get; set; }

        /// <summary>
        /// 用户等级
        /// </summary>
        [Description("用户等级")]
        [Browsable(false)]
        public string GradeId { get; set; }

        /// <summary>
        /// 注册来源
        /// </summary>
        [Description("注册来源")]
        public string Platform { get; set; }

        /// <summary>
        /// 微信二维码
        /// </summary>
        [Description("微信二维码")]
        [Browsable(false)]
        public string ImgIdWxQrCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string Province { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("")]
        public string City { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Description("最后登录时间")]
        public System.DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 父节点Id
        /// </summary>
        [Description("父节点Id")]
        [Browsable(false)]
        public string ParentId { get; set; }

        /// <summary>
        /// 来源Id(上下级,隔开)
        /// </summary>
        [Description("来源Id(上下级,隔开)")]
        [Browsable(false)]
        public string SourceUserId { get; set; }

        /// <summary>
        /// 店铺Id
        /// </summary>
        [Description("店铺Id")]
        [Browsable(false)]
        public string StoreId { get; set; }

        /// <summary>
        /// 优惠券数量
        /// </summary>
        public int CountCoupon { get; set; }

        /// <summary>
        /// 是否已绑定手机
        /// </summary>
        public bool IsBindPhone { get; set; }

        /// <summary>
        /// 是否已设置支付密码
        /// </summary>
        public bool IsHavePayPwd { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string StrUserType { get; set; }
        /// <summary>
        ///状态
        /// </summary>
        public string StrStatus { get; set; }

        /// <summary>
        /// 直接上级
        /// </summary>
        [Description("直接上级")]
        public ModelUser Parent { get; set; }

        /// <summary>
        /// 间接上级
        /// </summary>
        [Description("间接上级")]
        public ModelUser GrandParent { get; set; }


        public static ResUser ToView(ModelUser user,ModelFileUpload file)
        {
            ResUser vm = xConv.CopyMapper<ResUser, ModelUser>(user);
            vm.IsBindPhone = !string.IsNullOrEmpty(user.Phone);
            vm.IsHavePayPwd = !string.IsNullOrEmpty(user.BalancePwd);
           
            return vm;
        }
    }
}
