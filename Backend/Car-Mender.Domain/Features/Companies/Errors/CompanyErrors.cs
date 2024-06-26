using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Companies.Errors;

public static class CompanyErrors
{
	public static readonly Error CouldNotBeFound =
		new(CompanyErrorCodes.CouldNotBeFound, "Given company could not be found.");
}