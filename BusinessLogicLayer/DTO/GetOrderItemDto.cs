namespace BusinessLogicLayer.DTO;

public class GetOrderItemDto
{
    public Guid OrderItemID { get; set; }
    public Guid OrderID { get; set; }
    public string ProductRefID { get; set; }
    public string Sku { get; set; }
    public string ProductName { get; set; }
    public string Category { get; set; }

    // Pricing per line
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal LineDiscount { get; set; }
    public decimal LineTax { get; set; }
    public decimal LineTotal { get; set; }
}