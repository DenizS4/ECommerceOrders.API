namespace BusinessLogicLayer.Entities;

public class OrderItem
{
    public Guid OrderItemID { get; set; }
    public Guid OrderID { get; set; }               // logical link, no FK
    public string ProductRefID { get; set; }        // from Product svc

    // Snapshots
    public string Sku { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }        

    // Pricing per line
    public decimal UnitPrice { get; set; }          // after line-level discounts
    public decimal Quantity { get; set; }           // support fractional (e.g., weights)
    public decimal LineDiscount { get; set; }
    public decimal LineTax { get; set; }
    public decimal LineTotal { get; set; }          // UnitPrice*Qty - Discount + Tax
    
    public DateTime CreatedAt { get; set; }
}