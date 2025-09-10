using BusinessLogicLayer.Enums;

namespace BusinessLogicLayer.Entities;

public class Order
{
    public Guid OrderID { get; set; }
    public Guid BuyerID { get; set; } // from Auth/Users svc

    // Money
    public string Currency { get; set; } = "USD";
    public decimal Subtotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal ShippingTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal GrandTotal { get; set; } // Subtotal - Discount + Shipping + Tax

    // Statuses
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public FulfillmentStatus FulfillmentStatus { get; set; }
    public string BillingAddressJson { get; set; }  // {name,phone,lines,city,zip,country,…}
    public string ShippingAddressJson { get; set; }
    public string BuyerSnapshotJson { get; set; }   // {email,name,company,…}
    
    // System log
    public DateTime PlacedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime? FulfilledAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}