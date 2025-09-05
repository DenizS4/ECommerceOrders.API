using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceOrders.API.Controllers;

public class OrdersController : Controller
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    [HttpGet("GetAllOrders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var Orders = await _orderService.GetAllOrders();
        
        return Ok(Orders);
    }
    [HttpGet("GetOrderByOrderId")]
    public async Task<IActionResult> GetOrderByOrderId(int OrderId)
    {
        var Order = await _orderService.GetOrdersById(OrderId);

        return Ok(Order);
        
    }
    [HttpGet("GetOrdersByCategoryId")]
    public async Task<IActionResult> GetOrdersByCategoryId(string category)
    {
        var Orders = await _orderService.GetOrdersByCategory(category);
        
        return Ok(Orders);
        
    }
    [HttpGet("GetOrdersBySearchString")]
    public async Task<IActionResult> GetOrdersBySearchString(string searchString)
    {
        var Orders = await _orderService.GetOrdersBySearchString(searchString);
        
        return Ok(Orders);
    }
    [HttpPost("AddOrders")]
    public async Task<IActionResult> AddOrders(AddOrderDto Order)
    {
        if (Order == null)
        {
            throw new Exception("Order cannot be null");
        }

        var addedOrder = await _orderService.CreateOrder(Order);
        
        return Ok(addedOrder);
    }
    [HttpDelete("DeleteOrders")]
    public async Task<IActionResult> DeleteOrders(int OrderId)
    {
        if (OrderId == 0)
        {
            throw new Exception("OrderId cannot be zero");
        }
        var isDeleted = await _orderService.DeleteOrder(OrderId);
        
        return Ok(isDeleted);
    }
}