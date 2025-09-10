namespace BusinessLogicLayer.DTO;

public class AddOrderItemDto
{
    public string ProductRefID { get; set; }   // from Product service
    public string Sku { get; set; }
    public string ProductName { get; set; }    // snapshot at order time
    public string Category { get; set; }       // snapshot at order time

    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; }
    public decimal? LineDiscount { get; set; }
    public decimal? LineTax { get; set; }
    public decimal? LineTotal { get; set; }
}