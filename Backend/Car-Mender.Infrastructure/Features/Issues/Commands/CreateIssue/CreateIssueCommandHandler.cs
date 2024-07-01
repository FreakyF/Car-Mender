using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Errors;
using Car_Mender.Domain.Features.Appointments.Repository;
using Car_Mender.Domain.Features.Issues.Entities;
using Car_Mender.Domain.Features.Issues.Repository;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Domain.Features.Workers.Repository;
using FluentValidation;
using MediatR;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;

public class CreateIssueCommandHandler(
	IIssueRepository issueRepository,
	IAppointmentRepository appointmentRepository,
	IWorkerRepository workerRepository,
	IValidator<CreateIssueCommand> validator,
	IMapper mapper
) : IRequestHandler<CreateIssueCommand, Result<Guid>>
{
	public async Task<Result<Guid>> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
	{
		var appointmentExistsResult = await appointmentRepository.ExistsAsync(request.AppointmentId);
		if (appointmentExistsResult.IsFailure) return Result<Guid>.Failure(appointmentExistsResult.Error);

		var appointmentExists = appointmentExistsResult.Value;
		if (!appointmentExists) return AppointmentErrors.CouldNotBeFound;

		var workerExistsResult = await workerRepository.ExistsAsync(request.CreatorId);
		if (workerExistsResult.IsFailure) return Result<Guid>.Failure(workerExistsResult.Error);

		var workerExists = workerExistsResult.Value;
		if (!workerExists) return WorkerErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var workerEntity = mapper.Map<Issue>(request);
		return await issueRepository.CreateIssueAsync(workerEntity, cancellationToken);
	}
}