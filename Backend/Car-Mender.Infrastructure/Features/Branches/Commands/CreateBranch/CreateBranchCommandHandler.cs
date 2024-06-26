using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Branches.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Branches.Commands.CreateBranch;

public class CreateBranchCommandHandler(
	IBranchRepository branchRepository,
	IValidator<CreateBranchCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateBranchCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
	{
		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var branchEntity = mapper.Map<Branch>(request);
		return await branchRepository.CreateBranchAsync(branchEntity, cancellationToken);
	}
}