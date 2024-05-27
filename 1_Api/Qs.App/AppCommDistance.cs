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
    public class AppCommDistance : AppBaseString<ModelGoods, QsDBContext>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppCommDistance(IUnitWork<QsDBContext> unitWork, IRepository<ModelGoods, QsDBContext> repository,
            DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            
        }



        /// <summary>
        /// 计算距离
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="endLocation"></param>
        /// <returns></returns>
        public decimal CalcDistance(VmLocation startLocation, VmLocation endLocation)
        {
            decimal distance = 0m;
            string sql =
                $@"SELECT dbo.GetDistance({startLocation.Longitude},{startLocation.Latitude},{endLocation.Longitude},{endLocation.Latitude}) Distance";

            //var list = UnitWork.Query<VmDistanceKm>(sql).ToList();
            //distance = xConv.ToDecimal(list.FirstOrDefault().Distance);
            return distance;
        }
    }
}