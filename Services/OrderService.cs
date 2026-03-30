using OrderAPI.DTOs;
using OrderAPI.Models;
using OrderAPI.Repositories;

namespace OrderAPI.Services;

// Interface for order service operations
public interface IOrderService
{
    Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync();
    Task<OrderResponseDto?> GetOrderByIdAsync(int id);
    Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, UpdateOrderDto updateOrderDto);
    Task<bool> DeleteOrderAsync(int id);
}

// Service implementation for order business logic
public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersAsync()
    {
        var orders = await _repository.GetAllOrdersAsync();
        return orders.Select(MapToResponseDto);
    }

    public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
    {
        var order = await _repository.GetOrderByIdAsync(id);
        return order != null ? MapToResponseDto(order) : null;
    }

    public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        var order = new Order
        {
            CustomerName = createOrderDto.CustomerName,
            Amount = createOrderDto.Amount,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        var createdOrder = await _repository.CreateOrderAsync(order);
        return MapToResponseDto(createdOrder);
    }

    public async Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, UpdateOrderDto updateOrderDto)
    {
        var order = new Order { Status = updateOrderDto.Status };
        var updatedOrder = await _repository.UpdateOrderAsync(id, order);
        return updatedOrder != null ? MapToResponseDto(updatedOrder) : null;
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        return await _repository.DeleteOrderAsync(id);
    }

    private static OrderResponseDto MapToResponseDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            CustomerName = order.CustomerName,
            Amount = order.Amount,
            Status = order.Status.ToString(),
            CreatedAt = order.CreatedAt
        };
    }
}
