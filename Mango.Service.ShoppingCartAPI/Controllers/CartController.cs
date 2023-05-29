﻿using Mango.Service.ShoppingCartAPI.Models.DTO;
using Mango.Service.ShoppingCartAPI.Repository;
using Mango.Services.ShoppingCartAPi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDto _response;
        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            this._response = new ResponseDto();
        }
        [HttpGet("GetCart/{userId}")]

        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await _cartRepository.GetCartByUserIdAsync(userId);
                _response.Result = cartDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartRepository.CreateUpdteCartAsync(cartDto);
                _response.Result = cartDt;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]

        public async Task<object> UpdateCart(CartDto cartDto)
        {
            try
            {
                CartDto cartDt = await _cartRepository.CreateUpdteCartAsync(cartDto);
                _response.Result = cartDt;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;


        }

        [HttpPost("RemoveCart")]

        public async Task<object> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
               bool isSuccess = await _cartRepository.RemoveFromCartAsync(cartDetailsId);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;

        }

        [HttpPost("Test")]
        public object Test(darian model)
        {
            return _response;
        }
        public class darian
        {
            public string nombre { get; set; }
        }
    }
}