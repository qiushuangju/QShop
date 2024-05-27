
using Utils;
using Common;
using Common.Request.Electronic.Html;

public class PrintHtml{


    public static string query(PrintHtmlReq param){
        
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.ELECTRONIC_ORDER_HTML_URL,request);
        return result;
    }
}   