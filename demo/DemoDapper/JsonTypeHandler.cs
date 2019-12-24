using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace DemoDapper
{
    /// <summary>
    /// TypeHandler 自订Mapping逻辑使用、底层逻辑
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
   where T : class
    {
        /// <summary>
        /// 主要逻辑是在GenerateDeserializerFromMap方法Emit建立动态Mapping方法时,假如判断TypeHandler缓存有资料,以Parse方法取代原本的Set属性动作
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override T Parse(object value)
        {
            return JsonConvert.DeserializeObject<T>((string)value);
        }

        /// <summary>
        /// AddTypeHandlerImpl方法管理缓存的添加
        /// 在CreateParamInfoGenerator方法Emit建立动态AddParameter方法时,
        /// 假如该Mapping类别TypeHandler缓存内有资料,Emit添加呼叫SetValue方法动作
        /// 因为是泛型类别,取handler时可以避免了反射动作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}
