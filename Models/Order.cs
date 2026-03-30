namespace OrderAPI.Models;

// Represents an Order entity in the system
public class Order
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

// Represents the status of an order
public enum OrderStatus
{
    Pending,
    Completed,
    Cancelled
}
