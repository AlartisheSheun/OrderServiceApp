using OrderAPI.Models;

namespace OrderAPI.DTOs;

// Creating a new order
public class CreateOrderDto
{
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

// Updating an order's status
public class UpdateOrderDto
{
    public OrderStatus Status { get; set; }
}

// API responses
public class OrderResponseDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
