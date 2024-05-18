using Car_Mender.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Persistence.DatabaseContext;

public class AppDbContext(DbContextOptions options) : DbContext(options), IAppDbContext
{
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<Appointment> Appointments { get; set; } = null!;
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<BranchVehicle> BranchesVehicles { get; set; } = null!;
    public DbSet<Vehicle> Cars { get; set; } = null!;
    public DbSet<Engine> Engines { get; set; } = null!;
    public DbSet<Issue> Issues { get; set; } = null!;
    public DbSet<Worker> Workers { get; set; } = null!;
}