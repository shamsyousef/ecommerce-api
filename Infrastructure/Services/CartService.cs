using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {

        private readonly IDatabase _database;

        public CartService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<Cart?> GetBasketAsync(string id)
        {
            var data = await _database.StringGetAsync(id);

            return data.IsNullOrEmpty
                ? null
                : JsonSerializer.Deserialize<Cart>(data!);
        }

        public async Task<Cart?> SetBasketAsync(Cart basket)
        {
            var created = await _database.StringSetAsync(
                basket.Id,
                JsonSerializer.Serialize(basket),
                TimeSpan.FromDays(30));

            if (!created)
                return null;

            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }
    }
}
