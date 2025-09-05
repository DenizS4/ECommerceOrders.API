using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Interfaces;

namespace BusinessLogicLayer.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<List<GetOrdersDto>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllOrders();
        
        if(orders == null)
            throw new ApplicationException("No orders found");
        var ordersMapped = _mapper.Map<List<GetOrdersDto>>(orders);
        return ordersMapped;
    }

    public async Task<GetOrdersDto> GetOrdersById(int orderId)
    {
        var order = await _orderRepository.GetOrdersById(orderId);
        
        if(order == null)
            throw new ApplicationException("No orders found");
        var orderMapped = _mapper.Map<GetOrdersDto>(order);
        return orderMapped;
    }

    public async Task<List<GetOrdersDto>> GetOrdersByCategory(string category)
    {
        var orders = await _orderRepository.GetOrdersByCategory(category);
        
        if(orders == null)
            throw new ApplicationException("No orders found");
        var ordersMapped = _mapper.Map<List<GetOrdersDto>>(orders);
        return ordersMapped;
    }


    public async Task<List<GetOrdersDto>> GetOrdersBySearchString(string searchString)
    {
        var orders = await _orderRepository.GetOrdersBySearchString(searchString);
        
        if(orders == null)
            throw new ApplicationException("No orders found");
        var ordersMapped = _mapper.Map<List<GetOrdersDto>>(orders);
        return ordersMapped;
    }

    public async Task<GetOrdersDto> CreateOrder(AddOrderDto order)
    {
        //order.CreateDate = DateTime.Now;
        var orderMapped = _mapper.Map<Order>(order);
        var orders = await _orderRepository.CreateOrder(orderMapped);
        
        if(order == null)
            throw new ApplicationException("No order found");
        return orders;
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        var orders = await _orderRepository.DeleteOrder(orderId);
        
        if(orders == false)
            throw new ApplicationException("No order found");
        return true;
    }
}