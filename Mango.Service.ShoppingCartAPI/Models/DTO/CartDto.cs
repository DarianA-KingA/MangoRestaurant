namespace Mango.Service.ShoppingCartAPI.Models.DTO
{
    public class CartDto
    {
        public CartHeader CartHeader { get; set; }
        public IEnumerable<CartDetails> CartDetails { get; set; }
    }
}
