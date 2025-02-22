using Orders.Domain.Entities;
using Orders.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Repositories
{
    public class OrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            
            var orders = await _context.Orders
                .Include(o => o.Client)
                .ToListAsync();
            return orders.FirstOrDefault(o => o.ID == id);
            
        }

        public async Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            var existingOrder = await _context.Orders.FindAsync(order.ID);

            if (existingOrder != null)
            {
                _context.Entry(existingOrder).State = EntityState.Detached;
            }

            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public async Task DeleteOrderAsync(Guid id)
        {
            var existingOrder = await _context.Orders.FindAsync(id);

            if (existingOrder != null)
            {
                _context.Entry(existingOrder).State = EntityState.Detached; // Desadjuntar la entidad existente
            }

            existingOrder.IsDeleted = true;
            _context.Entry(existingOrder).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}

