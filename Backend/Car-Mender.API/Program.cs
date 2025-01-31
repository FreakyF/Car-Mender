using Car_Mender.API.Features.Swagger;
using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Appointments.Repository;
using Car_Mender.Domain.Features.Branches.Repository;
using Car_Mender.Domain.Features.Companies.Repository;
using Car_Mender.Domain.Features.Engines.Repository;
using Car_Mender.Domain.Features.Issues.Repository;
using Car_Mender.Domain.Features.Vehicles.Repository;
using Car_Mender.Domain.Features.Workers.Repository;
using Car_Mender.Domain.Repositories;
using Car_Mender.Infrastructure;
using Car_Mender.Infrastructure.Features.Appointments.Repository;
using Car_Mender.Infrastructure.Features.Branches.Repository;
using Car_Mender.Infrastructure.Features.BranchesVehicles.Repository;
using Car_Mender.Infrastructure.Features.Companies.Repository;
using Car_Mender.Infrastructure.Features.Engines.Repository;
using Car_Mender.Infrastructure.Features.Issues.Repository;
using Car_Mender.Infrastructure.Features.Vehicles.Repository;
using Car_Mender.Infrastructure.Features.Workers.Repository;
using Car_Mender.Infrastructure.Persistence.DatabaseContext;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllers().AddNewtonsoftJson();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c => { c.OperationFilter<JsonPatchDocumentFilter>(); });

		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
		builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
		{
			options.UseSqlServer(connectionString);
		});
		builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
		builder.Services.AddScoped<IBranchRepository, BranchRepository>();
		builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
		builder.Services.AddScoped<IEngineRepository, EngineRepository>();
		builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
		builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
		builder.Services.AddScoped<IIssueRepository, IssueRepository>();
		builder.Services.AddScoped<IBranchVehicleRepository, BranchVehicleRepository>();
		builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<InfrastructureAssemblyMarker>());
		builder.Services.AddValidatorsFromAssemblyContaining<InfrastructureAssemblyMarker>();

		builder.Services.AddAutoMapper(typeof(InfrastructureAssemblyMarker).Assembly);
		builder.Services.AddAutoMapper(typeof(Program).Assembly);

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}