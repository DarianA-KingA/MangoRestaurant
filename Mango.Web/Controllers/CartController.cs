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
        private readonly ICouponService _couponService;

        public CartController(IProductService productService, IShoppingCartService shoppingCartService, ICouponService couponService)
        {
            _productService = productService;
            _shoppingCartService = shoppingCartService;
            _couponService = couponService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }
        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shoppingCartService.ApplyCouponAsync<ResponseDto>(cartDto, accesToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return RedirectToAction(nameof(CartIndex));
        }
        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {

            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var accesToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _shoppingCartService.RemoveCouponAsync<ResponseDto>(cartDto.CartHeader.UserId, accesToken);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return RedirectToAction(nameof(CartIndex));
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

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
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
                if (!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCouponAsync<ResponseDto>(cartDto.CartHeader.CouponCode, accesToken);
                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponObj = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Result));
                        cartDto.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                    }
                }
                foreach (var details in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (details.Product.Price * details.Count);
                }
                cartDto.CartHeader.OrderTotal-=cartDto.CartHeader.DiscountTotal;
            }
            return cartDto;
        }
    }
}
