using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ShoppingCartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _httpClientFactory = clientFactory;
        }
        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }
        public async Task<T> Get<T>()
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = new darian(),
                Url = SD.ShoppingCartAPIBase + "/api/cart/Test",
                AccessToken = ""
            });
        }
        public class darian
        {
            public string nombre { get; set; }
        }
        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "api/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}
