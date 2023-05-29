using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDto
    {
        public ProductDto()
        {
            Count = 1;
        }
        public int ProductId { get; set; }
        [ValidateNever]
        public string Name { get; set; }
        [ValidateNever]

        public double Price { get; set; }
        [ValidateNever]

        public string Description { get; set; }
        [ValidateNever]

        public string ImageUrl { get; set; }
        [ValidateNever]

        public string CategoryName { get; set; }
        [Range(1, 100)]
        public int Count { get; set; }
    }
}
