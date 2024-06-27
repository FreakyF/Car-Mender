using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Features.Workers.Repository;
using Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;

public class CreateWorkerCommandHandler(
	IWorkerRepository workerRepository,
	IBranchRepository branchRepository,
	IValidator<CreateWorkerCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateWorkerCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
	{
		var branchExistsResult = await branchRepository.ExistsAsync(request.BranchId);
		if (branchExistsResult.IsFailure)
		{
			return Result<Guid>.Failure(branchExistsResult.Error);
		}

		var branchExists = branchExistsResult.Value;
		if (!branchExists)
		{
			return BranchErrors.CouldNotBeFound;
		}

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var workerEntity = mapper.Map<Worker>(request);
		return await workerRepository.CreateWorkerAsync(workerEntity, cancellationToken);
	}
}