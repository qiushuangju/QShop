using Common.Request;
using Common.Request.cloud;
using Utils;
using Common;
/// <summary>
/// 订单导入
/// </summary>
public static class CloudPrint
{

    /// <summary>
    /// 自定义打印接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string custom(BaseReq<CustomParam> param){
        param.method = ApiInfoConstant.CLOUD_PRINT_CUSTOM_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doGet(HttpUtils.buildUrl(ApiInfoConstant.CLOUD_PRINT_URL,param));
        return result;
    }

    /// <summary>
    /// 附件打印接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string attachment(BaseReq<AttachmentParam> param,string filePath,string filename){
        param.method = ApiInfoConstant.CLOUD_PRINT_ATTACHMENT_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostMultipartFormData<AttachmentParam>(HttpUtils.buildUrl(ApiInfoConstant.CLOUD_PRINT_URL,param),filePath,filename);
        return result;
    }

    /// <summary>
    /// 硬件状态接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string devStatus(BaseReq<DevStatusParam> param){
        param.method = ApiInfoConstant.CLOUD_PRINT_DEV_STATUS;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doGet(HttpUtils.buildUrl(ApiInfoConstant.CLOUD_PRINT_URL,param));
        return result;
    }

    /// <summary>
    /// 复打
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string printOld(BaseReq<PrintOldParam> param){
        param.method = ApiInfoConstant.CLOUD_PRINT_OLD_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.ELECTRONIC_ORDER_PIC_URL,request);
        return result;
    }

    /// <summary>
    /// 指令打印接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string commandPrint(BaseReq<CommandParam> param){
        param.method = ApiInfoConstant.CLOUD_PRINT_COMMAND;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.ELECTRONIC_ORDER_PRINT_URL,request);
        return result;
    }

    /// <summary>
    /// 发货单接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string billParcels<T>(BaseReq<ParcelsBillsParam<T>> param){
        param.method = ApiInfoConstant.BILL_PARCELS_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.BILL_PARCELS_URL,request);
        return result;
    }
}