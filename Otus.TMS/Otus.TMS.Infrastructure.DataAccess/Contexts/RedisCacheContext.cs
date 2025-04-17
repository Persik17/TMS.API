using StackExchange.Redis;

namespace Otus.TMS.Infrastructure.DataAccess.Contexts
{
    public class RedisCacheContext
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheContext(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public IDatabase Database => _redis.GetDatabase();
    }
}
