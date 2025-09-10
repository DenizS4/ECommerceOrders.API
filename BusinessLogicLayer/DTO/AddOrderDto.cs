namespace BusinessLogicLayer.DTO;

public class AddOrderDto
{
    public Guid BuyerID { get; set; }          // Who placed the order

    public string Currency { get; set; }       // ISO 4217 (USD, EUR, TRY, …)

    // Addresses
    public AddressDto BillingAddress { get; set; }
    public AddressDto ShippingAddress { get; set; }

    // Order items
    public List<AddOrderItemDto> Items { get; set; }

    // Optional: client can send snapshot (e.g., for guest checkout)
    public BuyerSnapshotDto BuyerSnapshot { get; set; }
    
    public decimal? Subtotal { get; set; }
    public decimal? DiscountTotal { get; set; }
    public decimal? ShippingTotal { get; set; }
    public decimal? TaxTotal { get; set; }
    public decimal? GrandTotal { get; set; }
}