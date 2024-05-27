using System;
using System.Linq;
using Qs.Comm;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qs.Comm.Utilities;
using Qs.Repository.Domain;

namespace Qs.Repository
{

    public partial class QsDBContext : DbContext
    {

        private ILoggerFactory _LoggerFactory;
        private IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        private IOptions<AppSetting> _appConfiguration;

        public QsDBContext(DbContextOptions<QsDBContext> options, ILoggerFactory loggerFactory,
            IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IOptions<AppSetting> appConfiguration)
            : base(options)
        {
            _LoggerFactory = loggerFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _appConfiguration = appConfiguration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true); //允许打印参数
            optionsBuilder.UseLoggerFactory(_LoggerFactory);
            InitTenant(optionsBuilder);
            base.OnConfiguring(optionsBuilder);
        }

        //初始化多租户信息，根据租户id调整数据库
        private void InitTenant(DbContextOptionsBuilder optionsBuilder)
        {

            var tenantId = _httpContextAccessor.GetTenantId();
            string connect = _configuration.GetConnectionString(tenantId);
            if (string.IsNullOrEmpty(connect))
            {
                throw new Exception($"未能找到租户{tenantId}对应的连接字符串信息");
            }

            //这个地方如果用IOption，在单元测试的时候会获取不到AppSetting的值😅
            var dbtypes = _configuration.GetSection("AppSetting:DbTypes").GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);

            var dbType = dbtypes[tenantId];
            if (dbType == Define.DbSql)
            {
                optionsBuilder.UseSqlServer(connect);
            }
            else if (dbType == Define.DbMySql) //mysql
            {
                optionsBuilder.UseMySql(connect, new MySqlServerVersion(new Version()));
            }
            else
            {
                //optionsBuilder.UseOracle(connect, options => options.UseOracleSQLCompatibility("11"));
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<DataPrivilegeRule>()
            //     .HasKey(c => new { c.Id });
        }
        public virtual DbSet<ModelOrderRefundSku> OrderRefundSkus { get; set; }
        public virtual DbSet<ModelSysSetting> SysSettings { get; set; }
        public virtual DbSet<ModelRechargeOrderPlan> RechargeOrderPlans { get; set; }
        public virtual DbSet<ModelRechargePlan> RechargePlans { get; set; }
        public virtual DbSet<ModelRechargeOrder> RechargeOrders { get; set; }
        public virtual DbSet<ModelUserBalanceLog> UserBalanceLogs { get; set; }
        public virtual DbSet<ModelInvitePoster> InvitePosters { get; set; }
        public virtual DbSet<ModelInviteLinkRecord> InviteLinkRecords { get; set; }
        public virtual DbSet<ModelInviteLink> InviteLinks { get; set; }
        public virtual DbSet<ModelOrder> Orders { get; set; }
        public virtual DbSet<ModelOrderSku> OrderSkus { get; set; }
        public virtual DbSet<ModelOrderAddress> OrderAddresses { get; set; }
        public virtual DbSet<ModelOrderTrack> OrderTracks { get; set; }
        public virtual DbSet<ModelUserAddress> UserAddresses { get; set; }
        public virtual DbSet<ModelCart> Carts { get; set; }
        public virtual DbSet<ModelCoupon> Coupons { get; set; }
        public virtual DbSet<ModelUserCoupon> UserCoupons { get; set; }
        public virtual DbSet<ModelGoodsSpec> GoodsSpecs { get; set; }
        public virtual DbSet<ModelGoodsSpecValue> GoodsSpecValues { get; set; }
        public virtual DbSet<ModelUserDrawMoneyLog> UserDrawMoneyLog { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryType> CategoryTypes { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleElement> ModuleElements { get; set; }
        public virtual DbSet<Relevance> Relevances { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ModelUser> Users { get; set; }
        public virtual DbSet<ModelGoodsCate> GoodsCates { get; set; }
        public virtual DbSet<ModelGoods> Goods { get; set; }
        public virtual DbSet<ModelGoodsService> GoodsServices { get; set; }
        public virtual DbSet<ModelGoodsComment> GoodsComments { get; set; }
        public virtual DbSet<ModelStorePage> StorePage { get; set; }
        public virtual DbSet<OpenJob> OpenJobs { get; set; }
        public virtual DbSet<ModelFileGroup> FileGroups { get; set; }
        public virtual DbSet<ModelArticleCategory> ArticleCategories { get; set; }
        public virtual DbSet<ModelArticle> Articles { get; set; }
        public virtual DbSet<ModelFileUpload> FileUploads { get; set; }
        public virtual DbSet<ModelGoodsSku> GoodsSkus { get; set; }
        public virtual DbSet<ModelSysArea> SysAreas { get; set; }
        public virtual DbSet<ModelRefundReason> RefundReasons { get; set; }
        public virtual DbSet<ModelStoreSetting> StoreSetting { get; set; }
        public virtual DbSet<ModelWpsNotify> WpsNotifies { get; set; }
        public virtual DbSet<ModelStore> Stores { get; set; }
        public virtual DbSet<ModelExpressCodeKuaiDi100> ExpressCodeKuaiDi100s { get; set; }
        public virtual DbSet<ModelExpressCodeKuaiDiNiao> ExpressCodeKuaiDiNiaos { get; set; }
        public virtual DbSet<ModelDelivery> Delivery { get; set; }
        public virtual DbSet<ModelDeliveryRule> DeliveryRule { get; set; }
        public virtual DbSet<ModelUserPointsLog> UserPointsLogs { get; set; }
        public virtual DbSet<ModelStoreAddress> StoreAddresses { get; set; }
        public virtual DbSet<ModelStoreExpress> StoreExpresses { get; set; }
        public virtual DbSet<ModelOrderRefundAddress> OrderRefundAddresses { get; set; }
        public virtual DbSet<ModelStoreSettingPay> StoreSettingPays { get; set; }

        //非数据库表格


        //public virtual DbQuery<VmStatisticOperate> StatisticOperate { get; set; }
        //public virtual DbQuery<VmStatisticOpUserOrderData> StatisticOpUserOrderData { get; set; }
        //public virtual DbQuery<SysTableColumn> SysTableColumns { get; set; }
        //public virtual DbQuery<VmStoreStorage> ResStoreStorage { get; set; }

        //public virtual DbQuery<ResInviteStatistics> ResInviteStatistics { get; set; }
        //// public virtual DbQuery<VmStatisticsOrder> VmStatisticsOrder { get; set; }

        //public virtual DbQuery<VmStorageOrderSummary> VmStorageOrderSummary { get; set; }

        //public virtual DbQuery<VmStatisticOrderByStatus> VmStatisticOrderByStatus { get; set; }

        //public virtual DbQuery<VmStatisticOrderServiceByStatus> VmStatisticOrderServiceByStatus { get; set; }

        //public virtual DbQuery<VmStatisticUserByDate> VmStatisticUserByDate { get; set; }
        //public virtual DbQuery<VmStatisticUserVipByDate> VmStatisticUserVipByDate { get; set; }
        //public virtual DbQuery<VmStatisticOrderGoodsNumByDate> VmStatisticOrderGoodsNumByDate { get; set; }

        //public virtual DbQuery<VmStatisticOrderByDate> VmStatisticOrderNumByDate { get; set; }

        //public virtual DbQuery<VmOrderFreightPriceByDate> VmOrderFreightPriceByDate { get; set; }

        //public virtual DbQuery<VmOrderFreightPriceByDriver> VmOrderFreightPriceByDriver { get; set; }

        //public virtual DbQuery<VmStatisticOrderByMonth> VmStatisticOrderNumByMonth { get; set; }

        //public virtual DbQuery<VmStatisticOrderByYear> VmStatisticOrderByYear { get; set; }

        //public virtual DbQuery<VmOrderPriceByBusiness> VmOrderPriceByBusiness { get; set; }
        //public virtual DbQuery<VmStatisticUserIncomeByDate> VmStatisticUserIncomeByDate { get; set; }
        //public virtual DbQuery<VmDistanceKm> VmDistanceKm { get; set; }

        //public virtual DbQuery<VmOrderFreightTotal> VmOrderFreightTotal { get; set; }





    }
}
