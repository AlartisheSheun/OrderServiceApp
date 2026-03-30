using OrderAPI.Data;
using OrderAPI.Models;

namespace OrderAPI.Repositories;

// Interface for order repository operations
public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(int id);
    Task<Order> CreateOrderAsync(Order order);
    Task<Order?> UpdateOrderAsync(int id, Order order);
    Task<bool> DeleteOrderAsync(int id);
}

// Repository implementation for order data access using Entity Framework Core
public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _context;

    public OrderRepository(OrderDbContext context)
    {
        _context = context;
    }
    
    // <inheritdoc />
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await Task.FromResult(_context.Orders.ToList());
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await Task.FromResult(_context.Orders.FirstOrDefault(o => o.Id == id));
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateOrderAsync(int id, Order order)
    {
        var existingOrder = await GetOrderByIdAsync(id);
        if (existingOrder == null)
            return null;

        existingOrder.Status = order.Status;
        _context.Orders.Update(existingOrder);
        await _context.SaveChangesAsync();
        return existingOrder;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await GetOrderByIdAsync(id);
        if (order == null)
            return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }
}
