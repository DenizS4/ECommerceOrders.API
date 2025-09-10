using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Entities;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class OrderRepository: IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public OrderRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Order>> GetAllOrders()
    {
        var orders = await _context.Orders.Where(x => x.IsDeleted == false).ToListAsync();
        
        
        return orders;
    }

    public async Task<List<OrderItem>> GetAllOrderItems()
    {
        var orderItems = await _context.OrderItems.ToListAsync();
        
        return orderItems;
    }

    public async Task<List<OrderItem>> GetAllOrderItemsByOrderId(Guid orderId)
    {
        var orderItems = await _context.OrderItems.Where(x=> x.OrderID == orderId).ToListAsync();

        return orderItems;
    }

    public async Task<List<OrderItem>> GetAllOrderItemsByCategory(string category)
    {
        var orderItems = await _context.OrderItems.Where(x=> x.Category == category).ToListAsync();
        
        return orderItems;
    }

    public async Task<Order> GetOrdersById(Guid orderId)
    {
        var order = await _context.Orders.Where(x => x.IsDeleted == false && x.OrderID == orderId).FirstOrDefaultAsync();
        if (order == null)
            throw new Exception("Order not found");
        return order;
    }

    public async Task<List<Order>> GetOrdersByCategory(string category)
    {
        
        var orderItems = await _context.OrderItems.Where(x => x.Category == category).Select(x => x.OrderID).Distinct().ToListAsync();
        var orders = await _context.Orders.Where(x => x.IsDeleted == false && orderItems.Contains(x.OrderID)).ToListAsync();

        return orders;
    }

    public Task<List<Order>> GetOrdersBySearchString(string searchString)
    {
        throw new NotImplementedException();
    }

    public async Task<GetOrderDto> CreateOrder(Order order, List<OrderItem> orderItems)
    {
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        var addedOrder = await _context.Orders.Where(x => x.OrderID == order.OrderID && x.IsDeleted == false).FirstOrDefaultAsync();
        if (addedOrder == null)
        {
            throw new Exception("Order couldn't created");
        }

        foreach (var orderItem in orderItems)
        {
            orderItem.OrderID = addedOrder.OrderID;
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }
        var orderMapped = _mapper.Map<GetOrderDto>(order, opt => opt.Items["orderItems"] = orderItems);

        return orderMapped;
    }

    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var order = await GetOrdersById(orderId);
        if (order == null)
        {
            throw new Exception("Order not found");
        }
        order.IsDeleted = true;
        _context.Orders.Update(order);
        var rows = await _context.SaveChangesAsync();
        
        return rows > 0;
    }
}