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

    public async Task<List<GetOrderDto>> GetAllOrders()
    {
        var orders = await _orderRepository.GetAllOrders();
        if (orders == null || orders.Count == 0)
            throw new ApplicationException("No orders found");

        var orderItems = await _orderRepository.GetAllOrderItems();
        
        var itemsByOrderId = orderItems
            .GroupBy(i => i.OrderID)
            .ToDictionary(g => g.Key, g => (IEnumerable<OrderItem>)g.ToList());
        var result = new List<GetOrderDto>(orders.Count);
        foreach (var order in orders)
        {
            itemsByOrderId.TryGetValue(order.OrderID, out var itemsForThisOrder);
            var dto = _mapper.Map<GetOrderDto>(
                order,
                opt => opt.Items["orderItems"] = itemsForThisOrder ?? Enumerable.Empty<OrderItem>()
            );
            result.Add(dto);
        }

        return result;
    }

    public async Task<GetOrderDto> GetOrdersById(Guid orderId)
    {
        var order = await _orderRepository.GetOrdersById(orderId);
        
        if(order == null)
            throw new ApplicationException("No orders found");
        var orderItems = await _orderRepository.GetAllOrderItemsByOrderId(orderId);
        
        var orderMapped = _mapper.Map<GetOrderDto>(order, opt => opt.Items["orderItems"] = orderItems);
        return orderMapped;
    }

    public async Task<List<GetOrderDto>> GetOrdersByCategory(string category)
    {
        var orders = await _orderRepository.GetOrdersByCategory(category);
        
        if(orders == null)
            throw new ApplicationException("No orders found");
        
        var orderItems = await _orderRepository.GetAllOrderItemsByCategory(category);
        
        var itemsByOrderId = orderItems
            .GroupBy(i => i.OrderID)
            .ToDictionary(g => g.Key, g => (IEnumerable<OrderItem>)g.ToList());
        
        var result = new List<GetOrderDto>(orders.Count);
        foreach (var order in orders)
        {
            itemsByOrderId.TryGetValue(order.OrderID, out var itemsForThisOrder);
            var dto = _mapper.Map<GetOrderDto>(
                order,
                opt => opt.Items["orderItems"] = itemsForThisOrder ?? Enumerable.Empty<OrderItem>()
            );
            result.Add(dto);
        }
        return result;
    }


    public async Task<List<GetOrderDto>> GetOrdersBySearchString(string searchString)
    {
        var orders = await _orderRepository.GetOrdersBySearchString(searchString);
        
        if(orders == null)
            throw new ApplicationException("No orders found");
        var ordersMapped = _mapper.Map<List<GetOrderDto>>(orders);
        return ordersMapped;
    }

    public async Task<GetOrderDto> CreateOrder(AddOrderDto order)
    {
        //order.CreateDate = DateTime.Now;
        var orderMapped = _mapper.Map<Order>(order);
        var orderItemsMapped = _mapper.Map<List<OrderItem>>(order.Items);
        var orders = await _orderRepository.CreateOrder(orderMapped, orderItemsMapped);
        
        if(order == null)
            throw new ApplicationException("No order found");
        return orders;
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var orders = await _orderRepository.DeleteOrder(orderId);
        
        if(orders == false)
            throw new ApplicationException("No order found");
        return true;
    }
}