using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using App.Wx;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.App.UserManager.Request;
using Qs.App.UserManager.Response;
using Qs.App.Wx;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;
using Qs.Repository.Vm;
using Qs.Repository.Wx;
using WebApi.Model.ModelRes;
using ModelUser = Qs.Repository.Domain.ModelUser;

namespace Qs.App.UserManager
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class AppUserManager : AppBaseString<ModelUser, QsDBContext>
    {
        private AppInviteLink _appInviteLink;
        private AppFileUpload _appFile;
        private AppStoreSetting _appStoreSetting;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unitWork"></param>
        /// <param name="repository"></param>
        /// <param name="appInviteLink"></param>
        /// <param name="appFile"></param>
        /// <param name="appStoreSetting"></param>
        /// <param name="dbExtension"></param>
        /// <param name="auth"></param>
        public AppUserManager(IUnitWork<QsDBContext> unitWork, IRepository<ModelUser, QsDBContext> repository,AppInviteLink appInviteLink,
            AppFileUpload appFile, AppStoreSetting  appStoreSetting, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _appInviteLink = appInviteLink;
            _appFile = appFile;
            _appStoreSetting = appStoreSetting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ModelUser GetByAccount(string account)
        {
            return Repository.FirstOrDefault(u => u.Account == account);
        }

        /// <summary>
        /// 加载当前登录用户可访问的一个部门及子部门全部用户
        /// 如果请求的request.OrgId为空，则可以获取到已被删除机构的用户（即：没有分配任何机构的用户）
        /// </summary>
        public TableData Load(QueryUserListReq req)
        {
            var loginUser = _auth.GetCurrentContext();

            IQueryable<ModelUser> query = UnitWork.Find<ModelUser>(null);
            if (!string.IsNullOrEmpty(req.Key))
            {
                query = UnitWork.Find<ModelUser>(u => u.NickName.Contains(req.Key) || u.Account.Contains(req.Key)
                                                                               || u.Phone.Contains(req.Key));
            }

            var linq = from user in query
                           join relevance in UnitWork.Find<Relevance>(u => u.Key == "UserOrg")
                               on user.Id equals relevance.FirstId into temp
                           from r in temp.DefaultIfEmpty()
                           select new
                           {
                               user.ExpendMoney,
                               user.Points,
                               user.Balance,
                               user.Account,
                               user.NickName,
                               user.UrlAvater,
                               user.Phone,
                               user.UserType,
                               user.Id,
                               user.Sex,
                               user.Status,
                               user.CreateTime,
                               user.LastLoginTime,
                               user.RealName,
                            
                               r.Key,
                               r.SecondId,
                               user.SourceUserId,
                               user.ParentId,
                           };
            if (req.MySon)
            {
                var currentId = loginUser.User.Id;
                req.LastSourceUserId = currentId;
            }
            if (xConv.ToInt(req.MinUserType) != 0)
            {
                linq = linq.Where(p => p.UserType >= req.MinUserType);
            }
            if (xConv.ToInt(req.MaxUserType) != 0)
            {
                linq = linq.Where(p => p.UserType <= req.MaxUserType);
            }
            if (xConv.ToInt(req.Status) != 0)
            {
                linq = linq.Where(p => p.Status.Equals(req.Status));
            }
            if (!string.IsNullOrEmpty(req.BusinessUserId))
            {
                linq = linq.Where(p => p.ParentId.Equals(req.BusinessUserId));
            }
           
            if (!string.IsNullOrEmpty(req.LastSourceUserId))
            {
                linq = linq.Where(p => p.ParentId == req.LastSourceUserId);
            }

            //总数
            int userOrgsCount = linq.Count();
            //分页
            linq = linq.OrderBy(u => u.NickName).Skip((req.Page - 1) * req.Limit).Take(req.Limit);
           
           
            var listRole = (from relevance in UnitWork.Find<Relevance>(u => u.Key == Define.UserRole)
                            join role in UnitWork.Find<Role>(p => true)
                                on relevance.SecondId equals role.Id
                            select new
                            {
                                userId = relevance.FirstId,
                                RoleId = role.Id,
                                RoleName = role.Name
                            }).ToList();

            List<UserView> listVmUser = linq.ToList().GroupBy(b => b.Account).Select(p => new UserView
            {
                Id = p.FirstOrDefault().Id,
                Account = p.Key,
                Name = p.FirstOrDefault().NickName,
                UrlAvater = p.FirstOrDefault().UrlAvater,
                Phone = p.FirstOrDefault().Phone,
                UserType = p.FirstOrDefault().UserType,
                StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), p.FirstOrDefault().UserType),
                Sex = p.FirstOrDefault().Sex,
                Status = p.FirstOrDefault().Status,
                StrStatus = xEnum.GetEnumDescription(typeof(xEnum.UserStatus), p.FirstOrDefault().Status),
                CreateTime = p.FirstOrDefault().CreateTime,
                LastLoginTime = p.FirstOrDefault().LastLoginTime,
                ExpendMoney = p.FirstOrDefault().ExpendMoney,
                Points = p.FirstOrDefault().Points,
                Balance = p.FirstOrDefault().Balance,
                RealName= p.FirstOrDefault().RealName,
           SourceUserId = p.FirstOrDefault().SourceUserId
            }).ToList();
            //直接下级
            List<ModelUser> listDirectUsers = UnitWork.Find<ModelUser>(p => listVmUser.Select(q => q.Id).Contains(p.ParentId)).ToList();
            //间接下级
            List<ModelUser> listIndirectUsers = UnitWork.Find<ModelUser>(p => listDirectUsers.Select(q => q.Id).Contains(p.ParentId)).ToList();
            foreach (var vm in listVmUser)
            {
                var listUserRole = listRole.Where(p => p.userId == vm.Id);
                vm.RoleIds = xConv.AddSplit(listUserRole.Select(p => p.RoleId).ToList(), ",");
                vm.RoleNames = xConv.AddSplit(listUserRole.Select(p => p.RoleName).ToList(), ",");
                vm.TeamUserTotal = listDirectUsers.Count(p => p.ParentId == vm.Id);
            }
            return new TableData
            {
                Count = userOrgsCount,
                Result = listVmUser,
            };
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ModelUser model)
        {
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
        ///   小程序新增或修改用户
        /// </summary>
        /// <param name="wxOpenId"></param>
        /// <param name="phone"></param>
        /// <param name="inviteCode">邀请码</param>
        public ModelUser AddOrUpdateWxUser(string wxOpenId,string phone, string inviteCode )
        {
            var storeId = _auth.GetStoreId();
            var user = UnitWork.FirstOrDefault<ModelUser>(p => p.WxOpenId == wxOpenId&&p.UserType>=(int)xEnum.UserType.Customer);
            if (user==null)
            {
                user = UnitWork.FirstOrDefault<ModelUser>(p => p.Phone == phone&&p.UserType >= (int)xEnum.UserType.Customer);
            }
            if (user == null)
            {
                user = new ModelUser();
                user.Id = xConv.NewGuid();
                user.UserType = (int) xEnum.UserType.Customer;
                user.SourceUserId = $"{user.Id}";
                if (!string.IsNullOrEmpty(inviteCode)) //有邀请码
                {
                    var link = UnitWork.FirstOrDefault<ModelInviteLink>(p => p.Id == inviteCode);
                    _appInviteLink.AcceptInvite(link, user); //接受邀请
                    ModelUser parentUser = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == link.UserId);
                    if (parentUser.UserType>=(int)xEnum.UserType.Agent)   //分销代理才有下级
                    {
                        user.SourceUserId = parentUser.UserType >= (int) xEnum.UserType.Agent
                            ? $"{parentUser.SourceUserId},{user.Id}"
                            : $"{user.Id}";
                        user.ParentId = parentUser.Id;
                    } 
                }

                if (!string.IsNullOrEmpty(wxOpenId))
                {
                    user.Account = wxOpenId;
                    user.WxOpenId = wxOpenId;
                }
                else
                {
                    user.Phone = phone;
                }

                user.StoreId = storeId;
                user.UrlAvater = $"{Define.HttpBaseApi()}/wwwroot/ImgWxApp/logo.png";
               
            }
            UnitWork.AddOrUpdate(user);
            UnitWork.Save();
            return user;
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResUser GetDetail(string id)
        {
            ModelUser user = Get(id);
            if (user==null)
            {
                user = _auth.GetCurrentContext().User;
            }

            var img = _appFile.Get(user.ImgIdWxQrCode);
            var vm = ResUser.ToView(user, img);
            vm.CountCoupon =
                UnitWork.Count<ModelUserCoupon>(p =>
                    p.UserId == user.Id && p.Status == (int)xEnum.CouponStatus.Usable);
            vm.StrStatus = xEnum.GetEnumDescription(typeof(xEnum.UserStatus), vm.Status);
            vm.StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType), vm.UserType);
            vm.Parent = UnitWork.Find<ModelUser>(p => p.Id == vm.ParentId).FirstOrDefault();
            vm.GrandParent = GetUserGrandParent(vm.SourceUserId,user.Id);
            return vm;
        }

        /// <summary>
        /// 获取用户间接上级
        /// </summary>
        /// <param name="sourceUserId"></param>
        /// <returns></returns>
        public ModelUser GetUserGrandParent(string sourceUserId,string userId)
        {
            List<string> ids = xConv.GetListParentId(sourceUserId);
            if (ids.Count() <=2)
                return null;
            var grandParentId = ids.Last();
            return UnitWork.Find<ModelUser>(p => p.Id == grandParentId).FirstOrDefault();
        }

        /// <summary>
        ///  业务员信息
        /// </summary>
        /// <returns></returns>
        public ResUserBusiness GetMyBusinessUserInfo()
        {
            ModelUser user = _auth.GetCurrentContext().User;
            var userBusiness = Repository.FirstOrDefault(p => p.Id == user.ParentId);
            ResUserBusiness res = xConv.CopyMapper<ResUserBusiness, ModelUser>(userBusiness);
            if (!string.IsNullOrEmpty(userBusiness.ImgIdWxQrCode))
            {
                var img = _appFile.Get(userBusiness.ImgIdWxQrCode);
                res.UrlWxQrCode = img.FilePath;
            }
            return res;
        }

        /// <summary>
        /// 删除用户,包含用户与组织关系、用户与角色关系
        /// </summary>
        /// <param name="ids"></param>
        public override void Delete(string[] ids)
        {
            UnitWork.ExecuteWithTransaction(() =>
            {
                UnitWork.Delete<Relevance>(u => (u.Key == Define.UserRole || u.Key == Define.UserOrg)
                                                && ids.Contains(u.FirstId));
                UnitWork.Delete<ModelUser>(u => ids.Contains(u.Id));
                UnitWork.Save();
            });

        }

        /// <summary>
        /// 修改密码 (App)
        /// </summary>
        /// <param name="req"></param>
        public void ChangeUserPwd(ReqChangeUserPwd req)
        {

            ModelUser user =
                Repository.FirstOrDefault(p => p.Phone == req.Phone && p.UserType <= (int)xEnum.UserType.Agent);
            user.Password = xConv.MD5Encoding(req.Pwd, xConv.ToStrDateTime(user.CreateTime));
            Repository.Update(user);
        }
        /// <summary>
        /// 修改用户类型
        /// </summary>
        /// <param name="req"></param>
        public void ChangeUserType(ReqChangeUserType req)
        {
            ModelUser user =
                UnitWork.FirstOrDefault<ModelUser>(p => p.Id == req.Id);
           
            req.Check();
            if (user.UserType <= (int)xEnum.UserType.SysAdmin)
            {
                throw new Exception("管理员类型不可修改!");
            }
            user.UserType = req.UserType;
            user.RealName = req.RealName;
            user.TokenTime = xConv.ToDateTime("2020-01-01");
            Repository.Update(user);
             UnitWork.Update<ModelInviteLinkRecord>(p => p.InviteeUid == req.Id,p=>new ModelInviteLinkRecord()
            {
                OpenType = req.UserType
            });
             UnitWork.Save();
        }

        /// <summary>
        /// 修改用户状态（审核）
        /// </summary>
        /// <param name="req"></param>
        public void ChangeUserStatus(ReqChangeUserStatus req)
        {
            var model = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == req.Id);
            UnitWork.ExecuteWithTransaction(() =>
            {
                UnitWork.Update<ModelUser>(u => u.Id == model.Id, u => new ModelUser
                {
                    Status = req.Status,
                });
               
                UnitWork.Save();
            });
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="req"></param>
        public void CheckUser(ReqCheckUser req)
        {
            var model = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == req.Id);
            UnitWork.ExecuteWithTransaction(() =>
            {
                model.Status = req.Status;
                UnitWork.AddOrUpdate(model);
              
                UnitWork.Save();
            });
        }

        /// <summary>
        /// 修改名字
        /// </summary>
        /// <param name="nickName"></param>
        public void ChangeNickName(ReqChangeName req)
        {
            var user = _auth.GetCurrentContext().User;
            user.NickName = req.Name;
            Repository.Update(user);
        }
        /// <summary>
        /// 修改头像
        /// </summary>
        /// <param name="urlAvater"></param>
        public void ChangeAvater(string urlAvater)
        {
            var user = _auth.GetCurrentContext().User;
            user.UrlAvater = urlAvater;
            Repository.Update(user);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="request"></param>
        public void ChangePassword(ReqChangePwd request)
        {
            if (request.UserType == null)
                throw new Exception("用户类型不能为空");
            var model = UnitWork.FirstOrDefault<ModelUser>(u => u.Account == request.Account && u.UserType == request.UserType);
            Repository.Update(u => u.Account == request.Account && u.UserType == request.UserType,
                user => new ModelUser
                {

                    Password = xConv.MD5Encoding(request.Password, xConv.ToStrDateTime(model.CreateTime))
                });
        }

        /// <summary>
        /// 管理端添加或修改用户(业务、司机、调度)
        /// </summary>
        /// <param name="req"></param>
        public void AddOrUpdateManagement(UpdateUserReq req)
        {
            var isNew = string.IsNullOrEmpty(req.Id) ? true : false;
            if (isNew)
            {
                var model = xConv.CopyMapper<ModelUser, UpdateUserReq>(req);
                model.CreateTime = System.DateTime.Now;
                model.Password = xConv.MD5Encoding(req.Password, xConv.ToStrDateTime(model.CreateTime));
                var users = UnitWork.Find<ModelUser>(u => u.Account == req.Account && u.UserType == req.UserType);
                if (users.Count() > 0)
                    throw new Exception("账号已存在，请更换账号");
                Repository.Add(model);
            }
            else
            {
                var model = UnitWork.FirstOrDefault<ModelUser>(u => u.Id == req.Id);
                var user = UnitWork.Find<ModelUser>(u =>
                u.Account == req.Account
                && u.UserType == req.UserType
                && u.Id != req.Id);
                if (user.Count() > 0)
                    throw new Exception("账号已存在，请更换账号");
                UnitWork.ExecuteWithTransaction(() =>
                {
                    //修改用户信息
                    UnitWork.Update<ModelUser>(u => u.Id == model.Id, u => new ModelUser
                    {
                        Account = req.Account,
                        Phone = req.Phone,
                        NickName = req.Name,
                        UserType = req.UserType,
                        Sex = req.Sex,
                        Status = req.Status,
                    });
                    UnitWork.Save();
                });
            }
        }

        /// <summary>
        /// 管理端添加或修改员工
        /// </summary>
        /// <param name="req"></param>
        public void AddOrUpdateEmp(UpdateUserReq req)
        {
            req.Check();
            ModelUser model = new ModelUser();
            var isNew = string.IsNullOrEmpty(req.Id) ? true : false;
            if (isNew)
            {
                model = xConv.CopyMapper<ModelUser, UpdateUserReq>(req);
                model.Id = xConv.NewGuid();
                model.CreateTime = System.DateTime.Now;
                model.UserType = (int)xEnum.UserType.StoreAdmin;
                model.Password = xConv.MD5Encoding(req.Password, xConv.ToStrDateTime(model.CreateTime));
                model.BalancePwd = xConv.MD5Encoding(req.BalancePwd, xConv.ToStrDateTime(model.CreateTime));
                var users = UnitWork.Find<ModelUser>(u => u.Account == req.Account
                                                          && u.UserType == (int)xEnum.UserType.StoreAdmin);
                if (users.Any())
                {
                    throw new Exception("账号已存在，请更换账号");
                }

                Repository.Add(model);
            }
            else
            {
                model = UnitWork.FirstOrDefault<ModelUser>(u => u.Id == req.Id);
                var user = UnitWork.Find<ModelUser>(u =>
                    u.Account == req.Account
                    && u.UserType <= (int)xEnum.UserType.SysAdmin
                    && u.Id != req.Id);
                if (user.Any())
                {
                    throw new Exception("账号已存在，请更换账号");
                }

                if (!string.IsNullOrEmpty(req.Password))
                {
                    var pwd = xConv.MD5Encoding(req.Password, xConv.ToStrDateTime(model.CreateTime));
                    UnitWork.Update<ModelUser>(u => u.Id == model.Id, u => new ModelUser
                    {
                        Password = pwd,

                    });
                }
                if (!string.IsNullOrEmpty(req.BalancePwd))
                {
                    var balancePwd = xConv.MD5Encoding(req.BalancePwd, xConv.ToStrDateTime(model.CreateTime));
                    UnitWork.Update<ModelUser>(u => u.Id == model.Id, u => new ModelUser
                    {
                        BalancePwd = balancePwd,

                    });
                }
                UnitWork.Update<ModelUser>(u => u.Id == model.Id, u => new ModelUser
                {
                    Account = req.Account,
                    Phone = req.Phone,
                    NickName = req.Name,
                    UserType = (int)xEnum.UserType.StoreAdmin,
                    Sex = req.Sex,
                    Status = req.Status,
                });
            }

            var listOrgId = xConv.ToListString(req.OrganizationIds);
            foreach (var orgId in listOrgId)
            {
                Relevance relevance = new Relevance();
                relevance.FirstId = model.Id;
                relevance.SecondId = orgId;
                relevance.Key = Define.UserOrg;
                relevance.Status = (int)xEnum.ComStatus.Normal;
                relevance.OperateTime = DateTime.Now;
                relevance.OperatorId = model.Id;
                UnitWork.Add(relevance);
            }
            UnitWork.Save();
        }


        /// <summary>
        /// 生成邀请海报(带二维码)
        /// </summary>
        /// <returns></returns>
        public ResInviteLink GenInvitePoster(ReqGenInvitePoster req, IHostingEnvironment _hostingEnvironment)
        {
            var poster = UnitWork.FirstOrDefault<ModelInvitePoster>(p => p.Id == req.PosterId);
            var user = _auth.GetCurrentContext().User;
            ModelInviteLink link = new ModelInviteLink()
            {
                Id = xConv.NewGuid(),
                InviterName = user.NickName,
                InviterPhone = user.Phone,
                PosterId = poster.Id,
                UserId = user.Id
            };
            UnitWork.AddOrUpdate(link);
            UnitWork.Save();

            VmSettingBasicWxApp setting = _appStoreSetting.GetDetail(DefineSetting.BasicWxApp);
            var basePath = _hostingEnvironment.WebRootPath;
            //邀请页面二维码
            var urlQrcode = CreateQrCode.GetQrCode(setting, basePath, $"packageMy/pages/personalcenter/Login/Login",
                $"{link.Id}", 105, 105);


            string rootPath = _hostingEnvironment.WebRootPath;
            ModelFileUpload file = UnitWork.FirstOrDefault<ModelFileUpload>(p => p.Id == poster.ImageId);
            string temp = file.FilePath.Replace($"{Define.HttpBaseApi()}", "").Replace("wwwroot", "")
                .Replace("/", "\\");
            string tempQrCode = urlQrcode.QrCodeRelativePath.Replace(Define.HttpBaseApi(), "")
                .Replace("/", "\\");
            Image imgSource = Image.FromFile($@"{_hostingEnvironment.WebRootPath}\{temp}");
            Image imgQrCode = Image.FromFile($@"{_hostingEnvironment.WebRootPath}\{tempQrCode}");

            string urlPoster =
                $"{Img.CombinImageQrCode(rootPath, imgSource, imgQrCode, xConv.ToInt(110), xConv.ToInt(1125), xConv.ToInt(295), xConv.ToInt(295))}";


            return new ResInviteLink()
            {
                Id = link.Id,
                UrlPoster = urlPoster
            };
        }

        /// <summary>
        /// 获取归属关系
        /// </summary>
        /// <param name="record">邀请记录</param>
        /// <param name="inviteeUserId">被邀请人UserId</param>
        /// <param name="opType">被邀请人设置身份</param>
        /// <returns></returns>
        public VmSourceUser GetUserSource(ModelInviteLinkRecord record, string inviteeUserId, xEnum.UserType opType)
        {
            VmSourceUser source = new VmSourceUser();
            source.ParentId = "";
            source.SourceUserId = $"{inviteeUserId}";
            if (record!=null)
            {    
                //查询邀请人信息,绑定上下级关系
                var inviteUser = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == record.InviterId);
                //邀请人存在身份才能成为被邀请人的上级
                if (inviteUser != null && inviteUser.UserType >= (int)xEnum.UserType.Agent)
                {
                    List<string> listUserId = xConv.ToListString(inviteUser.SourceUserId);
                    var parentSourceId = inviteUser.SourceUserId;
                    List<ModelUser> listParentUser = UnitWork.Find<ModelUser>(p => listUserId.Contains(p.Id)).ToList();

                    if (opType == xEnum.UserType.Agent)//代理不设置上级
                    {
                        parentSourceId = "";
                    }
                  
                    if (opType == xEnum.UserType.Customer) //会员路径拼接
                    {
                        string agentId = "";
                        string agentDealerId = "";
                        foreach (var item in listUserId)
                        {
                            var userParent = listParentUser.FirstOrDefault(p => p.Id == item) ?? new ModelUser();
                            // if (userParent.UserType == (int)xEnum.UserType.Agent)
                            // {
                            //     agentId = userParent.Id;
                            // }
                            // if (userParent.UserType == (int)xEnum.UserType.AgentDealer)
                            // {
                            //     agentDealerId = userParent.Id;
                            // }
                        }
                        parentSourceId = $"{agentId},{agentDealerId}";
                        parentSourceId = parentSourceId.TrimStart(',').TrimEnd(',');  //去除收尾逗号 
                    }
                    source.ParentId = parentSourceId==""? "":xConv.ToListString(parentSourceId).Last();
                    source.SourceUserId = string.IsNullOrEmpty(parentSourceId) ? $"{inviteeUserId}" : $"{parentSourceId},{inviteeUserId}";
                }
            }
           
            return source;
        }

       

        /// <summary>
        /// 充值余额(管理员)
        /// </summary>
        /// <returns></returns>
        public void RechargeBalance(ReqRechargeBalance req)
        {
            var opType = xEnum.BalanceType.AdminRecharge;
            switch (req.Mode.ToLower())
            {
                case "inc":
                    opType = xEnum.BalanceType.AdminRecharge;
                    break;
                case "dec":
                    opType = xEnum.BalanceType.AdminConsume;
                    break;
                case "final":
                    opType = xEnum.BalanceType.AdminOp;
                    break;
            }

            OpBalance(new Repository.Request.VmBalance()
            {
                UserId = req.UserId,
                OpType = opType,
                Money = xConv.ToDecimal(req.Money),
                Remark = req.Remark,
            });
        }

        /// <summary>
        /// 余额操作
        /// </summary>
        /// <returns></returns>
        public void OpBalance(Repository.Request.VmBalance req)
        {
            var balanceUser = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == req.UserId);
            var currentUser = _auth.GetCurrentContext().User;
            var storeId = _auth.GetStoreId();
            if (balanceUser == null)
            {
                throw new Exception("用户id不正确");
            }

            decimal amount = 0M;
            decimal payAmount = 0M;
            switch (req.OpType)
            {
                case xEnum.BalanceType.AdminConsume:
                    amount = -req.Money;
                    break;
                case xEnum.BalanceType.ConsumeOrder:
                    amount = -req.Money;
                    break;
                case xEnum.BalanceType.AdminRecharge:
                    amount = req.Money;
                    break;
                case xEnum.BalanceType.Recharge:
                    amount = req.Money;
                    break;
                case xEnum.BalanceType.AdminOp:
                    amount = req.Money;
                    break;
                case xEnum.BalanceType.Refund:
                    amount = req.Money;
                    break;
                case xEnum.BalanceType.Deposit:
                case xEnum.BalanceType.Final:
                    amount = -req.Money;
                    break;
            }
            string userTypeStr = xEnum.GetEnumDescription(typeof(xEnum.UserType), currentUser.UserType);
            if (balanceUser.Balance + amount < 0)
            {
                throw new Exception("余额不足,请使用其他方式支付!");
            }
            balanceUser.Balance = balanceUser.Balance + amount;
            if (xEnum.BalanceType.AdminOp == req.OpType)
            {
                balanceUser.Balance = amount;
            }
            ModelUserBalanceLog log = new ModelUserBalanceLog()
            {
                Scene = (int)req.OpType,
                OrderId = req.OrderId,
                Describe = $"{userTypeStr}[{currentUser.NickName}]操作",
                UserId = balanceUser.Id,
                Money = req.Money,
                Balance = balanceUser.Balance,
                Remark = req.Remark,
                StoreId = storeId
            };
            UnitWork.Add(log);        
            UnitWork.Update(balanceUser);
            UnitWork.Save();
        }


        /// <summary>
        /// 积分操作
        /// </summary>
        /// <returns></returns>
        public void OpPoints(VmPoints req)
        {
            if (req.Points == 0)
            {
                return;
            }
            var balanceUser = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == req.UserId);
            var currentUser = _auth.GetCurrentContext().User;
            var storeId = _auth.GetStoreId();
            if (balanceUser == null)
            {
                throw new Exception("用户id不正确");
            }
            decimal points = 0M;
            switch (req.OpType)
            {
                case xEnum.PointType.ConsumeOrder:
                    points = -req.Points;
                    break;
                case xEnum.PointType.OrderAdd:
                    points = req.Points;
                    break;
                case xEnum.PointType.Refund:
                    points = req.Points;
                    break;
            }
            string userTypeStr = xEnum.GetEnumDescription(typeof(xEnum.UserType), currentUser.UserType);
            if (balanceUser.Points + points < 0)
            {
                throw new Exception("积分不足,请不使用积分抵扣!");
            }

            balanceUser.Points = balanceUser.Points + points;
            ModelUserPointsLog log = new ModelUserPointsLog()
            {
                Scene = (int)req.OpType,
                OrderId = req.OrderId,
                Describe = $"{userTypeStr}[{currentUser.NickName}]操作",
                UserId = balanceUser.Id,
                Points = points,
                BalancePoints = balanceUser.Points,
                Remark = req.Remark,
                StoreId = storeId
            };
            UnitWork.Add(log);
            UnitWork.Update(balanceUser);
            UnitWork.Save();
        }

        // /// <summary>
        // /// 用户支付金额变动
        // /// </summary>
        // /// <param name="req"></param>
        // public void OpConsumptionMoney(ConsumptionMoneyVm req)
        // {
        //     if (req.User == null)
        //     {
        //         throw new Exception("未查询到用户");
        //     }
        //     //查询用户
        //     if (req.ConsumptionType == xEnum.ConsumptionMoneyType.Consume)//用户下单
        //     {
        //         //商品总额
        //         decimal money = req.Money - req.FreightPrice > 0 ? req.Money - xConv.ToDecimal(req.FreightPrice) : 0;
        //         req.User.PayMoney += req.Money > 0 ? req.Money : 0;//用户支付金额（含运费，含开会员金额，不含退款）
        //         req.User.ExpendMoney += req.Money > 0 ? req.Money : 0;//用户实际支付金额（含运费，不含退款）
        //         // req.User.PayGoodsTotalMoney = xConv.ToDecimal(req.User.PayGoodsTotalMoney ) + money;//用户购物金额（不含运费，不含退款）
        //     }
        //     else if (req.ConsumptionType == xEnum.ConsumptionMoneyType.OpenMember
        //         || req.ConsumptionType == xEnum.ConsumptionMoneyType.Freight)//开通会员、支付运费
        //     {
        //         req.User.PayMoney += req.Money > 0 ? req.Money : 0;//用户支付金额
        //     }
        //     else if (req.ConsumptionType == xEnum.ConsumptionMoneyType.Refund)
        //     {
        //         //商品总额
        //         decimal money = req.Money - req.FreightPrice > 0 ? req.Money - xConv.ToDecimal(req.FreightPrice) : 0;
        //         req.User.PayMoney -= req.Money;//用户支付金额
        //         req.User.ExpendMoney -= req.Money > 0 ? req.Money : 0;//用户实际支付金额（含运费，不含退款）
        //         // req.User.PayGoodsTotalMoney = xConv.ToDecimal(req.User.PayGoodsTotalMoney)-money;//用户购物金额（不含运费，不含退款）
        //     }
        //     UnitWork.Update(req.User);
        // }




        /// <summary>
        /// 充值到账
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="amount"></param>
        public void RechargePaid(string orderId, decimal amount)
        {
            var order = UnitWork.FirstOrDefault<ModelRechargeOrder>(p => p.Id == orderId);
            order.PayStatus = (int)xEnum.PayStatus.Paid;
            order.PayPrice = amount;
            order.PayTime = DateTime.Now;
            OpBalance(new Repository.Request.VmBalance()
            {
                Money = order.ActualMoney,
                OpType = xEnum.BalanceType.Recharge,
                OrderId = order.Id,
                Remark = $"用户充值,{order.PayPrice}送{order.GiftMoney}",
                UserId = order.UserId
            });
            UnitWork.Update(order);
            UnitWork.Save();


        }
        // /// <summary>
        // /// 充值到账
        // /// </summary>
        // /// <param name="orderId"></param>
        // /// <param name="amount"></param>
        // public void RechargePaidByPlan(string orderId, decimal amount)
        // {
        //     var order = UnitWork.FirstOrDefault<ModelRechargeOrder>(p => p.Id == orderId);
        //     var plan = UnitWork.FirstOrDefault<ModelRechargePlan>(p => p.Id == order.PlanId);
        //     order.PayStatus = (int)xEnum.PayStatus.Paid;
        //     order.PayPrice = amount;
        //     order.PayTime = DateTime.Now;
        //     OpBalance(new BalanceVm()
        //     {
        //         Money = order.ActualMoney,
        //         OpType = xEnum.BalanceType.Recharge,
        //         OrderId = order.Id,
        //         Remark = $"用户充值,{plan.Money}送{order.GiftMoney}",
        //         UserId = order.UserId
        //     });
        //     UnitWork.Update(order);
        //     UnitWork.Save();
        //
        //
        // }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <returns></returns>
        public void SetRechargePwd(ReqSetRechargePwd req)
        {
            req.Check();
            var user = _auth.GetCurrentContext().User;
            
            var userDb = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == user.Id);
            var balancePwd = xConv.MD5Encoding(req.RechargePwd, xConv.ToStrDateTime(userDb.CreateTime));
            userDb.BalancePwd = balancePwd;
            UnitWork.Update(userDb);
            UnitWork.Save();
        }
        /// <summary>
        /// 更换手机号
        /// </summary>
        public void SetReplacePhone(ReqReplacePhone req)
        {
            req.Check();
            var user = _auth.GetCurrentContext().User;
            
            var userDb = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == user.Id);
            var userOrDefault = UnitWork.FirstOrDefault<ModelUser>(p => p.Id != userDb.Id && p.UserType == userDb.UserType && p.Phone == req.NewPhone);
            if (userOrDefault != null)
            {
                throw new Exception("修改失败，手机号已存在");
            }
            userDb.Phone = req.NewPhone;
            UnitWork.Update(userDb);
            UnitWork.Save();
        }

        /// <summary>
        /// 设置支付密码
        /// </summary>
        /// <returns></returns>
        public void SetWxQrCode(ReqSetWxQrCode req)
        {
            var user = _auth.GetCurrentContext().User;
            var userDb = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == user.Id);
            userDb.ImgIdWxQrCode = req.ImgIdQrCode;
            UnitWork.Update(userDb);
            UnitWork.Save();
        }

        /// <summary>
        /// 设置头像昵称
        /// </summary>
        /// <returns></returns>
        public void ChangeUserInfo(ReqChangeUserInfo req)
        {
            var user = _auth.GetCurrentContext().User;
            var userDb = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == user.Id);
            userDb.UrlAvater = req.UrlAvater;
            userDb.NickName = req.Name;
            UnitWork.Update(userDb);
            UnitWork.Save();
        }


        /// <summary>
        /// 获取指定角色包含的用户列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TableData> LoadByRole(QueryUserListByRoleReq req)
        {
            var users = from userRole in UnitWork.Find<Relevance>(u =>
                    u.SecondId == req.roleId && u.Key == Define.UserRole)
                        join user in UnitWork.Find<ModelUser>(null) on userRole.FirstId equals user.Id into temp
                        from c in temp.Where(u => u.Id != null)
                        select c;

            return new TableData
            {
                Count = users.Count(),
                Result = users.Skip((req.Page - 1) * req.Limit).Take(req.Limit)
            };
        }
        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="req"></param>
        public void ChangeProfile(ChangeProfileReq req)
        {
            req.Check();
            var user = _auth.GetCurrentContext().User;
            if (user.Account == Define.SystemUserName)
            {
                throw new Exception("不能修改超级管理员信息");
            }

            Repository.Update(u => u.Id == user.Id, user => new ModelUser
            {
                UrlAvater = req.UrlAvater,
                NickName = req.Name,
                // Phone = res.Phone
            });
        }
        /// <summary>
        /// 转移客户（业务员）
        /// </summary>
        /// <param name="req"></param>
        public void ChangeChildBusinessUser(ChangeChildBusinessUserReq req)
        {
            req.Check();
            Expression<Func<ModelUser, bool>> where = p => true;
            // where = where.And(p => p.BusinessUserId == req.OldBusinessUserId);
           
            if (req.ListChildUserId != null&&req.ListChildUserId.Count>0)
            {
                var listChildUserId = req.ListChildUserId;
                where = where.And(p => listChildUserId.Contains(p.Id));
            }
            UnitWork.Update<ModelUser>(where, u=>new ModelUser
            {
                // BusinessUserId = req.NewBusinessUserId
            });
            // UnitWork.Update<ModelOrder>(whereOrder, u => new ModelOrder
            // {
            //     BusinessUserId = req.NewBusinessUserId
            // });
            UnitWork.Save();
        }

        /// <summary>
        /// 检测会员 经销商 代理 是否到期
        /// </summary>
        public  void JobCheckVipDue()
        {
            // //唯粉、经销商
            // var listUsers = UnitWork.Find<ModelUser>(p => p.UserType >= (int)xEnum.UserType.Business &&
            // p.UserType <= (int)xEnum.UserType.Agent).ToList();
            // //已过期会员
            // var listUserVipEnd = listUsers.FindAll(p => p.VipEndDate <= DateTime.Now);
            // //未过期唯粉
            // var listUserVip = listUsers.FindAll(p => p.VipEndDate > DateTime.Now&&p.UserType==(int)xEnum.UserType.Vip);
            // UnitWork.ExecuteWithTransaction(() =>
            // {
            //     //已过期会员
            //     foreach (var item in listUserVipEnd)
            //     {
            //         //vip充值记录、经销商充值记录为空，降为普通用户
            //         //清空用户上下级关系、积分
            //         //清除下级SourceUserId中的用户id
            //         var user = listUsers.FirstOrDefault(p => p.Id == item.Id);
            //         user.SourceUserId = "";
            //         user.ParentId = "";
            //         user.Points = 0;
            //         user.UserType = (int)xEnum.UserType.User;
            //         //清除下级SourceUserId中的用户id
            //         var childsList = listUsers.FindAll(p => p.SourceUserId.Contains(user.Id));
            //         childsList.ForEach(element =>
            //         {
            //             var child = listUsers.FirstOrDefault(p => p.Id == element.Id);
            //             if (child != null)
            //             {
            //                 var sourceUserIds = child.SourceUserId.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
            //                 var index = sourceUserIds.IndexOf(user.Id);
            //                 sourceUserIds.Remove(user.Id);
            //                 child.SourceUserId = string.Join(",", sourceUserIds);
            //                 child.ParentId = sourceUserIds.Count() == 0 ? "" : sourceUserIds.Last();
            //             }
            //         });
            //     }
            //     //未过期会员升、降级
            //     foreach (var item in listUserVip)
            //     {
            //        var user= listUsers.FirstOrDefault(p => p.Id == item.Id);
            //         var childs = listUsers.Count(p => p.ParentId == user.Id);
            //         if (childs >= 10)
            //         {
            //             user.UserType = (int)xEnum.UserType.AgentDealer;
            //         }
            //     }
            //     listUsers.ForEach(ele => UnitWork.Update(ele));
            //     UnitWork.Save();
            // });
        }

        // /// <summary>
        // /// 检验经销商
        // /// 检测用户等级（升级/降级）第二步
        // /// </summary>
        // public void CheckUserTypeByWbFamily()
        // {
        //     //当前年份
        //     int yearTime = DateTime.Now.Year;
        //     //查询所有有身份的用户
        //     var listUsers = UnitWork.Find<ModelUser>(p => p.UserType >= (int)xEnum.UserType.Vip && p.UserType <= (int)xEnum.UserType.Agent).ToList();
        //     //用户id
        //     var ids = listUsers.Select(p => p.Id);
        //     //经销商充值记录
        //     var listWbFamilyLog = UnitWork.Find<ModelUserWbFamilyLog>(p => p.EndDate > DateTime.Now && ids.Contains(p.UserId));
        //     //经销商充值记录
        //     var listWbMasterLog = UnitWork.Find<ModelUserWbMasterLog>(p => p.UpYear == yearTime && ids.Contains(p.UserId));
        //     //经销商
        //     var listUserWbFamily = listUsers.FindAll(p => p.UserType == (int)xEnum.UserType.AgentDealer);
        //     UnitWork.ExecuteWithTransaction(() =>
        //     {
        //         foreach (var element in listUserWbFamily)
        //         {
        //             var user = listUsers.FirstOrDefault(p => p.Id == element.Id);
        //             //用户充值经销商记录
        //             var wbFamilyLog = listWbFamilyLog.FirstOrDefault(p => p.UserId == user.Id);
        //             //旗下所有用户
        //             var childsAll = listUsers.FindAll(p => p.ParentId == user.Id);
        //             //旗下大于等于经销商用户
        //             var childsWbFamilyCount = childsAll.Count(p => p.UserType >= (int)xEnum.UserType.AgentDealer);
        //             //降级:旗下用户少于10人&&无充值经销商记录
        //             if (childsAll.Count() < Define.UpWbFamily && wbFamilyLog == null)
        //             {
        //                 user.UserType = (int)xEnum.UserType.Vip;
        //             }
        //             //升级:旗下经销商、代理商用户总数大于升级经销商所需数量
        //             else if (childsWbFamilyCount >= Define.UpWbMaster)
        //             {
        //                 //查询代理商记录
        //                 var wbMasterlog = listWbMasterLog.FirstOrDefault(p => p.UserId == user.Id);
        //                 user.UserType = (int)xEnum.UserType.Agent;
        //                 if (wbMasterlog == null)
        //                 {
        //                     //添加代理商记录
        //                     ModelUserWbMasterLog userWbMasterLog = new ModelUserWbMasterLog()
        //                     {
        //                         UserId = user.Id,
        //                         StoreId = user.StoreId,
        //                         UserCount = childsWbFamilyCount,
        //                         UpYear = yearTime,
        //                         CreateTime = DateTime.Now,
        //                     };
        //                     UnitWork.AddOrUpdate(userWbMasterLog);
        //                 }
        //                 else
        //                 {
        //                     //更新下级总数
        //                     if (wbMasterlog.UserCount < childsWbFamilyCount)
        //                     {
        //                         wbMasterlog.UserCount = childsWbFamilyCount;
        //                         UnitWork.AddOrUpdate(wbMasterlog);
        //                     }
        //                 }
        //             }
        //         }
        //         listUsers.ForEach(ele => { UnitWork.Update(ele); });
        //         UnitWork.Save();
        //     });
        // }

        // /// <summary>
        // /// 检验代理商
        // /// 检测用户等级（升级/降级）第三步
        // /// </summary>
        // public void CheckUserTypeByWbMaster()
        // {
        //     //当前年份
        //     int yearTime = DateTime.Now.Year;
        //     //查询所有有身份的用户
        //     var listUsers = UnitWork.Find<ModelUser>(p => p.UserType >= (int)xEnum.UserType.Vip && p.UserType <= (int)xEnum.UserType.Agent).ToList();
        //     //用户id
        //     var ids = listUsers.Select(p => p.Id);
        //     //经销商充值记录
        //     var listWbFamilyLog = UnitWork.Find<ModelUserWbFamilyLog>(p => p.EndDate > DateTime.Now && ids.Contains(p.UserId));
        //     //代理商记录
        //     var listWbMasterLog = UnitWork.Find<ModelUserWbMasterLog>(p => p.UpYear == yearTime && ids.Contains(p.UserId));
        //     //代理商
        //     var listWbMasterUser = listUsers.FindAll(p => p.UserType == (int)xEnum.UserType.Agent);
        //     UnitWork.ExecuteWithTransaction(() =>
        //     {
        //         //代理商
        //         foreach (var element in listWbMasterUser)
        //         {
        //             var user = listUsers.FirstOrDefault(p => p.Id == element.Id);
        //             //旗下经销商、代理商总数
        //             var childsAllCount = listUsers.Count(p => p.ParentId == user.Id && p.UserType >= (int)xEnum.UserType.AgentDealer);
        //             //查询代理商记录
        //             var wbMasterlog = listWbMasterLog.FirstOrDefault(p => p.UserId == user.Id);
        //             //降级
        //             if (childsAllCount < Define.UpWbMaster)
        //             {
        //                 //用户充值经销商记录
        //                 var wbFamilyLog = listWbFamilyLog.FirstOrDefault(p => p.UserId == user.Id);
        //                 if (wbFamilyLog != null || childsAllCount > Define.UpWbFamily)
        //                 {
        //                     user.UserType = (int)xEnum.UserType.AgentDealer;
        //                 }
        //                 else if (childsAllCount < Define.UpWbFamily)
        //                 {
        //                     user.UserType = (int)xEnum.UserType.Vip;
        //                 }
        //             }
        //             else
        //             {
        //                 //更新旗下用户最高总数
        //                 if (childsAllCount > wbMasterlog.UserCount)
        //                 {
        //                     wbMasterlog.UserCount = childsAllCount;
        //                     UnitWork.Update(wbMasterlog);
        //                 }
        //             }
        //         }
        //         listUsers.ForEach(ele => { UnitWork.Update(ele); });
        //         UnitWork.Save();
        //     });
        // }

        /// <summary>
        /// 我的团队统计（团队总人数，vip总数  经销商总数  代理商总数）
        /// </summary>
        public VmTeamStatistics MyTeamStatistics(ReqStatistic req)
        {

            var user = _auth.GetCurrentContext().User;
            
            if (!string.IsNullOrEmpty(req.UserId))
            {
                user = UnitWork.FirstOrDefault<ModelUser>(p => p.Id == req.UserId);
            }
            else
            {
                req.UserId = user.Id;
            }

            VmTeamStatistics res = new VmTeamStatistics();
            List<ModelUser> list = UnitWork.Find<ModelUser>(p => p.SourceUserId.Contains(req.UserId)
            &&!p.SourceUserId.EndsWith(req.UserId)).ToList();
            res.UserAvater = user.UrlAvater;
            res.Name = user.NickName;
            res.StrUserType = xEnum.GetEnumDescription(typeof(xEnum.UserType),user.UserType);
            res.TotalTeam = list.Count();
            res.TotalDealers = list.Count(p => p.UserType == (int)xEnum.UserType.Agent);
            res.TotalCustomer = list.Count(p => p.UserType == (int)xEnum.UserType.Customer);

            // decimal childsSaleTotalMoney = xConv.ToDecimal(UnitWork.Find<ModelUser>(p => list.Select(q => q.Id).Contains(p.ParentId)).Sum(p => p.PayGoodsTotalMoney));
            // res.TeamSaleTotalMoney = childsSaleTotalMoney + res.MySaleTotalMoney;
            res.CreateTime = user.CreateTime;
            return res;
        }

        /// <summary>
        /// 根据日期分组统计指定时间段内新增的用户
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<VmStatisticUserByDate> ListLinqUserByDate(string startTime, string endTime)
        {
            string sql = "select t1.CreateTime,count(Id) as UserCount from(select Id,cast(convert(varchar(10), CreateTime, 120) as datetime) as CreateTime from tb_User ";
            sql = sql + $"where UserType>={(int)xEnum.UserType.Customer} ";
            if (!string.IsNullOrEmpty(startTime))
            {
                sql = sql + $"and CreateTime>='{startTime}'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sql = sql + $"and CreateTime>='{endTime}'";
            }
            sql = sql + ")t1 group by t1.CreateTime";
            //return UnitWork.Query<VmStatisticUserByDate>(sql);
            return null;
        }


        /// <summary>
        /// 根据入会日期查询会员，入会日期和用户类型分组
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public IQueryable<VmStatisticUserVipByDate> ListLinqUserBusByDate(string startTime=null, string endTime=null)
        {
            string sql = "select UserType,OpenTime,COUNT(Id) as UserTotal from (select Id,UserType,cast(convert(varchar(10), OpenTime, 120) as datetime) as OpenTime " +
                $"from tb_User where UserType>={(int)xEnum.UserType.Agent} ";
            if (!string.IsNullOrEmpty(startTime))
            {
                sql = sql + $"and OpenTime>='{startTime}'";
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                sql = sql + $"and OpenTime>='{endTime}'";
            }
            sql = sql + $") t1 group by UserType,OpenTime";
            //return UnitWork.Query<VmStatisticUserVipByDate>(sql);

            return null;
        }


      

        
    }
}