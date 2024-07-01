using AutoMapper;
using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Errors;
using Car_Mender.Domain.Features.Appointments.Repository;
using Car_Mender.Domain.Features.Branches.Errors;
using Car_Mender.Domain.Features.Issues.DTOs;
using Car_Mender.Domain.Features.Issues.Entities;
using Car_Mender.Domain.Features.Issues.Errors;
using Car_Mender.Domain.Features.Issues.Repository;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Domain.Features.Workers.Errors;
using Car_Mender.Domain.Features.Workers.Repository;
using Car_Mender.Infrastructure.Features.Workers.Commands.UpdateWorker;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.UpdateIssue;

public class UpdateIssueCommandHandler(
	IIssueRepository issueRepository,
	IAppointmentRepository appointmentRepository,
	IWorkerRepository workerRepository,
	IValidator<UpdateIssueCommand> validator,
	IMapper mapper
) : IRequestHandler<UpdateIssueCommand, Result>
{
	public async Task<Result> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
	{
		var appointmentIdUpdateOperation = GetUpdateAppointmentIdOperation(request);
		var appointmentIdBeingUpdated = appointmentIdUpdateOperation is not null;
		if (appointmentIdBeingUpdated)
		{
			var appointmentId = Guid.Parse(appointmentIdUpdateOperation!.value.ToString()!);
			if (appointmentId.Equals(Guid.Empty)) return Error.InvalidId;
			var appointmentExistsResult = await appointmentRepository.ExistsAsync(appointmentId);
			if (!appointmentExistsResult.Value) return AppointmentErrors.CouldNotBeFound;
		}

		var workerIdUpdateOperation = GetUpdateWorkerIdOperation(request);
		var workerIdBeingUpdated = workerIdUpdateOperation is not null;
		if (workerIdBeingUpdated)
		{
			var workerId = Guid.Parse(workerIdUpdateOperation!.value.ToString()!);
			if (workerId.Equals(Guid.Empty)) return Error.InvalidId;
			var workerExistsResult = await workerRepository.ExistsAsync(workerId);
			if (!workerExistsResult.Value) return WorkerErrors.CouldNotBeFound;
		}

		var issueExistsResult = await issueRepository.ExistsAsync(request.Id);
		if (!issueExistsResult.Value) return IssueErrors.CouldNotBeFound;

		var validationResult = await validator.ValidateAsync(request, cancellationToken);
		if (!validationResult.IsValid)
		{
			var errors = validationResult.Errors.Select(e => e.ErrorMessage);
			return Error.ValidationError(errors);
		}

		var getIssueResult = await issueRepository.GetIssueByIdAsync(request.Id, cancellationToken);
		var patchDoc = mapper.Map<JsonPatchDocument<Issue>>(request.PatchDocument);
		patchDoc.ApplyTo(getIssueResult.Value!);
		await issueRepository.SaveChangesAsync(cancellationToken);

		return Result.Success();
	}

	private static Operation<UpdateIssueDto>? GetUpdateAppointmentIdOperation(UpdateIssueCommand request)
	{
		return request.PatchDocument.Operations
			.Find(op =>
				string.Equals(op.path, $"/{nameof(UpdateIssueDto.CreatorId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}

	private static Operation<UpdateIssueDto>? GetUpdateWorkerIdOperation(UpdateIssueCommand request)
	{
		return request.PatchDocument.Operations
			.Find(op =>
				string.Equals(op.path, $"/{nameof(UpdateWorkerDto.BranchId)}",
					StringComparison.InvariantCultureIgnoreCase)
			);
	}
}