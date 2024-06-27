using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Workers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Persistence.DatabaseContext;

public interface IAppDbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<BranchVehicle> BranchesVehicles { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Engine> Engines { get; set; }
    public DbSet<Issue> Issues { get; set; }
    public DbSet<Worker> Workers { get; set; }
}