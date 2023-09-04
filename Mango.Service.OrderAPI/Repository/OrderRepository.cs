using Mango.Service.OrderAPI.Context;
using Mango.Service.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationContext> _dbContext;
        public OrderRepository(DbContextOptions<ApplicationContext> dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _db = new ApplicationContext(_dbContext);
            _db.OrderHeaders.Add(orderHeader);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {

            await using var _db = new ApplicationContext(_dbContext);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u=>u.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null) 
            { 
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
        }
    }
}
