using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;

namespace API.Controllers
{
    public class CartController(ICartService basketService) : ControllerBase
    {
        
            [HttpGet]
            public async Task<ActionResult<Cart>> GetBasket(string id)
            {
                var basket = await basketService.GetBasketAsync(id);

                return basket ?? new Cart { Id = id };
            }

            [HttpPost]
            public async Task<ActionResult<Cart>> UpdateBasket(Cart basket)
            {
                var updatedBasket = await basketService.SetBasketAsync(basket);

                if (updatedBasket == null)
                    return BadRequest("Problem updating basket");

                return Ok(updatedBasket);
            }

            [HttpDelete]
            public async Task<IActionResult> DeleteBasket(string id)
            {
                var deleted = await basketService.DeleteBasketAsync(id);

                if (!deleted)
                    return BadRequest("Problem deleting basket");

                return Ok();
            }
        }
    
}
