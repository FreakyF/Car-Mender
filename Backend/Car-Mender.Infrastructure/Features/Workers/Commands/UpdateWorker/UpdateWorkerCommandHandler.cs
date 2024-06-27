using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Features.Workers.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.UpdateWorker;

public class UpdateWorkerCommandHandler(
	IWorkerRepository workerRepository,
	IBranchRepository branchRepository,
	IValidator<UpdateWorkerCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateWorkerCommand, Result>
{
	public async Task<Result> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
	{
		var branchExistsResult = await branchRepository.ExistsAsync(request.Id);
		if (branchExistsResult.IsFailure)
		{
			return Result.Failure(branchExistsResult.Error);
		}

		var branchExists = branchExistsResult.Value;
		if (branchExists)
		{
			return BranchErrors.CouldNotBeFound;
		}

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getBranchResult = await workerRepository.GetWorkerByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Worker>>(request.PatchDocument);
		patchDoc.ApplyTo(getBranchResult.Value!);
		await branchRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}