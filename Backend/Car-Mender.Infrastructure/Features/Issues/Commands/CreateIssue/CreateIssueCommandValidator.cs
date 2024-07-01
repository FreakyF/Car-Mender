using Car_Mender.Domain.Features.Issues.Entities;
using FluentValidation;

namespace Car_Mender.Infrastructure.Features.Issues.Commands.CreateIssue;

public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
{
	public CreateIssueCommandValidator()
	{
		RuleFor(i => i.Description)
			.NotEmpty().WithMessage($"{nameof(Issue.Description)} cannot be empty.")
			.MaximumLength(1000)
			.WithMessage($"{nameof(Issue.Description)} cannot be longer than 1000 characters.");

		RuleFor(i => i.Status)
			.IsInEnum().WithMessage(
				$"{nameof(Issue.Status)}  must be a valid issue status:  {{string.Join(\", \", Enum.GetNames(typeof(IssueStatus)))}}.");

		RuleFor(i => i.ReporterType)
			.IsInEnum().WithMessage(
				$"{nameof(Issue.Status)}  must be a valid reporter type:  {{string.Join(\", \", Enum.GetNames(typeof(ReporterType)))}}.");

		RuleFor(i => i.ReportedDate)
			.NotEmpty().WithMessage($"{nameof(Issue.ReportedDate)} cannot be empty.")
			.Must(date => date.Date <= DateTime.Today)
			.WithMessage($"{nameof(Issue.ReportedDate)} must be today or in the past.");
	}
}