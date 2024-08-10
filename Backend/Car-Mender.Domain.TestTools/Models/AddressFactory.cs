using Bogus;
using Car_Mender.Domain.Models;

namespace Car_Mender.Domain.TestTools.Models;

public static class AddressFactory
{
    private static readonly Faker<Address> FakerDefinition = new Faker<Address>()
        .RuleFor(a => a.Street, f => f.Address.StreetAddress())
        .RuleFor(a => a.City, f => f.Address.City())
        .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
        .RuleFor(a => a.Region, f => f.Address.State())
        .RuleFor(a => a.Country, f => f.Address.Country());

    public static Address Create() => FakerDefinition.Generate();
}