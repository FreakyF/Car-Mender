using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.BranchesVehicles.Errors;

public static class BranchVehiclesErrors
{
	public static readonly Error CouldNotBeFound =
		new(BranchVehicleErrorCodes.CouldNotBeFound, "Given branch vehicle could not be found.");
}