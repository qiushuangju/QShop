using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppComm : AppBaseString<ModelGoods, QsDBContext>
    {
        private AppOrder _appOrder;
        private AppCategory _appDic;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppComm(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoods, QsDBContext> repository,
            DbExtension dbExtension, AppOrder appOrder, AppCategory appDic, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _appOrder = appOrder;
            _appDic = appDic;
        }


        /// <summary>
        /// 获取用户首页数据
        /// </summary>
        public ResHomeDataUser GetHomeDataUser()
        {
            var user = _auth.GetCurrentContext().User;

            //销售订单
            List<ModelOrder> listOrder = UnitWork.Find<ModelOrder>(p => p.UserId == (user.Id)).ToList();
            var appDic = _appDic.GetDicByCode("OfficialPhone");
            ResHomeDataUser res = new ResHomeDataUser()
            {
                //销售统计
                CountNotPaid =
                    listOrder.Count(_appOrder.GetWhereByBigStatus((int) xEnum.OrderBigStatus.NotPaid).Compile()),
                CountWaitDeliver =
                    listOrder.Count(_appOrder.GetWhereByBigStatus((int) xEnum.OrderBigStatus.WaitDeliver).Compile()),
                CountWaitReceiving =
                    listOrder.Count(_appOrder.GetWhereByBigStatus((int) xEnum.OrderBigStatus.WaitReceiving).Compile()),
                CountReceived =
                    listOrder.Count(_appOrder.GetWhereByBigStatus((int) xEnum.OrderBigStatus.WaitComment).Compile()),                                     
                CountAfterSale =
                    listOrder.Count(_appOrder.GetWhereByBigStatus((int) xEnum.OrderBigStatus.Refund).Compile()),
                OfficialPhone = appDic.DtValue
            };
            return res;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public ResPcHomeData GetPcHomeData(DateTime startDate)
        {
            var loginContext = _auth.GetCurrentContext();

            ResPcHomeData res = new ResPcHomeData()
            {
                CountUser = UnitWork.Count<ModelUser>(p =>
                    p.UserType <= (int) xEnum.UserType.Customer && p.CreateTime >= startDate),
                CountAmount = 0,
                CountOrder = 0

            };
            return res;
        }

    }
}