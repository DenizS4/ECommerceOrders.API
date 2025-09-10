namespace BusinessLogicLayer.DTO;

public class AddressDto
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string City { get; set; }
    public string Zip { get; set; }
    public string Country { get; set; }  // ISO-2 code
}