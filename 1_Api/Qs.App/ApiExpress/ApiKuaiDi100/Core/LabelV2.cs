using Common.Request;
using Common.Request.Label;
using Utils;
using Common;
/// <summary>
/// 商家寄件（官方快递）
/// </summary>
public static class LabelV2
{

    /// <summary>
    /// 下单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string order(BaseReq<OrderParam> param){
        
        param.method = ApiInfoConstant.ORDER;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.NEW_TEMPLATE_URL,request);
        return result;
    }

    /// <summary>
    /// 复打
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string repeatPrint(BaseReq<RepeatPrintParam> param){
        
        param.method = ApiInfoConstant.CLOUD_PRINT_OLD_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.NEW_TEMPLATE_URL,request);
        return result;
    }

    /// <summary>
    /// 自定义打印
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string customPrint(BaseReq<CustomPrintParam> param){
        
        param.method = ApiInfoConstant.CUSTOM;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.NEW_TEMPLATE_URL,request);
        return result;
    }
}