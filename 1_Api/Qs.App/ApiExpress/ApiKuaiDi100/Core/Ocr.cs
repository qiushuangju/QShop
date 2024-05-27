using Common;
using Utils;
using Common.Request;
using Common.Request.Electronic.ocr;

public static class Ocr
{
    public static string ocr(BaseReq<OcrParam> param)
    {

        var request = ObjectToDictionaryUtils.ObjectToMap(param);
          if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.OCR_URL,request);
        return result;
    }
}
