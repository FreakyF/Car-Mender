using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Companies.Repository;
using Car_Mender.Domain.Models;
using Car_Mender.Infrastructure.Features.Companies.Commands.CreateCompany;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Car_Mender.Infrastructure.Tests.Features.Companies.Commands.CreateCompany;

public class CreateCompanyCommandHandlerTests
{
    private readonly Mock<ICompanyRepository> _mockCompanyRepository;
    private readonly Mock<IValidator<CreateCompanyCommand>> _mockValidator;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CreateCompanyCommandHandler _handler;

    public CreateCompanyCommandHandlerTests()
    {
        _mockCompanyRepository = new Mock<ICompanyRepository>();
        _mockValidator = new Mock<IValidator<CreateCompanyCommand>>();
        _mockMapper = new Mock<IMapper>();
        _handler = new CreateCompanyCommandHandler(_mockCompanyRepository.Object, _mockValidator.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldReturnValidId()
    {
        // Arrange
        var command = new CreateCompanyCommand
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
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateCompanyCommand>(), It.IsAny<CancellationToken>()))
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
            Email = "invalid mail",
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

        var company = new Company
        {
            Email = "invalid mail",
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

    [Fact]
    public async Task Handle_RequestHasSpecificProperty_ShouldMapCorrectly()
    {
        // Arrange
        var command = new CreateCompanyCommand
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
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var company = new Company
        {
            Email = command.Email,
            Name = command.Name,
            Address = command.Address,
            Phone = command.Phone,
            Nip = command.Nip
        };

        _mockMapper.Setup(m => m.Map<Company>(It.IsAny<CreateCompanyCommand>()))
            .Returns(company);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockMapper.Verify(m => m.Map<Company>(It.Is<CreateCompanyCommand>(c =>
            c.Email == command.Email &&
            c.Name == command.Name &&
            c.Address == command.Address &&
            c.Phone == command.Phone &&
            c.Nip == command.Nip)), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidMapping_ShouldReturnMappingError()
    {
        // Arrange
        var command = new CreateCompanyCommand
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
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateCompanyCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var incorrectCompany = new Company
        {
            Email = "incorrect@mail.com",
            Name = "IncorrectName",
            Address = new Address
            {
                Street = "IncorrectStreet",
                City = "IncorrectCity",
                PostalCode = "98-765",
                Region = "IncorrectRegion",
                Country = "IncorrectCountry"
            },
            Phone = "987654321",
            Nip = "098-765-43-21"
        };

        _mockMapper.Setup(m => m.Map<Company>(It.IsAny<CreateCompanyCommand>()))
            .Returns(incorrectCompany);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mockMapper.Verify(m => m.Map<Company>(It.Is<CreateCompanyCommand>(c =>
            c.Email == command.Email &&
            c.Name == command.Name &&
            c.Address.Street == command.Address.Street &&
            c.Address.City == command.Address.City &&
            c.Address.PostalCode == command.Address.PostalCode &&
            c.Address.Region == command.Address.Region &&
            c.Address.Country == command.Address.Country &&
            c.Phone == command.Phone &&
            c.Nip == command.Nip)), Times.Once);
    }
}