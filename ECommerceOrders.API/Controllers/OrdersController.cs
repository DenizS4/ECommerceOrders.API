// using BusinessLogicLayer.DTO;
// using BusinessLogicLayer.Interfaces;
// using Microsoft.AspNetCore.Mvc;
//
// namespace ECommerceOrders.API.Controllers;
//
// public class OrdersController : Controller
// {
//     private readonly IOrderService _orderService;
//
//     public OrdersController(IOrderService orderService)
//     {
//         _orderService = orderService;
//     }
//     [HttpGet("GetAllOrders")]
//     public async Task<IActionResult> GetAllOrders()
//     {
//         var orders = await _orderService.GetAllOrders();
//         
//         return Ok(orders);
//     }
//     [HttpGet("GetOrderByOrderId")]
//     public async Task<IActionResult> GetOrderByOrderId(Guid orderId)
//     {
//         var order = await _orderService.GetOrdersById(orderId);
//
//         return Ok(order);
//         
//     }
//     [HttpGet("GetOrdersByCategory")]
//     public async Task<IActionResult> GetOrdersByCategoryId(string category)
//     {
//         var orders = await _orderService.GetOrdersByCategory(category);
//         
//         return Ok(orders);
//         
//     }
//     [HttpGet("GetOrdersBySearchString")]
//     public async Task<IActionResult> GetOrdersBySearchString(string searchString)
//     {
//         var orders = await _orderService.GetOrdersBySearchString(searchString);
//         
//         return Ok(orders);
//     }
//     [HttpPost("AddOrders")]
//     public async Task<IActionResult> AddOrders(AddOrderDto order)
//     {
//         if (order == null)
//         {
//             throw new Exception("Order cannot be null");
//         }
//
//         var addedOrder = await _orderService.CreateOrder(order);
//         
//         return Ok(addedOrder);
//     }
//     [HttpDelete("DeleteOrders")]
//     public async Task<IActionResult> DeleteOrders(Guid orderId)
//     {
//         if (orderId == null)
//         {
//             throw new Exception("OrderId cannot be zero");
//         }
//         var isDeleted = await _orderService.DeleteOrder(orderId);
//         
//         return Ok(isDeleted);
//     }
// }