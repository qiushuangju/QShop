using Newtonsoft.Json;

namespace  Common.Request.Electronic.ocr
{
    public class OcrParam
    {
     /// <summary>
     /// 图像数据，base64编码，要求base64编码后大小不超过4M,支持jpg/jpeg/png/bmp格式
     /// </summary>
     /// <value></value>
    public string image {get; set;}
    /// <summary>
    /// 是否兼容图像倾斜，true：是；false：否，默认不检测，即：false
    /// </summary>
    /// <value></value>
    public bool enableTilt {get; set;}
    /// <summary>
    /// 图片URL。image、imageUrl、pdfUrl三者必填其一，优先顺序：image>imageUrl>pdfUrl，最大长度不超过1024b，下载超时默认为2s
    /// </summary>
    /// <value></value>
    public string imageUrl {get; set;}

    /// <summary>
    /// 需要检测识别的面单元素。取值范围：barcode,qrcode,receiver,sender,bulkpen。不传或者 null 则默认为 ["barcode", "receiver", "sender"]
    /// </summary>
    /// <value></value>
    public string[] include {get; set;}
    /// <summary>
    /// PDF文件URL。image、imageUrl、pdfUrl三者必填其一，优先顺序：image>imageUrl>pdfUrl，最大长度不超过1024b，下载超时默认为2s
    /// </summary>
    /// <value></value>
    public string pdfUrl {get; set;}

     public override string ToString()
        {
            return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
        }
    }
}