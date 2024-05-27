using System.Collections;
using TencentCloud.Common;

namespace Qs.Comm
{
    public static class Define
    {
        
        /// <summary>
        /// 订单自动收货天数
        /// </summary>
        public static int DaysReceived = 1; 
        /// <summary>
        /// 订单自动结束天数
        /// </summary>
        public static int DaysDone = 1;
        /// <summary>
        /// 奖励积分比例
        /// </summary>
        public static decimal PointRate = 1M;
        /// <summary>
        /// Token时效天数
        /// </summary>
        public static int TokenDays = 10; //Token时效天数    
        /// <summary>
        /// 配送至少提前小时
        /// </summary>
        public const int PreHour = 4;

        /// <summary>
        /// 用户
        /// </summary>
        public const string AppWxApp = "MP-WEIXIN";
        /// <summary>
        /// 后端-商户管理
        /// </summary>
        public const string AppWebStore = "WebStore";
        /// <summary>
        /// 后端-超管管理
        /// </summary>
        public const string AppWebAdmin = "WebAdmin";
        /// <summary>
        /// 用户角色关联KEY
        /// </summary>
        public static string UserRole = "UserRole";
        /// <summary>
        ///   角色资源关联KEY
        /// </summary>
        public const string RoleResource = "RoleResource";
        /// <summary>
        ///用户机构关联KEY
        /// </summary>
        public const string UserOrg = "UserOrg";

        /// <summary>
        ///  角色菜单关联KEY
        /// </summary>
        public const string RoleElement = "RoleElement";
        /// <summary>
        ///  角色模块关联KEY
        /// </summary>
        public const string RoleModule = "RoleModule";
        /// <summary>
        ///  角色数据字段权限
        /// </summary>
        public const string RoleDataProperty = "RoleDataProperty";
        /// <summary>
        /// 角色-平台管理员
        /// </summary>
        public const string RoleIdSysAdmin = "df5ca6cd70da4747abe3cd3373e866fe";
        /// <summary>
        /// 角色-商城管理员
        /// </summary>
        public const string RoleIdStoreAdmin = "e6889fdc888846e2b85c0b22bee5caef";
        /// <summary>
        ///   sql server
        /// </summary>
        public const string DbSql = "SqlServer";

        /// <summary>
        ///      mysql
        /// </summary>
        public const string DbMySql = "MySql";

        /// <summary>
        ///   oracle
        /// </summary>
        public const string DbOracle = "Oracle";

        /// <summary>
        /// 支付小时
        /// </summary>
        public const int OrderPayHour = 24;    
        /// <summary>
        /// Token名称
        /// </summary>
        public const string TokenName = "X-Token";

        /// <summary>
        /// 租户Id
        /// </summary>
        public const string TenantId = "tenantId";

        /// <summary>
        ///  超管账号
        /// </summary>
        public const string SystemUserName = "system";

        /// <summary>
        ///   超管密码
        /// </summary>
        public const string SystemUserPwd = "systemS=1";

        ///// <summary>
        ///// 数据权限配置中，当前登录用户的key
        ///// </summary>
        //public const string DataPrivilegeLoginUser = "{loginUser}";

        ///// <summary>
        /////    数据权限配置中，当前登录用户角色的key
        ///// </summary>
        //public const string DataPrivilegeLoginRole = "{loginRole}";

        ///// <summary>
        /////   数据权限配置中，当前登录用户部门的key
        ///// </summary>
        //public const string DataPrivilegeLoginOrg = "{loginOrg}";

        /// <summary>
        /// 任务
        /// </summary>
        public const string JobMapKey = "OpenJob";

        /// <summary>
        /// 版本
        /// </summary>
        // public static string Version = "Dev";
        public static string Version = "Pro";

        /// <summary>
        /// Im AdminUserId
        /// </summary>
        public static string ImAdminUserId = "admin";

        /// <summary>
        /// Im AppId
        /// </summary>
        public static int ImSdkAppId = 1400570989;

        /// <summary>
        /// Im Key
        /// </summary>
        public static string ImSdkKey = "dfca53869c60a03ef6a3f68496996ac68d28a814ffb11dd69ba963c3925dd55b";

        /// <summary>
        /// Im服务域名
        /// </summary>
        public static string ImUrl = "http://119.91.147.137";


        /// <summary>                                                                                                          
        /// 腾讯云 秘钥
        /// </summary>
        public static Credential TengXunCred = new Credential
        {
            SecretId = "xxxxxx",
            SecretKey = "xxxxxx"
        };

        /// <summary>
        /// 升级代理商所需下级经销商数
        /// </summary>
        public static int UpWbMaster = 50;
        /// <summary>
        /// 升级经销商所需下级唯粉数
        /// </summary>
        public static int UpWbFamily = 10;

        public static string UrlOrderNotify = "http://apidemo.qshopcn.com/api/WpsNotify/OrderNotify";

        /// <summary>
        /// 是否开发版本
        /// </summary>
        public static bool IsDevVersion()
        {
            if (!Version.Contains("Dev"))
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// 本项目网址
        /// </summary>
        public static string UrlApi = "https://apidemo.qshopcn.com/";

        /// <summary>
        /// 当前网址
        /// </summary>
        public static string HttpBaseApi()
        {
            string url = UrlApi;
            if (IsDevVersion())
            {
                url = "http://localhost:5000";
            }

            return url;
        }

    }

    
}
