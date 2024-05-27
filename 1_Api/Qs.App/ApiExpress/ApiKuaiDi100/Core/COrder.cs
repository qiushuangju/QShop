using Common.Request.Corder;
using Common.Request;
using Utils;
using Common;
using System;
/// <summary>
/// c端寄件
/// </summary>
public static class Corder
{
   
    /// <summary>
    /// 下单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string order(BaseReq<COrderParam> param){
        
        param.method = ApiInfoConstant.CORDER;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.C_ORDER_URL,request);
        return result;
    }



    /// <summary>
    /// C端寄件下单取消接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string cancel(BaseReq<COrderCancelParam> param){
        
        param.method = ApiInfoConstant.CANCEL_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.C_ORDER_URL,request);
        return result;
    }

    /// <summary>
    /// C端寄件价格查询接口
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string queryPrice(BaseReq<COrderQueryPriceParam> param){
        
        param.method = ApiInfoConstant.PRICE;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.C_ORDER_URL,request);
        return result;
    }
}