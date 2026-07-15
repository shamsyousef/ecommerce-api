using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICartService
    {
        Task<Cart?> GetBasketAsync(string id);

        Task<Cart?> SetBasketAsync(Cart basket);

        Task<bool> DeleteBasketAsync(string id);
    }
}
