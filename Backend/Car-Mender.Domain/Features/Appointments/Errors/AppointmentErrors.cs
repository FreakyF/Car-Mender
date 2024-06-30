using Car_Mender.Domain.Common;

namespace Car_Mender.Domain.Features.Appointments.Errors;

public class AppointmentErrors
{
	public static readonly Error CouldNotBeFound =
		new(AppointmentErrorCodes.CouldNotBeFound, "Given appointment could not be found.");
}