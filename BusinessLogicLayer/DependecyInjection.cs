using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace BusinessLogicLayer;

public static class DependecyInjection
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddTransient<IOrderService, OrderService>();
        services.AddValidatorsFromAssembly(typeof(AddOrdersValidator).Assembly);
        return services;
    }
}