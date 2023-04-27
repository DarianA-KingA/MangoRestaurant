using Microsoft.EntityFrameworkCore;
namespace Mango.Services.ProductAPI.DbContexts
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext>options):base(options)
        {
            
        }
        
    }
}
