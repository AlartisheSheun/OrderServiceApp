using Microsoft.AspNetCore.Mvc;
using OrderAPI.DTOs;
using OrderAPI.Models;
using OrderAPI.Services;

namespace OrderAPI.Controllers;

// Managing orders
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }


    // Get all orders
    [HttpGet("Retrieve/all/orders")]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderResponseDto>>>> GetOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(ApiResponse<IEnumerable<OrderResponseDto>>.SuccessResponse(orders, "Orders retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<IEnumerable<OrderResponseDto>>.ErrorResponse("An error occurred while retrieving orders"));
        }
    }


    // Get order by ID
    [HttpGet("Retrieve/order/by/{id}")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> GetOrder(int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found", id);
                return NotFound(ApiResponse<OrderResponseDto>.ErrorResponse("Order not found"));
            }

            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(order, "Order retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order with ID {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<OrderResponseDto>.ErrorResponse("An error occurred while retrieving the order"));
        }
    }


    // Create a new order
    [HttpPost("Create/new/order")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(createOrderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id },
                ApiResponse<OrderResponseDto>.SuccessResponse(order, "Order created successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<OrderResponseDto>.ErrorResponse("An error occurred while creating the order"));
        }
    }


    // Update order status
    [HttpPut("Update/order/status/by/{id}")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
    {
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, updateOrderDto);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found for update", id);
                return NotFound(ApiResponse<OrderResponseDto>.ErrorResponse("Order not found"));
            }

            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(order, "Order updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order with ID {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<OrderResponseDto>.ErrorResponse("An error occurred while updating the order"));
        }
    }

    // Delete order by ID
    [HttpDelete("Delete/order/by/{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteOrder(int id)
    {
        try
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
            {
                _logger.LogWarning("Order with ID {OrderId} not found for deletion", id);
                return NotFound(ApiResponse<object>.ErrorResponse("Order not found"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Order deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting order with ID {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.ErrorResponse("An error occurred while deleting the order"));
        }
    }
}
