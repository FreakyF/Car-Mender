using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Engines.Errors;

public static class EngineErrors
{
	public static readonly Error CouldNotBeFound =
		new(EngineErrorCodes.CouldNotBeFound, "Given engine could not be found.");
}