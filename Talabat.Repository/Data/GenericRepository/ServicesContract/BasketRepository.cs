using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.GenericRepository.RepositoriesContract;

namespace Talabat.Repository.Data.GenericRepository.ServicesContract
{
    public class BasketRepository : IBasketRepository
    {
        public IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasket(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var basket = await _database.StringGetAsync(BasketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }



        public async Task<CustomerBasket?> UpdateBaseket(CustomerBasket customerBasket)
        {
            var createorupdate = await _database.StringSetAsync(customerBasket.Id, JsonSerializer.Serialize(customerBasket), TimeSpan.FromDays(30));
            if (createorupdate is false) { return null; }
            return await GetBasketAsync(customerBasket.Id);
        }
    }
}
