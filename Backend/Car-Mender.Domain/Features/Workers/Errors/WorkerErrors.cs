using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Workers.Errors;

public static class WorkerErrors
{
	public static readonly Error CouldNotBeFound =
		new(WorkerErrorCodes.CouldNotBeFound, "Given worker could not be found.");
}