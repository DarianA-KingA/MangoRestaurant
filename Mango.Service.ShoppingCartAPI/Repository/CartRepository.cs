using AutoMapper;
using Mango.Service.ShoppingCartAPI.Context;
using Mango.Service.ShoppingCartAPI.Models;
using Mango.Service.ShoppingCartAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationContext _db;
        private readonly IMapper _mapper;
        public CartRepository(ApplicationContext db, IMapper mapper) 
        {
            _db = db;
            _mapper = mapper;   
        }
        
        public async Task<CartDto> CreateUpdteCartAsync(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);
            //check if product exits in database, if not create it
            var productInDb = await _db.Products
                .FirstOrDefaultAsync(p => p.ProductId == cartDto.CartDetails.FirstOrDefault()
                .ProductId);
            if (productInDb == null) 
            {
                _db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await _db.SaveChangesAsync();
                //cart.CartDetails.FirstOrDefault().Product = null;
            }
            //check if header is null
            var cartHeaderFromDb = await _db.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(c=>c.UserId == cart.CartHeader.UserId);
            if (cartHeaderFromDb == null)
            {
                //create header and details
                _db.CartHeaders.Add(cart.CartHeader);
                await _db.SaveChangesAsync();
                //when you save, the object get returns with its id
                cart.CartDetails.FirstOrDefault().CartDetailsId = cart.CartHeader.CartHeaderId;

                //se lleva a cabo este paso, porque sino entity framework tratara de agregar ese producto que ya se agrego mas arriba al ver que no es null
                cart.CartDetails.FirstOrDefault().Product = null;
                _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _db.SaveChangesAsync();
            }
            //if header is not null
            else
            {
                //cheack if details has the same product, in case that the user just wants to update de quantity
                var cartDetailsFromDb = await  _db.CartDetails.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                    c.CartDetailsId == cartHeaderFromDb.CartHeaderId);
                if (cartDetailsFromDb == null)
                {
                    //create deatils
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;

                    _db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
                else
                {
                    //update the count / cart details
                    cart.CartDetails.FirstOrDefault().Product = null;
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                    _db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await _db.SaveChangesAsync();
                }
            }

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var cartHeaderFromDb = await _db.CartHeaders.FirstOrDefaultAsync( u=>u.UserId == userId);
            if (cartHeaderFromDb != null)
            {
                _db.CartDetails.RemoveRange(
                    _db.CartDetails.Where(c=>c.CartHeaderId == cartHeaderFromDb.CartHeaderId));
                _db.CartHeaders.Remove(cartHeaderFromDb);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<CartDto> GetCartByUserIdAsync(string userId)
        {
            CartDto cart = new()
            {
                CartHeader= await _db.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId)
            };

            cart.CartDetails =  _db.CartDetails.Where(c => c.CartHeaderId == cart.CartHeader.CartHeaderId).Include(c=>c.Product);

            return _mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveFromCartAsync(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await _db.CartDetails
                .FirstOrDefaultAsync(c => c.CartDetailsId == cartDetailsId);

                int totalCountOfCartItems = _db.CartDetails
                    .Where(c => c.CartHeaderId == cartDetails.CartHeaderId).Count();

                _db.CartDetails.Remove(cartDetails);
                if (totalCountOfCartItems == 1)
                {
                    var cartHeaderToRemove = await _db.CartHeaders
                        .FirstOrDefaultAsync(c => c.CartHeaderId == cartDetails.CartHeaderId);
                    _db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            
        }
    }
}
