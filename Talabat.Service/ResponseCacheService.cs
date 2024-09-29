using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Repository.Data.Repositories;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Talabat.Service
{
    public class ResponseCacheService : ICacheResponseService
    {
        private readonly IDatabase database;
        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            database =redis.GetDatabase();
        }
        public async Task CacheResponse(string cachekey, object response, TimeSpan timetolive)
        {
            if (response == null) return;
            var options=new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
           var serializedjso=JsonSerializer.Serialize(response,options);
            await database.StringSetAsync(cachekey, serializedjso);
        }

        public async Task<string?> Getcachedresponse(string cachekey)
        {
            var cachedresponse=await database.StringGetAsync(cachekey);
            if (cachedresponse.IsNullOrEmpty) return null;
            return cachedresponse;
        }
    }
}
