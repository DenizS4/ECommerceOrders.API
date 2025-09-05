using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetAllOrders();
    Task<Order> GetOrdersById(int orderId);
    Task<List<Order>> GetOrdersByCategory(string category);
    Task<List<Order>> GetOrdersBySearchString(string searchString);
    Task<GetOrdersDto> CreateOrder(Order order);
    Task<bool> DeleteOrder(int orderId);
}