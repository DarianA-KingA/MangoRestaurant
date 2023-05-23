using Mango.Service.ShoppingCartAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ShoppingCartAPI.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartHeader> CartHeaders { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }


    }
}
