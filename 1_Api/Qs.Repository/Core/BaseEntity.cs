namespace Qs.Repository.Core
{
    public abstract class BaseEntity
    {
        /// <summary>
        /// 判断主键是否为空，常用做判定操作是【添加】还是【编辑】
        /// </summary>
        /// <returns></returns>
        public abstract bool KeyIsNull();

        /// <summary>
        /// 创建默认的主键值
        /// </summary>
        public abstract void GenerateDefaultKeyVal();

        public BaseEntity()
        {
            // 创建实体增加ID
            // if (KeyIsNull())
            // {
            //     GenerateDefaultKeyVal();
            // }
        }
    }
}