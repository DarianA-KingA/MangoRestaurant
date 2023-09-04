

using Mango.Service.ShoppingCartAPI.Models.DTO;

namespace Mango.Service.ShoppingCartAPI.Repository
{
    public interface ICouponReposiroty
    {
        Task<CouponDto> GetCoupon(string couponName);
    }
}
