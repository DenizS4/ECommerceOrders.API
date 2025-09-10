using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Interfaces;

public interface IOrderService
{
    Task<List<GetOrderDto>> GetAllOrders();
    Task<GetOrderDto> GetOrdersById(Guid orderId);
    Task<List<GetOrderDto>> GetOrdersByCategory(string category);
    Task<List<GetOrderDto>> GetOrdersBySearchString(string searchString);
    Task<GetOrderDto> CreateOrder(AddOrderDto order);
    Task<bool> DeleteOrder(Guid orderId);
}