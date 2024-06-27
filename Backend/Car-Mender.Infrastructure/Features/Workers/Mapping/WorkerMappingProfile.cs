using AutoMapper;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Workers.DTOs;
using Car_Mender.Domain.Features.Workers.Entities;
using Car_Mender.Infrastructure.Features.Workers.Commands.CreateWorker;

namespace Car_Mender.Infrastructure.Features.Workers.Mapping;

public class WorkerMappingProfile : Profile
{
	public WorkerMappingProfile()
	{
		CreateMap<CreateWorkerCommand, Worker>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.BranchId, opt => opt.Ignore());
		CreateMap<Worker, GetWorkerDto>();
	}
}