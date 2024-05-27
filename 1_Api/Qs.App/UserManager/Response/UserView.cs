using System;
using Qs.Comm;
using Qs.Comm;
using Qs.Repository.Domain;
using Qs.Repository.Response;

namespace Qs.App.Response
{
    public  class UserView
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }


        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string Account { get; set; }


        /// <summary>
        /// 名字/昵称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        public string Phone { get; set; }
        /// <summary>
        ///头像
        /// </summary>
        public string UrlAvater { get; set; }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public int Sex { get; set; }

        /// <summary>
        /// 商城
        /// </summary>
        public string StoreId { get; set; }
        /// <summary>
        /// 商城
        /// </summary>
        public string StoreName{ get; set; }

        /// <summary>
        /// 来源上下级
        /// </summary>
        public string SourceUserId { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        /// <returns></returns>
        public int? Status { get; set; }

        public string StrStatus { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        /// <returns></returns>
        public int? UserType { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public string StrUserType { get; set; }
        /// <summary>
        /// 组织类型
        /// </summary>
        /// <returns></returns>
        public int Type { get; set; }

        /// <summary>
        /// 用户可用余额
        /// </summary>
        public decimal Balance { get; set; }

        /// <summary>
        /// 用户可用积分
        /// </summary>
        public decimal Points { get; set; }

        /// <summary>
        /// 实际消费的金额(不含退款)
        /// </summary>
        public decimal ExpendMoney { get; set; }
        /// <summary>
        ///真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 开户银行
        /// </summary>
        public string BankName { get; set; }
        /// <summary>
        /// 开户支行
        /// </summary>
        public string RegBank { get; set; }
        /// <summary>
        /// 银行卡号
        /// </summary>
        public string BankCardNo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTime { get; set; }


        /// <summary>
        /// 创建人名字
        /// </summary>
        /// <value>The create user.</value>
        public string CreateUser { get; set; }

        /// <summary>
        /// 用户订单数据汇总
        /// </summary>
        public UserDataAggregation DataAggregation { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 角色Ids
        /// </summary>
        public string RoleIds { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleNames { get; set; }
      

        /// <summary>
        /// 团队总人数
        /// </summary>
        public int TeamUserTotal { get; set; }

        /// <summary>
        /// 我的销售额
        /// </summary>
        public decimal MySaleTotalMoney { get; set; }

        /// <summary>
        /// 团队销售额
        /// </summary>
        public decimal TeamSaleTotalMoney { get; set; }

        public static implicit operator UserView(ModelUser user)
        {
            return user.MapTo<UserView>();
        }

        public static implicit operator ModelUser(UserView view)
        {
            return view.MapTo<ModelUser>();
        }

        public UserView()
        {
            
            CreateUser = string.Empty;
            DataAggregation = new UserDataAggregation();
        }
    }
    /// <summary>
    /// 旗下用户数据汇总
    /// </summary>
    public class UserDataAggregation
    {
        /// <summary>
        /// 旗下用户总数
        /// </summary>
        public int ChildUsersTotal { get; set; }

        /// <summary>
        /// 旗下用户订单数
        /// </summary>
        public int OrderTotal { get; set; }

        /// <summary>
        /// 订单金额汇总
        /// </summary>
        public decimal OrderMoneyTotal { get; set; }
    }
}
