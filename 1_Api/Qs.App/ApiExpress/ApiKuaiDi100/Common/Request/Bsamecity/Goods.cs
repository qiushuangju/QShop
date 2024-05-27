using Newtonsoft.Json;

namespace Common.Request.Bsamecity
{
    public class Goods
    {

        /**
        * 商品名称
        */
        public string name;
        /**
        * 商品类型，参考物品类型
        */
        public string type;
        /**
        * 商品数量
        */
        public int count;

        public override string ToString()
            {
                return JsonConvert.SerializeObject(this,Formatting.Indented,new JsonSerializerSettings(){NullValueHandling = NullValueHandling.Ignore});
            }
        }
}