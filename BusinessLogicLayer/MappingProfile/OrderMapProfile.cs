using AutoMapper;
using System.Collections.Generic;
using System.Text.Json;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.Entities;

namespace BusinessLogicLayer.MappingProfile;

public class OrderMapProfile: Profile
{
    private static readonly JsonSerializerOptions _jsonOpts = new(JsonSerializerDefaults.Web);
    public OrderMapProfile()
    {
        CreateMap<Order, GetOrderDto>()
            // Enum -> string (if your Order has enums)
            .ForMember(d => d.OrderStatus,        o => o.MapFrom(s => s.OrderStatus.ToString()))
            .ForMember(d => d.PaymentStatus,      o => o.MapFrom(s => s.PaymentStatus.ToString()))
            .ForMember(d => d.FulfillmentStatus,  o => o.MapFrom(s => s.FulfillmentStatus.ToString()))
            // JSON -> typed DTOs
            .ForMember(d => d.BillingAddress,     o => o.MapFrom(s => FromJson<AddressDto>(s.BillingAddressJson)))
            .ForMember(d => d.ShippingAddress,    o => o.MapFrom(s => FromJson<AddressDto>(s.ShippingAddressJson)))
            .ForMember(d => d.BuyerSnapshot,      o => o.MapFrom(s => FromJson<BuyerSnapshotDto>(s.BuyerSnapshotJson)))
            //Prices
            .ForMember(d => d.Subtotal,              o => o.MapFrom(s => s.Subtotal))
            .ForMember(d => d.TaxTotal,              o => o.MapFrom(s => s.TaxTotal))
            .ForMember(d => d.ShippingTotal,              o => o.MapFrom(s => s.ShippingTotal))
            .ForMember(d => d.DiscountTotal,              o => o.MapFrom(s => s.DiscountTotal))
            .ForMember(d => d.GrandTotal,              o => o.MapFrom(s => s.GrandTotal))
            // Items will be injected via context.Items in the mapping call
            .ForMember(d => d.Items,              o => o.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                // If you pass items via: mapper.Map<GetOrderDto>(order, opt => opt.Items["orderItems"] = items);
                if (ctx.Items.TryGetValue("orderItems", out var value) && value is IEnumerable<OrderItem> items)
                {
                    dest.Items = ctx.Mapper.Map<List<GetOrderItemDto>>(items);
                }
            }).ReverseMap();
        
        
        CreateMap<Order, AddOrderDto>()
            // Enum -> string (if your Order has enums)
            // JSON -> typed DTOs
            .ForMember(d => d.BillingAddress, o => o.MapFrom(s => FromJson<AddressDto>(s.BillingAddressJson)))
            .ForMember(d => d.ShippingAddress, o => o.MapFrom(s => FromJson<AddressDto>(s.ShippingAddressJson)))
            .ForMember(d => d.BuyerSnapshot, o => o.MapFrom(s => FromJson<BuyerSnapshotDto>(s.BuyerSnapshotJson)))

            // Items will be injected via context.Items in the mapping call
            .ForMember(d => d.Items, o => o.Ignore())
            .AfterMap((src, dest, ctx) =>
            {
                // If you pass items via: mapper.Map<GetOrderDto>(order, opt => opt.Items["orderItems"] = items);
                if (ctx.Items.TryGetValue("orderItems", out var value) && value is IEnumerable<OrderItem> items)
                {
                    dest.Items = ctx.Mapper.Map<List<AddOrderItemDto>>(items);
                }
            }) .ReverseMap()
            // statuses are set by your business logic, not client
            .ForMember(o => o.OrderStatus,       opt => opt.Ignore())
            .ForMember(o => o.PaymentStatus,     opt => opt.Ignore())
            .ForMember(o => o.FulfillmentStatus, opt => opt.Ignore())

            // json snapshots
            .ForMember(o => o.BillingAddressJson,  opt => opt.MapFrom(s => ToJson(s.BillingAddress)))
            .ForMember(o => o.ShippingAddressJson, opt => opt.MapFrom(s => ToJson(s.ShippingAddress)))
            .ForMember(o => o.BuyerSnapshotJson,   opt => opt.MapFrom(s => ToJson(s.BuyerSnapshot)))

            // system fields (set in service/repo, not from DTO)
            .ForMember(o => o.CreatedAt,   opt => opt.Ignore())
            .ForMember(o => o.UpdatedAt,   opt => opt.Ignore())
            .ForMember(o => o.PlacedAt,    opt => opt.Ignore())
            .ForMember(o => o.PaidAt,      opt => opt.Ignore())
            .ForMember(o => o.FulfilledAt, opt => opt.Ignore())
            .ForMember(o => o.IsDeleted,   opt => opt.Ignore());
        
        
        CreateMap<AddOrderItemDto, OrderItem>()
            .ForMember(d => d.OrderItemID, o => o.Ignore()) // generated in code
            .ForMember(d => d.OrderID,     o => o.Ignore()) // set when attaching to an order
            .ForMember(d => d.CreatedAt,   o => o.MapFrom(_ => DateTime.UtcNow))
            // Normalize nullables to zero where appropriate
            .ForMember(d => d.LineDiscount, o => o.MapFrom(s => s.LineDiscount ?? 0m))
            .ForMember(d => d.LineTax,      o => o.MapFrom(s => s.LineTax ?? 0m))
            .ForMember(d => d.LineTotal,    o => o.MapFrom(s =>
                (s.UnitPrice * s.Quantity) - (s.LineDiscount ?? 0m) + (s.LineTax ?? 0m)
            )).ReverseMap();
        CreateMap<GetOrderItemDto, OrderItem>()
            .ForMember(d => d.OrderItemID, o => o.MapFrom(s => s.OrderItemID)) 
            .ForMember(d => d.OrderID,     o => o.MapFrom(s => s.OrderID)) // set when attaching to an order
            .ForMember(d => d.CreatedAt,   o => o.MapFrom(_ => DateTime.UtcNow))
            // Normalize nullables to zero where appropriate
            .ForMember(d => d.LineDiscount, o => o.MapFrom(s => s.LineDiscount))
            .ForMember(d => d.LineTax,      o => o.MapFrom(s => s.LineTax))
            .ForMember(d => d.LineTotal,    o => o.MapFrom(s =>
                (s.UnitPrice * s.Quantity) - (s.LineDiscount) + (s.LineTax)
            )).ReverseMap();
    }
    private static T? FromJson<T>(string json)
        => string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(json, _jsonOpts);
    private static string? ToJson<T>(T obj)
        => obj == null ? null : JsonSerializer.Serialize(obj, _jsonOpts);
}