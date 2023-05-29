using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();
            var accesstoken = await HttpContext.GetTokenAsync("access_token");

            var response = await _productService.GetAllProductAsync<ResponseDto>(accesstoken);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
        public async Task<IActionResult> CreateProduct()
        {

            return View(new ProductDto());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.CreateProductAsync<ResponseDto>(model, accesstoken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> UpdateProduct(int productId)
        {
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accesstoken);
            if (response != null && response.IsSuccess)
            {
                var test = Convert.ToString(response.Result);
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.UpdateProductAsync<ResponseDto>(model, accesstoken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var accesstoken = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId, accesstoken);
            if (response != null && response.IsSuccess)
            {
                var test = Convert.ToString(response.Result);
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var accesstoken = await HttpContext.GetTokenAsync("access_token");
                var response = await _productService.DeleteProductAsync<ResponseDto>(model.ProductId, accesstoken);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
    }
}

