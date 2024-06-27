using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Domain.Features.Workers.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

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
		var branchIdUpdateOperation = GetUpdateBranchIdOperation(request);
		var branchIdBeingUpdated = branchIdUpdateOperation is not null;
		if (branchIdBeingUpdated)
		{
			var branchId = Guid.Parse(branchIdUpdateOperation!.value.ToString()!);
			if (branchId.Equals(Guid.Empty)) return Error.InvalidId;
			var branchExistsResult = await branchRepository.ExistsAsync(branchId);
			if (!branchExistsResult.Value) return BranchErrors.CouldNotBeFound;
		}

		var workerExistResult = await workerRepository.ExistsAsync(request.Id);
		if (!workerExistResult.Value) return WorkerErrors.CouldNotBeFound;

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

	private static Operation<UpdateWorkerDto>? GetUpdateBranchIdOperation(UpdateWorkerCommand request)
	{
		return request.PatchDocument.Operations
			.FirstOrDefault(op =>
				string.Equals(op.path, $"/{nameof(UpdateWorkerDto.BranchId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}
}