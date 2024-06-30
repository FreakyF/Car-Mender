using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Companies.Errors;

namespace Car_Mender.Domain.Features.Engines.Errors;

public static class EngineErrors
{
	public static readonly Error CouldNotBeFound =
		new(EngineErrorCodes.CouldNotBeFound, "Given engine could not be found.");
}