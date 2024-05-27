using System.Collections.Generic;
using System.Linq;
using Qs.App.Base;
using Qs.App.Interface;
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
    ///  App
    /// </summary>
    public class AppRefundReason : AppBaseString<ModelRefundReason, QsDBContext>
    {
        private AppRevelanceManager _revelanceApp;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppRefundReason(IUnitWork<QsDBContext> unitWork, IRepository<ModelRefundReason, QsDBContext> repository,
            AppRevelanceManager app,DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _revelanceApp = app;
        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuRefundReason req)
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
        public List<ModelRefundReason> ListByWhere(ReqQuRefundReason req, bool isPage = false)
        {
            IQueryable<ModelRefundReason> linq = ListLinq(req);
            List<ModelRefundReason> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list;
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelRefundReason> ListLinq(ReqQuRefundReason req)
        {
            var linq = UnitWork.Find<ModelRefundReason>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.Value.Contains(req.Key));
            }
            
            return linq;
        }  

        /// <summary>
        /// 新增或修改
        /// </summary>
        public void AddOrUpdate(ReqAuRefundReason req)
        {
            var model = xConv.CopyMapper<ModelRefundReason, ReqAuRefundReason>(req);
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
        /// 父Id下所有子节点
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<ResDictionary> ListTree(string parentId)
        {
            List<ResDictionary> list = new List<ResDictionary>();
            List<ModelRefundReason> listClassDb = Repository.Find(p => p.IsDelete == (int)xEnum.YesOrNo.No).ToList();
            List<ModelRefundReason> root = listClassDb.Where(p => p.ParentId == parentId).ToList();
            foreach (var sysClass in root)
            {
                //实体转化 
                ResDictionary parentItem = xConv.CopyMapper<ResDictionary, ModelRefundReason>(sysClass);
                //获取子级
                GetChildren(ref parentItem, listClassDb);
                list.Add(parentItem);
            }
            return list;
        }

        /// <summary>
        /// 获取子级
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="allList"></param>
        /// <returns></returns>
        private static void GetChildren(ref ResDictionary parent, List<ModelRefundReason> allList)
        {
            foreach (ModelRefundReason item in allList)
            {
                if (item.ParentId == parent.Id)
                {
                    //实体转化
                    SubDic child = xConv.CopyMapper<SubDic, ModelRefundReason>(item);
                    if (parent.ListSon == null)
                    {
                        parent.ListSon = new List<SubDic>();
                    }
                    //添加到父级的Children中
                    parent.ListSon.Add(child);
                }
            }
        }

    }
}