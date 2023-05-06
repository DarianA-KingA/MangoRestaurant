using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;

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
            var response = await _productService.GetAllProductAsync<ResponseDto>();
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
                var response = await _productService.CreateProductAsync<ResponseDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> UpdateProduct(int productId)
        {

            var response = await _productService.GetAllProductByIdAsync<ResponseDto>(productId);
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
                var response = await _productService.UpdateProductAsync<ResponseDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
        public async Task<IActionResult> DeleteProduct(int productId)
        {

            var response = await _productService.GetAllProductByIdAsync<ResponseDto>(productId);
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
        public async Task<IActionResult> DeleteProduct(ProductDto model)
        {
            var response = await _productService.DeleteProductAsync<ResponseDto>(model.ProductId);
            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(ProductIndex));
            }
            return View(model);
        }
    }
}

