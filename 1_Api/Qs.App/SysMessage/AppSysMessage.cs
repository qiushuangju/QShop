using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qs.Comm;
using Microsoft.Extensions.Logging;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Response;
using Qs.Repository.Request;

namespace Qs.App
{
    /// <summary>
    ///消息通知
    /// </summary>
    public class AppSysMessage : AppBaseString<ModelSysMessage, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        private readonly ILogger<AppSysMessage> _logger;

        public AppSysMessage(IUnitWork<QsDBContext> unitWork, IRepository<ModelSysMessage, QsDBContext> repository,
            AppRevelanceManager app, ILogger<AppSysMessage> logger, DbExtension dbExtension, IAuth auth) : base(
            unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
            _logger = logger;
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(QuerySysMessageListReq req)
        {
            var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            var linq = UnitWork.Find<ModelSysMessage>(u => true);

            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(u => u.Title.Contains(req.Key) || u.Id.Contains(req.Key));
            }

            //过滤消息状态
            if (xConv.ToInt(req.Status) != 0)
            {
                linq = linq.Where(u => u.ToStatus == req.Status);
            }

            if (xConv.ToInt(req.Type) == (int) xEnum.MessageType.PublicNotice)
            {
                linq = linq.Where(u => u.Type == (int) xEnum.MessageType.PublicNotice && u.ToId == "");
            }

            if (xConv.ToInt(req.Type) == (int) xEnum.MessageType.Message)
            {
                var loginUser = _auth.GetCurrentContext().User;
                if (loginUser != null)
                {
                    linq = linq.Where(u => u.ToId == loginUser.Id);
                }
            }

            result.Result = linq.OrderByDescending(u => u.CreateTime)
                .Skip((req.Page - 1) * req.Limit)
                .Take(req.Limit);
            result.Count = linq.Count();
            return result;
        }

        public void Add(ModelSysMessage obj)
        {
            Repository.Add(obj);
        }

        /// <summary>
        /// 发送指定消息给用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="message"></param>
        public void SendMsgTo(string userId, string message)
        {
            ModelUser user = null;
            if (userId == Guid.Empty.ToString())
            {
                user = new ModelUser
                {
                    NickName = Define.SystemUserName,
                    Id = userId
                };
            }
            else
            {
                user = UnitWork.FirstOrDefault<ModelUser>(u => u.Id == userId);
            }

            if (user == null)
            {
                _logger.LogError($"未能找到用户{userId},不能给该用户发送消息");
                return;
            }

            Repository.Add(new ModelSysMessage
            {
                ToId = user.Id,
                ToName = user.NickName,
                SubTypeName = "系统消息",
                SubType = (int) xEnum.MessageSubType.MsgSys,
                FromId = Guid.Empty.ToString(),
                FromName = "系统管理员",
                Content = message,
                CreateTime = DateTime.Now
            });
        }


        /// <summary>
        /// 消息变为已读
        /// </summary>
        public List<ResMsgSubType> ListMessageType()
        {
            var user = _auth.GetCurrentContext().User;
            List<ResMsgSubType> listVm = new List<ResMsgSubType>();
            var list = xEnum.ListEnumModel(typeof(xEnum.MessageType));
            foreach (var item in list)
            {
                ResMsgSubType vm = new ResMsgSubType()
                {
                    CountMsg = UnitWork.Count<ModelSysMessage>(p => p.ToStatus == (int) xEnum.ComStatus.Disable
                                                                    && p.ToId == user.Id),
                    Type = item.Value,
                    Title = item.Text       
                };
                listVm.Add(vm);
            }
            return listVm;
        }

        /// <summary>
        /// 消息变为已读
        /// </summary>                  
        public void Read(ReadMsgReq req)
        {
            UnitWork.Update<ModelSysMessage>(u => u.Id == req.Id, u => new ModelSysMessage
            {
                ToStatus = (int)xEnum.ComStatus.Normal
            });
        }

        /// <summary>
        /// 消息采用逻辑删除
        /// </summary>
        /// <param name="ids"></param>
        public void Del(string[] ids)
        {
            UnitWork.Delete<ModelSysMessage>(p=> ids.ToList().Contains(p.Id));

        }

        //AddOrUpdateNotice

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdateNotice(ReqAuSysMessage req)
        {
            var model = xConv.CopyMapper<ModelSysMessage, ReqAuSysMessage>(req);
            var isNew = string.IsNullOrEmpty(model.Id) ? true : false;
            if (isNew)
            {
                var user = _auth.GetCurrentContext().User;
                model.Type = (int)xEnum.MessageType.PublicNotice;
                model.SubType = (int)xEnum.MessageSubType.MsgSys;
                model.UserId = user.Id;
                model.CreateTime = DateTime.Now;
                Repository.Add(model);
            }
            else
            {
                UnitWork.Update<ModelSysMessage>(u => u.Id == model.Id, u => new ModelSysMessage
                {
                   Title=model.Title,
                   SubTypeName=model.SubTypeName,
                    Content= model.Content,
                });
            }
        }
    }
}