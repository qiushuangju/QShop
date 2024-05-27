using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qs.App.Base;
using Qs.App.Interface;
using Qs.App.Request;
using Qs.App.Response;
using Qs.Comm;
using Qs.Comm.Extensions;
using Qs.Comm.Helpers;
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
    public class AppFileUpload : AppBaseString<ModelFileUpload, QsDBContext>
    {
        private ILogger<AppFile> _logger;
        private string _filePath;
        // private string _fileMiddlePath;
        private string _dbFilePath; //数据库中的文件路径
        private string _dbThumbnail; //数据库中的缩略图路径

        IOptions<AppSetting> _setOptions;
        /// <summary>
        /// 
        /// </summary>
        public string midPath;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppFileUpload(IOptions<AppSetting> setOptions, IUnitWork<QsDBContext> unitWork, IRepository<ModelFileUpload, QsDBContext> repository,
            ILogger<AppFile> logger, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _logger = logger;
            _setOptions = setOptions;
            midPath = setOptions.Value.UploadPath;
            _filePath = string.IsNullOrEmpty(midPath) ? AppContext.BaseDirectory :
                $"{AppContext.BaseDirectory}\\{midPath}";

        }
        
        /// <summary>
        /// 加载列表
        /// </summary>
        public TableData Load(ReqQuFileUpload req)
        {           
            var result = new TableData();
            result.Result = ListByWhere(req,true);
            result.Count = ListLinq(req).Count();
            return result;
        }
        
        /// <summary>
        /// 列表查询(不分页)
        /// </summary>
        public List<ModelFileUpload> ListByWhere(ReqQuFileUpload req, bool isPage = false)
        {
            IQueryable<ModelFileUpload> linq = ListLinq(req).OrderByDescending(p=>p.CreateTime);
            List<ModelFileUpload> list = isPage ? linq.Skip((req.Page - 1) * req.Limit).Take(req.Limit).ToList() : linq.ToList();
            return list.OrderByDescending(p => p.CreateTime).ToList();
        }

        /// <summary>
        /// listLinq
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public IQueryable<ModelFileUpload> ListLinq(ReqQuFileUpload req)
        {
            var linq = UnitWork.Find<ModelFileUpload>(p => true);
            if (!string.IsNullOrEmpty(req.Key))
            {
                linq = linq.Where(p => p.FileName.Contains(req.Key));
            }
            if (!string.IsNullOrEmpty(req.FileName))
            {
                linq = linq.Where(p => p.FileName.Contains(req.FileName));
            }
            if (xConv.ToInt(req.Channel)>0)
            {
                linq = linq.Where(p => p.Channel==req.Channel);
            }
            if (xConv.ToInt(req.FileType) > 0)
            {
                linq = linq.Where(p => p.FileType == req.FileType);
            }

            if (!string.IsNullOrEmpty(req.Permission))
            {
                var curContent = _auth.GetCurrentContext();
                var userId = curContent.User.Id;
                var role = curContent.Roles.Select(p => p.Id).ToList();
                if (req.Permission=="self")
                {
                    linq = linq.Where(p => p.CreateUserId == userId);
                }
                if (req.Permission == "role")
                {
                    linq = linq.Where(p => p.CreateUserId == userId);
                }
            }
            if (!string.IsNullOrEmpty(req.GroupId))
            {
                if (req.GroupId== "default")
                {
                    linq = linq.Where(p => p.GroupId == "");
                }
                else
                {
                    linq = linq.Where(p => p.GroupId == req.GroupId);
                }
               
            }
            return linq;
        }

        /// <summary>
        /// 批量添加附件
        /// </summary>
        /// <param name="req"></param>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public List<ModelFileUpload> Add(ReqAuFileUpload req, xEnum.FileType fileType)
        {
            var result = new List<ModelFileUpload>();
            foreach (var file in req.Files)
            {
                ModelFileUpload fileDb = Add(file, fileType, req.GroupId, xConv.ToInt(req.Channel));
                result.Add(fileDb);
            }

            return result;
        }

        /// <summary>
        /// 批量添加附件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<ModelFileUpload> Add(IFormFileCollection files, xEnum.FileType fileType)
        {

            var result = new List<ModelFileUpload>();
            foreach (var file in files)
            {
                result.Add(Add(file,fileType, "",10));
            }
            return result;
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="groupId"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public ModelFileUpload Add(IFormFile file, xEnum.FileType fileType, string groupId="", int channel = 20)
         {

             string userId = "";
             var roleId = "";
             var curContent = _auth.GetCurrentContext();
             var storeId = _auth.GetStoreId();
             if (curContent!=null)
             {
                 userId = curContent.User.Id;
                 roleId = xConv.ListToString(curContent.Roles.Select(p => p.Id).ToList());
             }
            if (file != null)
            {
                _logger.LogInformation("收到新文件: " + file.FileName);
                _logger.LogInformation("收到新文件: " + file.Length);
            }
            else
            {
                _logger.LogWarning("收到新文件为空");
            }

            if (file != null && file.Length > 0 && file.Length < 1024 * 1024 * 100)
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var data = binaryReader.ReadBytes((int) file.Length);
                    SaveFile(fileName, data, fileType);

                    var filedb = new ModelFileUpload()
                    {
                        FileName = fileName,
                        GroupId = xConv.ToString(groupId),
                        Channel = channel,
                        FileType = (int)fileType,
                        Thumbnail =(string.IsNullOrEmpty(_dbThumbnail) ? "" : $"{Define.HttpBaseApi()}/{_dbThumbnail}").Replace("\\", "/"),
                        FilePath = (fileType == xEnum.FileType.PayCert ? _dbFilePath : $"{Define.HttpBaseApi()}/{_dbFilePath}").Replace("\\", "/"),
                        FileSize = file.Length.ToInt(),
                        CreateUserName = _auth.GetUserName(),
                        Extension = Path.GetExtension(fileName),
                        CreateUserId = userId,
                        RoleIds = roleId,
                        StoreId = storeId,
                    };
                    Repository.Add(filedb);

                    return filedb;
                }
            }
            else
            {
                throw new Exception("文件过大");
            }
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="ids"></param>
        public override void Delete(string[] ids)
        {
            var files = base.Repository.Find(u => ids.Contains(u.Id)).ToList();
            for (int i = 0; i < files.Count(); i++)
            {
                var uploadPath = Path.Combine(_filePath, files[i].FilePath);
                try
                {
                    FileHelper.FileDel(uploadPath);
                    if (!string.IsNullOrEmpty(files[i].Thumbnail))
                    {
                        FileHelper.FileDel(Path.Combine(_filePath, files[i].Thumbnail));
                    }
                }
                catch (Exception e)
                {
                }

                Repository.Delete(u => u.Id == files[i].Id);
            }
        }

        /// <summary>
        /// 存储文件，如果是图片文件则生成缩略图
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBuffers"></param>
        /// <exception cref="Exception"></exception>
        private void SaveFile(string fileName, byte[] fileBuffers, xEnum.FileType fileType)
        {
            string folder = DateTime.Now.ToString("yyyyMM");
            var storeId=_auth.GetStoreId();
            var ext = Path.GetExtension(fileName).ToLower();
            var fileNameNotExt = fileName.Replace($"{ext}","");
            string newName = GenerateId.GenerateOrderNumber() + ext;
            //判断文件是否为空
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("文件名不能为空");
            }

            //判断文件是否为空
            if (fileBuffers.Length < 1)
            {
                throw new Exception("文件不能为空");
            }
            if (fileType == xEnum.FileType.PayCert)//支付证书
            {
                 midPath = _setOptions.Value.UploadPathCart;
                folder = "";
                newName = $"{fileNameNotExt}_{storeId}{ext}";
                _filePath = string.IsNullOrEmpty(midPath) ? AppContext.BaseDirectory : $"{AppContext.BaseDirectory}\\{midPath}";
            }

            var uploadPath = Path.Combine(_filePath, folder);
            _logger.LogInformation("文件写入：" + uploadPath);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            using (var fs = new FileStream(Path.Combine(uploadPath, newName), FileMode.Create))
            {
                fs.Write(fileBuffers, 0, fileBuffers.Length);
                fs.Close();

                //生成缩略图
                if (ext.Contains(".jpg") || ext.Contains(".jpeg") || ext.Contains(".png") || ext.Contains(".bmp") ||
                    ext.Contains(".gif"))
                {
                    string thumbnailName = GenerateId.GenerateOrderNumber() + ext;
                    ImgHelper.MakeThumbnail(Path.Combine(uploadPath, newName), Path.Combine(uploadPath, thumbnailName));
                    _dbThumbnail = $"{midPath}\\{Path.Combine(folder, thumbnailName)}";
                }
                _dbFilePath = $"{midPath}\\{Path.Combine(folder, newName)}";
            }
        }

       
        /// <summary>
        /// 移动到分组
        /// </summary>
        public void MoveToGroup(ReqMoveGroup req)
        {
            // var file = UnitWork.FirstOrDefault<ModelFileUpload>(p => req.ListId.Contains(p.Id));
            // file.GroupId = req.GroupId;
            UnitWork.Update<ModelFileUpload>(p=> req.ListId.Contains(p.Id),p=>new ModelFileUpload()
            {
                  GroupId = req.GroupId
            });
            UnitWork.Save();
        }
    }
}