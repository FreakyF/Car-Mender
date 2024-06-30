using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Vehicles.Errors;

public static class VehicleErrors
{
	public static readonly Error CouldNotBeFound =
		new(VehicleErrorCodes.CouldNotBeFound, "Given vehicle could not be found.");
}