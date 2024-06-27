using Car_Mender.Domain.Entities;
using Car_Mender.Domain.Features.Branches.Entities;
using Car_Mender.Domain.Features.Companies.Entities;
using Car_Mender.Domain.Features.Engines.Entities;
using Car_Mender.Domain.Features.Workers.Entities;
using Microsoft.EntityFrameworkCore;

namespace Car_Mender.Infrastructure.Persistence.DatabaseContext;

public class AppDbContext(DbContextOptions options) : DbContext(options), IAppDbContext
{
	public DbSet<Company> Companies { get; set; } = null!;
	public DbSet<Appointment> Appointments { get; set; } = null!;
	public DbSet<Branch> Branches { get; set; } = null!;
	public DbSet<BranchVehicle> BranchesVehicles { get; set; } = null!;
	public DbSet<Vehicle> Vehicles { get; set; } = null!;
	public DbSet<Engine> Engines { get; set; } = null!;
	public DbSet<Issue> Issues { get; set; } = null!;
	public DbSet<Worker> Workers { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Company>().OwnsOne(c => c.Address);
		modelBuilder.Entity<Branch>().OwnsOne(b => b.Address);
	}
}