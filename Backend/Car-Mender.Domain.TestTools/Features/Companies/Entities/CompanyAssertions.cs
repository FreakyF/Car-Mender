using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Xunit;

namespace Car_Mender.Domain.TestTools.Features.Companies.Entities;

public static class CompanyAssertions
{
    public static void AssertEqualTo(this GetCompanyDto actual, Company expected)
    {
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Email, actual.Email);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Nip, actual.Nip);
        Assert.Equal(expected.Phone, actual.Phone);
        Assert.Equal(expected.Address, actual.Address);
    }
}