namespace BusinessLogicLayer.DTO;

public class GetOrderDto
{
    public Guid OrderID { get; set; }
    public Guid BuyerID { get; set; }

    // Money
    public string Currency { get; set; }
    public decimal Subtotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal ShippingTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; }

    // Statuses
    public string OrderStatus { get; set; }
    public string PaymentStatus { get; set; }
    public string FulfillmentStatus { get; set; }

    // Addresses & snapshot
    public AddressDto BillingAddress { get; set; }
    public AddressDto ShippingAddress { get; set; }
    public BuyerSnapshotDto BuyerSnapshot { get; set; }

    // Items (merged into the response)
    public List<GetOrderItemDto> Items { get; set; }

    // System log
    public DateTime PlacedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}