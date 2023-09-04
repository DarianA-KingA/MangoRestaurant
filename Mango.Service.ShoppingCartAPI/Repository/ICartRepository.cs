using Mango.Service.ShoppingCartAPI.Models.DTO;

namespace Mango.Service.ShoppingCartAPI.Repository
{
    public interface ICartRepository
    {
       Task<CartDto> GetCartByUserIdAsync(string userId);
       Task<CartDto> CreateUpdteCartAsync(CartDto cartDto);
       Task<bool> RemoveFromCartAsync(int cartDetailsId);
       Task<bool> Applycoupon(string userId, string couponCode);
       Task<bool> Removecoupon(string userId);
       Task<bool> ClearCartAsync(string userId);
    }
}
