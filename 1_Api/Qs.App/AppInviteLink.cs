using System;
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
    public class AppInviteLink : AppBaseString<ModelInviteLink, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppInviteLink(IUnitWork<QsDBContext> unitWork, IRepository<ModelInviteLink, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuInviteLink req)
        {           
            //var loginContext = _auth.GetCurrentUser();
            var result = new TableData();
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelInviteLink> ListByWhere(ReqQuInviteLink req, bool isPage = false)
        {
            IQueryable<ModelInviteLink> linq = ListLinq(req);
            List<ModelInviteLink> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelInviteLink> ListLinq(ReqQuInviteLink req)
        {
            var linq = UnitWork.Find<ModelInviteLink>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.InviterName.Contains(req.Key));
            }
            return linq;
        }

        /// <summary>
        /// 接受邀请
        /// </summary>
        /// <param name="link">邀请人</param>
        /// <param name="user">被邀请人</param>
        public void AcceptInvite(ModelInviteLink link, ModelUser user)
        {
            if (link == null)
                return;
            if (link.UserId != user.Id)//邀请用户与被邀请用户不相同
            {
                var poster = UnitWork.FirstOrDefault<ModelInvitePoster>(p => p.Id == link.PosterId);
                //查询邀请记录是否存在
                var record = UnitWork.FirstOrDefault<ModelInviteLinkRecord>
                    (p => p.InviterId== link.UserId && p.InviteeUid.Equals(user.Id));
                if (record != null)
                {   //修改邀请记录  邀请时间
                    record.InvitationTime = DateTime.Now;
                    record.OpenTime=DateTime.Now;
                    record.OpenType = (int) xEnum.UserType.Customer;
                }
                else
                {//新增邀请记录
                    record = new ModelInviteLinkRecord()
                    {
                        LinkId = link.Id,
                        PosterId = link.PosterId,
                        InviterId = link.UserId,
                        InviterName = link.InviterName,
                        InviterPhone = link.InviterPhone,
                        InviteeUid = user.Id,
                        InviteeName = user.NickName,
                        InviteePhone = user.Phone,
                        Status = (int)xEnum.InviteRecordStatus.Reg,
                        InviteeCouponAmout = 0,
                        InviteeCouponQuantity = 0,
                        IsSendInviterCoupon = 0,
                        IsSendInviteeCoupon = 0,
                        CreateTime = DateTime.Now,
                        InvitationTime = DateTime.Now,
                        IsOpenVip=0,
                    };
                    poster.CountRegUser += 1;
                    UnitWork.AddOrUpdate(poster);
                }
                UnitWork.AddOrUpdate(record);
                UnitWork.Save();
            }
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuInviteLink req)
        {
            var model = xConv.CopyMapper<ModelInviteLink, ReqAuInviteLink>(req);
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
        
    }
}