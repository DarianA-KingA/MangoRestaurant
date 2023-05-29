using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public CartController(IProductService productService, IShoppingCartService shoppingCartService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }
        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shoppingCartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accesToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        private async Task<CartDto> LoadCartDtoBaseOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shoppingCartService.GetCartByUserIdAsync<ResponseDto>(userId, accesToken);

            CartDto cartDto = new();
            if (response != null && response.IsSuccess) 
            {
                cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            if (cartDto.CartHeader != null)
            {
                foreach (var details in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (details.Product.Price * details.Count);
                }
            }
            return cartDto;
        }
    }
}
