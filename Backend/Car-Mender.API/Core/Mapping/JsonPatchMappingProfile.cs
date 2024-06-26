using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Car_Mender.API.Core.Mapping;

public class JsonPatchMappingProfile : Profile
{
	public JsonPatchMappingProfile()
	{
		CreateMap(typeof(JsonPatchDocument<>), typeof(JsonPatchDocument<>));
		CreateMap(typeof(Operation<>), typeof(Operation<>));
	}
}