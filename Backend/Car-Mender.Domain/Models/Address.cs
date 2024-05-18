namespace Car_Mender.Domain.Models;

// ReSharper disable once ClassNeverInstantiated.Global
public class Address
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public required string Region { get; set; }
    public required string Country { get; set; }
}