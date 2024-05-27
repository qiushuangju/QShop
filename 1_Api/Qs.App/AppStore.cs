using System;
using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Repository;
using Qs.Repository.Base;
using Qs.Repository.Domain;
using Qs.Repository.Interface;
using Qs.Repository.Request;
using Qs.Repository.Response;

namespace Qs.App
{
    /// <summary>
    /// 店铺表 App
    /// </summary>
    public class AppStore : AppBaseString<ModelStore, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppStore(IUnitWork<QsDBContext> unitWork, IRepository<ModelStore, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuStore req)
        {           
            //var loginContext = _auth.GetCurrentContext();
            var result = new TableData();
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ResStore> ListByWhere(ReqQuStore req, bool isPage = false)
        {
            IQueryable<ModelStore> linq = ListLinq(req);
            List<ModelStore> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            var listFileId = list.Select(p => p.LogoImageId).ToList();
            var listFile = UnitWork.Find<ModelFileUpload>(p => listFileId.Contains(p.Id)).ToList();
            List<ResStore> listRes = new List<ResStore>();
            foreach (var item in list)
            {
                listRes.Add(ResStore.ToView(item, listFile)); 
            }
            return listRes;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelStore> ListLinq(ReqQuStore req)
        {
            var linq = UnitWork.Find<ModelStore>(p => true);
            var storeId = _auth.GetStoreId();
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.StoreName.Contains(req.Key));
            }
            if (!string.IsNullOrEmpty(storeId))
            {
                linq = linq.Where(p => p.StoreId.Contains(storeId));
            }
            return linq;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddStore(ReqAuStore req)
        {
            var model = xConv.CopyMapper<ModelStore, ReqAuStore>(req);
            model.Id = xConv.NewGuid();
            model.StoreId = model.Id;
           int countDb=  UnitWork.Count<ModelUser>(p => p.NickName == req.StoreUserName);
           if (countDb>0)
           {
               throw new Exception($"用户已存在!");
           }
            ModelUser user = new ModelUser();
            user.Id = xConv.NewGuid();
            user.UserType = (int) xEnum.UserType.StoreAdmin;
            user.NickName = req.StoreUserName;
            user.Account = req.StoreUserName;
            user.StoreId = model.Id;
            user.CreateTime=DateTime.Now;
            user.Password = xConv.MD5Encoding(req.StorePwd, xConv.ToStrDateTime(user.CreateTime));

            Relevance relevance = new Relevance();
            relevance.FirstId = user.Id;
            relevance.SecondId = Define.RoleIdStoreAdmin;
            relevance.Key = Define.UserRole;
            relevance.Status = (int) xEnum.ComStatus.Normal;
            relevance.OperateTime = DateTime.Now;
            relevance.OperatorId = model.Id;

            UnitWork.Add(model);
            UnitWork.Add(user);
            UnitWork.Add(relevance);
            UnitWork.Save();
        }

        /// <summary>
        /// 修改商城(平台管理员)
        /// </summary>
        public void UpdateStoreInfo(ReqUpdateStoreInfo req)
        {
            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            Repository.Update(p => p.Id == req.Id, u => new ModelStore()
            {
                CompanyName = req.CompanyName,
                LinkMan = req.LinkMan,
                Phone = req.Phone,
            });
        }

        /// <summary>
        /// 修改商城(店铺自己)
        /// </summary>
        public void UpdateStore(ReqUpdateStore req)
        {

            var model = Repository.FirstOrDefault(p => p.Id == req.Id);
            Repository.Update(p => p.Id == req.Id, u => new ModelStore()
            {
                StoreName = req.StoreName,
                Describe = req.Describe,
                LogoImageId = req.LogoImageId
            });
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        public ResStore GetDetail(string id)
        {
           var model= Repository.FirstOrDefault(p => p.Id == id);
           var file = UnitWork.FirstOrDefault<ModelFileUpload>(p => p.Id == model.LogoImageId);
           ResStore res = ResStore.ToView(model, new List<ModelFileUpload>(){file});
           return res;
        }

    }
}