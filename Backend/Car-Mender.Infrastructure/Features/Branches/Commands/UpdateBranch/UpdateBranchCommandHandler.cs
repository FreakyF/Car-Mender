using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Branches.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.UpdateBranch;

public class UpdateBranchCommandHandler(
	IBranchRepository branchRepository,
	IValidator<UpdateBranchCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateBranchCommand, Result>
{
	public async Task<Result> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
	{
		var branchExistsResult = await branchRepository.ExistsAsync(request.Id);
		if (!branchExistsResult.Value)
		{
			return BranchErrors.CouldNotBeFound;
		}

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getBranchResult = await branchRepository.GetBranchByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Branch>>(request.PatchDocument);
		patchDoc.ApplyTo(getBranchResult.Value!);
		await branchRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}
}