using Qs.Repository.Base;

namespace Qs.Repository.Request
{   
    /// <summary>
    /// 查询对象
    /// </summary>
    public class ReqQuFileUpload : BaseReqPage
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public int? FileType { get; set; }
        /// <summary>
        /// 上传渠道
        /// </summary>
        public int? Channel { get; set; }
        /// <summary>
        /// 数据权限(self:自己 role:同一角色 空串:所有 )
        /// </summary>
        public string Permission { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 分组Id
        /// </summary>
        public string GroupId { get; set; }
      
    }
}