using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Engines.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.CreateEngine;

public class CreateEngineCommandHandler(
	IEngineRepository engineRepository,
	IValidator<CreateEngineCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateEngineCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateEngineCommand request, CancellationToken cancellationToken)
	{
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var engineEntity = mapper.Map<Engine>(request);
		return await engineRepository.CreateEngineAsync(engineEntity, cancellationToken);
	}
}