using Mango.Service.ShoppingCartAPI.Models.DTO;
using Mango.Services.ShoppingCartAPi.Models.DTO;
using Newtonsoft.Json;

namespace Mango.Service.ShoppingCartAPI.Repository
{
    public class CouponReposiroty : ICouponReposiroty
    {
        private readonly HttpClient client;
        public CouponReposiroty(HttpClient client)
        {
            this.client = client;
        }
        public async Task<CouponDto> GetCoupon(string couponName)
        {
            var response = await client.GetAsync($"api/coupon/{couponName}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            return new CouponDto();
        }
    }
}
