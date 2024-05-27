using Common.Request;
using Common.Request.internationalshipment;
using Utils;
using Common;
/// <summary>
/// 国际电子面单API
/// </summary>
public static class InternationalShipment
{

    /// <summary>
    /// 获取面单
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
     public static string getLabel(BaseReq<ShipmentReq> param){
        
    
        var request = ObjectToDictionaryUtils.ObjectToMap(param);
        
        if(request == null){
            return null;
        }
        var result = HttpUtils.doPostForm(ApiInfoConstant.INTERNATIONAL_SHIPMENT_URL,request);
        return result;
    }


}