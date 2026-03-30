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
    public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving orders");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving orders");
        }
    }


    // Get order by ID
    [HttpGet("Retrieve/order/by/{id}")]
    public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found", id);
                return NotFound(new { message = "Order not found" });
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order with ID {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the order");
        }
    }


    // Create a new order
    [HttpPost("Create/new/order")]
    public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            var order = await _orderService.CreateOrderAsync(createOrderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order");
        }
    }


    // Update order status
    [HttpPut("Update/order/status/by/{id}")]
    public async Task<ActionResult<OrderResponseDto>> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
    {
        try
        {
            var order = await _orderService.UpdateOrderStatusAsync(id, updateOrderDto);
            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found for update", id);
                return NotFound(new { message = "Order not found" });
            }

            return Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order with ID {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the order");
        }
    }

    // Delete order by ID
    [HttpDelete("Delete/order/by/{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        try
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
            {
                _logger.LogWarning("Order with ID {OrderId} not found for deletion", id);
                return NotFound(new { message = "Order not found" });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting order with ID {OrderId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the order");
        }
    }
}
