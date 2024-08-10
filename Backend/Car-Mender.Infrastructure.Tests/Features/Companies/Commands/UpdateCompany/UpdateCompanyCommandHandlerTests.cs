using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.DTOs;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Companies.Repository;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
using Car_Mender.Infrastructure.Features.Companies.Commands.UpdateCompany;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using System.ComponentModel.Design;

namespace Car_Mender.Infrastructure.Tests.Features.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private readonly Mock<IValidator<UpdateCompanyCommand>> _mockValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UpdateCompanyCommandHandler _handler; // TODO: USE IRequestHandler<Command, ReturnValue>

    public UpdateCompanyCommandHandlerTests()
    {
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _mockValidator = new Mock<IValidator<UpdateCompanyCommand>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new UpdateCompanyCommandHandler(_mockCompanyRepository.Object, _mockValidator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var company = new Company
        {
            Email = "test@mail.com",
            Name = "TestName",
            Address = new Address
            {
                Street = "TestStreet",
                City = "TestCity",
                PostalCode = "12-345",
                Region = "TestRegion",
                Country = "TestCountry"
            },
            Phone = "123456789",
            Nip = "123-456-78-90"
        };

        var command = new UpdateCompanyCommand(
            company.Id,
            new JsonPatchDocument<UpdateCompanyDto>()
        );

        _mockCompanyRepository.Setup(repo => repo.ExistsAsync(company.Id))
            .ReturnsAsync(Result<bool>.Success(true));

        var validationResult = new ValidationResult();
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<UpdateCompanyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockCompanyRepository.Setup(repo => repo.GetCompanyByIdAsync(company.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Company>.Success(company));

        var patchDoc = new JsonPatchDocument<Company>();
        _mockMapper.Setup(m => m.Map<JsonPatchDocument<Company>>(It.IsAny<JsonPatchDocument<UpdateCompanyDto>>()))
            .Returns(patchDoc);

        _mockCompanyRepository.Setup(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<int>.Success(1));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
