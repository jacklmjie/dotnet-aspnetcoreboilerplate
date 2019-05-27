using EasyCaching.Core;

namespace Core.Service
{
   public abstract class RedisCachingService
    {
        public readonly IRedisCachingProvider _redisCachingProvider;
        public RedisCachingService(IEasyCachingProviderFactory factory)
        {
            _redisCachingProvider = factory.GetRedisProvider("redis2");
        }
    }
}
