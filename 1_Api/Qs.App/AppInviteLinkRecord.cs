using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.App
{
    /// <summary>
    /// 应用层
    /// </summary>
    public class AppInviteLinkRecord : AppBaseString<ModelInviteLinkRecord, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppInviteLinkRecord(IUnitWork<QsDBContext> unitWork, IRepository<ModelInviteLinkRecord, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuInviteLinkRecord req)
        {           
            var result = new TableData();         
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResInviteUser> ListByWhere(ReqQuInviteLinkRecord req, bool isPage = false)
        {
            IQueryable<ResInviteUser> linq = ListLinq(req);
            List<ResInviteUser> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            List<ResInviteUser> listVm = new List<ResInviteUser>();
            foreach (var item in list)
            {
                listVm.Add(ResInviteUser.ToView(item));
            }
            return listVm.OrderByDescending(p => p.CreateTime).ToList();
        }
         
        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ResInviteUser> ListLinq(ReqQuInviteLinkRecord req)
        {
            var linq = from record in UnitWork.Find<ModelInviteLinkRecord>(null)
                join user in UnitWork.Find<ModelUser>(null) on record.InviteeUid equals user.Id
                select new ResInviteUser
                {   InviterId=record.InviterId,
                    InviteeUid = record.InviteeUid,
                    InviteeAvater = user.UrlAvater,
                    InviteePhone = user.Phone,
                    InviteeName = user.NickName,
                    InvitationTime = record.InvitationTime,
                    Status = record.Status,
                    OpenType = record.OpenType,
                    IsOpenVip = record.IsOpenVip,
                    CreateTime = record.CreateTime,
                };
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.InviteeName.Contains(req.Key)|| p.InviteePhone.Contains(req.Key));
            }

            if (xConv.ToBool(req.OnlyMy))
            {
                var user = _auth.GetCurrentContext().User;
                linq = linq.Where(p => p.InviterId==user.Id);
            }
            return linq.OrderByDescending(p=>p.CreateTime);
        }

      

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuInviteLinkRecord req)
        {
            var model = xConv.CopyMapper<ModelInviteLinkRecord, ReqAuInviteLinkRecord>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                Repository.Add(model);
            }
            else
            {
                Repository.Update(model);
            }
        }

        /// <summary>
        /// 统计我的邀请
        /// </summary>
        /// <returns></returns>
        public ResInviteLinkRecordStatistics MyInviteLinkRecordStatistics(ReqStatistic req)
        {
            if (req.OnlyMy)
            {
                req.UserId= _auth.GetCurrentContext().User.Id;
            }
            ResInviteLinkRecordStatistics res = new ResInviteLinkRecordStatistics();
            res.InviteTotal = UnitWork.Count<ModelInviteLinkRecord>(p => p.InviterId == req.UserId);
            res.InviteOpenVipTotal = UnitWork.Count<ModelInviteLinkRecord>(p => p.InviterId == req.UserId
                && p.IsOpenVip == (int)xEnum.YesOrNo.Yes);
            return res;
        }

        /// <summary>
        /// 获取最后邀请记录
        /// </summary>
        /// <param name="uid">被邀请人(为空则为当前用户)</param>
        /// <returns></returns>
        public ModelInviteLinkRecord LastInviteRecord(string uid="")
        {
            if (string.IsNullOrEmpty(uid))
            {
                uid= _auth.GetCurrentContext().User.Id;
            }

           var lastInviteRecord=UnitWork.Find<ModelInviteLinkRecord>(p => p.InviteeUid == uid).OrderByDescending(p => p.CreateTime)
                .FirstOrDefault();
           return lastInviteRecord;
        }

    }
}