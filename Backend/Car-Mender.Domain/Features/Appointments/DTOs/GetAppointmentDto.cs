using Car_Mender.Domain.Common;
using Car_Mender.Domain.Features.Appointments.Errors;

namespace Car_Mender.Domain.Features.Appointments.DTOs;

public static class GetAppointmentDto
{
	public static readonly Error CouldNotBeFound =
		new(AppointmentErrors.CouldNotBeFound, "Given appointment could not be found.");
}