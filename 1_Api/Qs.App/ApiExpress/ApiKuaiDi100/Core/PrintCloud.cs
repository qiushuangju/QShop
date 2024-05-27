
using Utils;
using Common;
using Common.Request;
using Common.Request.Electronic;
using Common.Request.Electronic.Print;

public class PrintCloud{


    public static string query(PrintCloudReq param){

        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.ELECTRONIC_ORDER_PRINT_URL,request);
        return result;
    }

    /// <summary>
    /// 面单取消
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string cancel(BaseReq<CancelPrint> param){
        param.method = ApiInfoConstant.CANCEL_METHOD;
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.ELECTRONIC_ORDER_HTML_URL,request);
        return result;
    }
}   