using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Interfaces;

namespace DataAccessLayer.Repositories;

public class OrderRepository: IOrderRepository
{
    public Task<List<Order>> GetAllOrders()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrdersById(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrdersByCategory(string category)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrdersBySearchString(string searchString)
    {
        throw new NotImplementedException();
    }

    public Task<GetOrdersDto> CreateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteOrder(int orderId)
    {
        throw new NotImplementedException();
    }
}