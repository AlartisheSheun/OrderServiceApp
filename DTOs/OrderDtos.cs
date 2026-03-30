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

// Generic API response wrapper
public class ApiResponse<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool Success { get; set; }

    public static ApiResponse<T> SuccessResponse(T? data, string message = "Success")
    {
        return new ApiResponse<T>
        {
            Data = data,
            Message = message,
            Success = true
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Data = data,
            Message = message,
            Success = false
        };
    }
}
