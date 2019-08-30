using EasyCaching.Core;

namespace Core.Contract
{
   public abstract class RedisCachingContract
    {
        public readonly IRedisCachingProvider _redisCachingProvider;
        public RedisCachingContract(IEasyCachingProviderFactory factory)
        {
            _redisCachingProvider = factory.GetRedisProvider("redis2");
        }
    }
}
