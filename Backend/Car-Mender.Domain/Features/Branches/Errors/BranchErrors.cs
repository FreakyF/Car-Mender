using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Branches.Errors;

public static class BranchErrors
{
	public static readonly Error CouldNotBeFound =
		new(BranchErrorCodes.CouldNotBeFound, "Given branch could not be found.");
}