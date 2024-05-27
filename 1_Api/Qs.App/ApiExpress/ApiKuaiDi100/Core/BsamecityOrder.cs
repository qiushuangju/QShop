using Common.Request;
using Common.Request.Bsamecity;
using Utils;
using Common;
/// <summary>
/// 同城寄件
/// </summary>
public static class BsamecityOrder
{

    /// <summary>
    /// 预下单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string price(BaseReq<BsamecityOrderParam> param){
        
        param.method = ApiInfoConstant.BSAMECITY_EXPRESS_PRICE;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);

        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.BSAMECITY_EXPRESS_URL,request);
        return result;
    }

    /// <summary>
    /// 下单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string order(BaseReq<BsamecityOrderParam> param){
        
        param.method = ApiInfoConstant.BSAMECITY_EXPRESS_ORDER;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);

        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.BSAMECITY_EXPRESS_URL,request);
        return result;
    }

    /// <summary>
    /// 预取消
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string precancel(BaseReq<BsamecityCancelParam> param){
        
        param.method = ApiInfoConstant.BSAMECITY_EXPRESS_PRECANCEL;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);

        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.BSAMECITY_EXPRESS_URL,request);
        return result;
    }

    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string cancel(BaseReq<BsamecityCancelParam> param){
        
        param.method = ApiInfoConstant.BSAMECITY_EXPRESS_CANCEL;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);

        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.BSAMECITY_EXPRESS_URL,request);
        return result;
    }

    /// <summary>
    /// 加小费
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string addfee(BaseReq<BsamecityAddfeeParam> param){
        
        param.method = ApiInfoConstant.BSAMECITY_EXPRESS_ADDFEE;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);

        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.BSAMECITY_EXPRESS_URL,request);
        return result;
    }
}