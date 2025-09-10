using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Interfaces;


namespace ECommerceOrders.API.APIEndpoints;

public static class OrderApiEndpoints
{
    public static IEndpointRouteBuilder MapOrderApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/getOrders", async (IOrderService orderService) =>
        {
            var orders = await orderService.GetAllOrders();

            return Results.Ok(orders);
        });
        app.MapGet("/api/getOrdersById/{OrderID}", async (IOrderService orderService, Guid orderID) =>
        {
            var orders = await orderService.GetOrdersById(orderID);

            return Results.Ok(orders);
        });
        app.MapGet("/api/getOrdersByCategory/{Category}", async (IOrderService orderService, string category) =>
        {
            var orders = await orderService.GetOrdersByCategory(category);

            return Results.Ok(orders);
        });
        app.MapPost("/api/addOrders", async (IOrderService orderService, AddOrderDto order) =>
        {
            var orders = await orderService.CreateOrder(order);

            return Results.Ok(orders);
        });
        app.MapDelete("/api/deleteOrders/{OrderID}", async (IOrderService orderService, Guid orderID) =>
        {
            var orders = await orderService.DeleteOrder(orderID);

            return Results.Ok(orders);
        });
        return app;
    }
}