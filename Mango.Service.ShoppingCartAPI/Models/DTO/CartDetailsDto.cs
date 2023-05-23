using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Service.ShoppingCartAPI.Models.DTO
{
    public class CartDetailDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public virtual CartHeader CartHeader { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Count { get; set; }
    }
}
