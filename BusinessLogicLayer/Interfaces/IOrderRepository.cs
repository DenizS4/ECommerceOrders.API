using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllOrders();
    Task<List<OrderItem>> GetAllOrderItems();
    Task<List<OrderItem>> GetAllOrderItemsByOrderId(Guid orderId);
    Task<List<OrderItem>> GetAllOrderItemsByCategory(string category);
    Task<Order> GetOrdersById(Guid orderId);
    Task<List<Order>> GetOrdersByCategory(string category);
    Task<List<Order>> GetOrdersBySearchString(string searchString);
    Task<GetOrderDto> CreateOrder(Order order, List<OrderItem> orderItem);
    Task<bool> DeleteOrder(Guid orderId);
}