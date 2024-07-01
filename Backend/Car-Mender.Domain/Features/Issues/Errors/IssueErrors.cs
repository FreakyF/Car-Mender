using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Issues.Errors;

public static class IssueErrors
{
	public static readonly Error CouldNotBeFound =
		new(IssueErrorCodes.CouldNotBeFound, "Given issue could not be found.");
}