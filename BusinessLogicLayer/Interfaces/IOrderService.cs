using BusinessLogicLayer.DTO;

namespace BusinessLogicLayer.Interfaces;

public interface IOrderService
{
    Task<List<GetOrdersDto>> GetAllOrders();
    Task<GetOrdersDto> GetOrdersById(int orderId);
    Task<List<GetOrdersDto>> GetOrdersByCategory(string category);
    Task<List<GetOrdersDto>> GetOrdersBySearchString(string searchString);
    Task<GetOrdersDto> CreateOrder(AddOrderDto order);
    Task<bool> DeleteOrder(int orderId);
}