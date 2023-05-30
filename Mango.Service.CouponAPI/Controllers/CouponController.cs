using Mango.Service.CouponAPI.Models.DTO;
using Mango.Service.CouponAPI.Respository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDto _response;
        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            this._response = new();
        }
        [HttpGet("{code}")]
        [Authorize]
        public async Task<object> GetDiscountForCode(string CouponCode)
        {
            try
            {
                CouponDto couponDto = await _couponRepository.GetCouponByCodeAsync(CouponCode);
                _response.Result = couponDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
