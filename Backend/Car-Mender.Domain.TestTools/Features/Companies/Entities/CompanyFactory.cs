using Bogus;
using Car_Mender.Domain.TestTools.Models;
using Company = Car_Mender.Domain.Features.Companies.Entities.Company;

namespace Car_Mender.Domain.TestTools.Features.Companies.Entities;

public static class CompanyFactory
{
    private static readonly Faker<Company> FakerDefinition = new Faker<Company>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.Email, f => f.Internet.Email())
        .RuleFor(c => c.Name, f => f.Company.CompanyName())
        .RuleFor(c => c.Address, f => AddressFactory.Create())
        .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
        .RuleFor(c => c.Nip, f => f.Random.Replace("###-###-##-##"));

    public static Company Create() => FakerDefinition.Generate();
}