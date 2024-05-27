using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Qs.App.ApiExpress;
using Qs.App.ApiKuaiDiNiao.Res;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Base;
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
    public class AppExpress : AppBaseString<ModelExpressCodeKuaiDi100, QsDBContext>
    {
    
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppExpress(IUnitWork<QsDBContext> unitWork, IRepository<ModelExpressCodeKuaiDi100, QsDBContext> repository,
            DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuExpress req)
        {
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
            result.Result = ListByWhere(req, true);
            result.Count = ListLinq(req).Count();
            return result;
        }

        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelExpressCodeKuaiDi100> ListByWhere(ReqQuExpress req, bool isPage = false)
        {
            IQueryable<ModelExpressCodeKuaiDi100> linq = ListLinq(req);
            List<ModelExpressCodeKuaiDi100> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.ToList();
        }
        

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelExpressCodeKuaiDi100> ListLinq(ReqQuExpress req)
        {
            var linq = UnitWork.Find<ModelExpressCodeKuaiDi100>(p => true);

            if (xConv.ToInt(req.Type) != 0)
            {
                linq = linq.Where(p => p.Type == xConv.ToInt(req.Type));
            }
            return linq;
        }


        /// <summary>
        ///获取快递轨迹
        /// </summary>
        /// <param name="orderSkuId">订单SkuId</param>
        /// <returns></returns>
        public ResListTrack ListTrack(string orderSkuId)
        {
            var orderSku = UnitWork.FirstOrDefault<ModelOrderSku>(p => p.Id == orderSkuId);
            ResListTrack res = new ResListTrack();
            if (orderSku.OrderStatus>=(int)xEnum.OrderStatus.WaitReceiving)
            {
                var orderAddress = UnitWork.FirstOrDefault<ModelOrderAddress>(p => p.OrderId == orderSku.OrderId);
                IApiExpress apiExpress = FactoryExpress.CreateExpress(xEnum.ExpressName.Kd100);
                var code = GetComCode(xEnum.ExpressName.Kd100, orderSku.ExpressCompany);
                List<TrackInfo> list = apiExpress.GetTrack(code, orderSku.ExpressNo, orderAddress.Phone);   //快递100

                res.ExpressName = orderSku.ExpressCompany;
                res.ExpressNo = orderSku.ExpressNo;
                res.ListInfo = list;

            }
            

            return res;

        }

        /// <summary>
        ///获取快递最后轨迹
        /// </summary>
        /// <param name="orderId">订单Id</param>
        /// <returns></returns>
        public TrackInfo GetLasetTrack(string orderId)
        {
            TrackInfo laset = new TrackInfo() {AcceptStation = "暂无轨迹信息", AcceptTime = DateTime.Now};

            var orderSku = UnitWork.FirstOrDefault<ModelOrder>(p => p.Id == orderId);
            if (orderSku.OrderStatus >= (int) xEnum.OrderStatus.WaitReceiving)
            {

                var orderAddress = UnitWork.FirstOrDefault<ModelOrderAddress>(p => p.OrderId == orderId);
                IApiExpress apiExpress = FactoryExpress.CreateExpress(xEnum.ExpressName.Kd100);
                var code = GetComCode(xEnum.ExpressName.Kd100, orderSku.ExpressCompany);
                List<TrackInfo> list = apiExpress.GetTrack(code, orderSku.ExpressNo, orderAddress.Phone); //快递100
                // yuantong
                if (list.Count > 0)
                {
                    laset = list.FirstOrDefault();
                }
            }
            return laset;
        }

        /// <summary>
        /// 根据快递公司名称获取编码
        /// </summary>
        /// <param name="express"></param>
        /// <param name="comName"></param>
        /// <returns></returns>
        public string GetComCode(xEnum.ExpressName express,string comName)
        {
            string code = "";
            if (express== xEnum.ExpressName.Kd100)
            {
                code = UnitWork.FirstOrDefault<ModelExpressCodeKuaiDi100>(p => p.ExpressName == comName).ExpressCode;
            }
            if (express == xEnum.ExpressName.Kdn)
            {
                code = UnitWork.FirstOrDefault<ModelExpressCodeKuaiDiNiao>(p => p.ExpressName == comName).ExpressCode;
            }

            return code;
        }

    }
}