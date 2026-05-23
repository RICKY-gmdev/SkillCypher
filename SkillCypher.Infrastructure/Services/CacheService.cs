using System.Text.Json;
using StackExchange.Redis;
using SkillCypher.Core.Interfaces;

namespace SkillCypher.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _redis;
        public CacheService(IConnectionMultiplexer connection)
        {
            _redis = connection.GetDatabase();
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _redis.StringGetAsync(key);

            if(value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(value!);
        }
        public async Task SetAsync<T>(
            string key,
            T value,
            TimeSpan? expiry = null)
        {
            var serialized = JsonSerializer.Serialize(value);
            await _redis.StringSetAsync(
                key,
                serialized,
                expiry ?? TimeSpan.FromMinutes(5)
            );
        }

        public async Task RemoveAsync(string key)
        {
            await _redis.KeyDeleteAsync(key);
        }
    }
}