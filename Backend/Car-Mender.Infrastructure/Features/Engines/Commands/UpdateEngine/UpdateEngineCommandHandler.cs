using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Engines.Errors;
using Car_Mender.Domain.Features.Engines.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Engines.Commands.UpdateEngine;

public class UpdateEngineCommandHandler(
	IEngineRepository engineRepository,
	IValidator<UpdateEngineCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateEngineCommand, Result>
{
	public async Task<Result> Handle(UpdateEngineCommand request, CancellationToken cancellationToken)
	{
		var engineExistsResult = await engineRepository.ExistsAsync(request.Id);
		if (!engineExistsResult.Value)
		{
			return EngineErrors.CouldNotBeFound;
		}

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getEngineResult = await engineRepository.GetEngineByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Engine>>(request.PatchDocument);
		patchDoc.ApplyTo(getEngineResult.Value!);
		await engineRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}