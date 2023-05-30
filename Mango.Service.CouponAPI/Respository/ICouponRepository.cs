using Mango.Service.CouponAPI.Models.DTO;

namespace Mango.Service.CouponAPI.Respository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCodeAsync(string couponCode);


    }
}
