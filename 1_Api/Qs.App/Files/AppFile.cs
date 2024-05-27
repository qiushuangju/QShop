using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Qs.Comm;
using Microsoft.AspNetCore.Http;
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

namespace Qs.App
{
    /// <summary>
    /// 文件管理
    /// </summary>
    public class AppFile : AppBaseString<ModelFileUpload,QsDBContext>
    {
        private ILogger<AppFile> _logger;
        private string _filePath;
        // private string _fileMiddlePath;
        private string _dbFilePath; //数据库中的文件路径
        private string _dbThumbnail; //数据库中的缩略图路径
        private string _midPath = "";

        public AppFile(IOptions<AppSetting> setOptions, IUnitWork<QsDBContext> unitWork, IRepository<ModelFileUpload, QsDBContext> repository,
            ILogger<AppFile> logger, DbExtension dbExtension, IAuth auth) : base(unitWork, repository, dbExtension, auth)
        {
            _logger = logger;
            _midPath = setOptions.Value.UploadPath;
            _filePath = string.IsNullOrEmpty(_midPath) ? AppContext.BaseDirectory:
                $"{AppContext.BaseDirectory}\\{_midPath}" ;
           
        }

        /// <summary>
        /// 加载附件列表
        /// </summary>
        public async Task<TableData> Load(QueryFileListReq request)
        {
            var result = new TableData();
            var objs = UnitWork.Find<ModelFileUpload>(null);
            if (!string.IsNullOrEmpty(request.Key))
            {
                objs = objs.Where(u => u.FileName.Contains(request.Key) || u.FilePath.Contains(request.Key));
            }

            result.Result = objs.OrderByDescending(u => u.CreateTime)
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit);
            result.Count = objs.Count();
            return result;
        }
        
        /// <summary>
        /// 批量添加附件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<ModelFileUpload> Add(IFormFileCollection files)
        {
            //if (!_auth.CheckLogin())
            //{
            //    throw new Exception("必需登录才能上传附件");
            //}
            
            var result = new List<ModelFileUpload>();
            foreach (var file in files)
            {
                result.Add(Add(file));
            }

            return result;
        }

    

        public ModelFileUpload Add(IFormFile file)
        {
            if (file != null)
            {
                _logger.LogInformation("收到新文件: " + file.FileName);
                _logger.LogInformation("收到新文件: " + file.Length);
            }
            else
            {
                _logger.LogWarning("收到新文件为空");
            }

            if (file != null && file.Length > 0 && file.Length < 1024*1024*100)
            {
                using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var data = binaryReader.ReadBytes((int) file.Length);
                    SaveFile(fileName, data);

                    var filedb = new ModelFileUpload
                    {
                        FilePath = $"{Define.HttpBaseApi()}/{_dbFilePath}".Replace("\\", "/"),
                        Thumbnail = $"{Define.HttpBaseApi()}/{_dbThumbnail}".Replace("\\", "/"),
                        FileName = fileName,
                        FileSize = file.Length.ToInt(),
                        CreateUserName = _auth.GetUserName(),
                        FileType = (int)xEnum.FileType.Image,
                        Extension = Path.GetExtension(fileName)
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

                Repository.Delete(u =>u.Id == files[i].Id);
            }
        }

        /// <summary>
        /// 存储文件，如果是图片文件则生成缩略图
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileBuffers"></param>
        /// <exception cref="Exception"></exception>
        private void SaveFile(string fileName, byte[] fileBuffers)
        {
            string folder = DateTime.Now.ToString("yyyyMM");

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

            var uploadPath = Path.Combine(_filePath, folder);
            _logger.LogInformation("文件写入：" + uploadPath);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            var ext = Path.GetExtension(fileName).ToLower();
            string newName = GenerateId.GenerateOrderNumber() + ext;

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
                    _dbThumbnail = $"{_midPath}\\{Path.Combine(folder, thumbnailName)}";
                }


                _dbFilePath = $"{_midPath}\\{Path.Combine(folder, newName)}";
            }
        }
    }
}