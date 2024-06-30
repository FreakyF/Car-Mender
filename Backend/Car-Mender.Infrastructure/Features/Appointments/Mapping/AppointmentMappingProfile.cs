using AutoMapper;
using Car_Mender.Domain.Features.Appointments.DTOs;
using Car_Mender.Domain.Features.Appointments.Entities;
using Car_Mender.Infrastructure.Features.Appointments.Commands.CreateAppointment;

namespace Car_Mender.Infrastructure.Features.Appointments.Mapping;

public class AppointmentMappingProfile : Profile
{
	public AppointmentMappingProfile()
	{
		CreateMap<CreateAppointmentCommand, Appointment>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.Vehicle, opt => opt.Ignore());
		CreateMap<Appointment, GetAppointmentDto>();
		CreateMap<Appointment, UpdateAppointmentDto>();
	}
}