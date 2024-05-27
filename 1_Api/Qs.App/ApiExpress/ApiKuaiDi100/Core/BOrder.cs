using Common.Request.Border;
using Common.Request;
using Utils;
using Common;
using System;
/// <summary>
/// 商家寄件（优选）接口已弃用
/// </summary>
[Obsolete]
public static class Border
{
    /// <summary>
    /// 查询运力
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public static string transportCapacity(BaseReq<BOrderQueryParam> param){

        param.method = ApiInfoConstant.B_ORDER_QUERY_TRANSPORT_CAPACITY_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.B_ORDER_URL,request);
        return result;
    }

    /// <summary>
    /// 下单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string order(BaseReq<BOrderParam> param){
        
        param.method = ApiInfoConstant.B_ORDER_SEND_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.B_ORDER_URL,request);
        return result;
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string getCode(BaseReq<BOrderGetCodeParam> param){
        
        param.method = ApiInfoConstant.B_ORDER_CODE_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.B_ORDER_URL,request);
        return result;
    }

    /// <summary>
    /// 取消下单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string cancel(BaseReq<BOrderCancelParam> param){
        
        param.method = ApiInfoConstant.B_ORDER_CANCEL_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.B_ORDER_URL,request);
        return result;
    }
}