using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Companies.Repository;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Car_Mender.Infrastructure.Tests.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private readonly Mock<IValidator<CreateCompanyCommand>> _mockValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly IRequestHandler<CreateCompanyCommand,  Result<Guid>> _handler;

    public CreateCompanyCommandHandlerTests()
    {
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _mockValidator = new Mock<IValidator<CreateCompanyCommand>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateCompanyCommandHandler(
            _mockCompanyRepository.Object,
            _mockValidator.Object,
            _mockMapper.Object
        );
    }

    private static CreateCompanyCommand GetCreateCompanyCommand() => new()
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

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnValidId()
    {
        // Arrange
        var command = GetCreateCompanyCommand();

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
        var validationResult = new ValidationResult();
        // TODO: zrobic it.Is dla konkretnej instancji WSZEDZIE GDZIE TO MOZLIWE.
        _mockValidator.Setup(v => 
                v.ValidateAsync(It.IsAny<CreateCompanyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        _mockMapper.Setup(m => m.Map<Company>(It.IsAny<CreateCompanyCommand>()))
            .Returns(company);

        _mockCompanyRepository.Setup(r => r.CreateCompanyAsync(It.IsAny<Company>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Guid>.Success(company.Id));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(company.Id, result.Value);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ShouldReturnValidationErrorCode()
    {
        // Arrange
        var command = new CreateCompanyCommand
        {
            Email = null,
            Name = null,
            Address = null,
            Phone = null,
            Nip = null
        };

        var validationErrors = new List<ValidationFailure>
        {
            new(nameof(Company.Email), $"Invalid {nameof(Company.Email).ToLowerInvariant()} format")
        };
        var validationResult = new ValidationResult(validationErrors);
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateCompanyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(ErrorCodes.ValidationError, result.Error.Code);
    }
}