using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.GenericRepository.RepositoriesContract
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string BasketId);
        public Task<CustomerBasket?> UpdateBaseket(CustomerBasket customerBasket);
        public Task<bool> DeleteBasket(string BasketId);

    }
}
