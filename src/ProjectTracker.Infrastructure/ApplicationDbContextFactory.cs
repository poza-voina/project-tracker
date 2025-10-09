using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectTracker.Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext> //TODO что-то со строкой придумать
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

		optionsBuilder.UseNpgsql(
			"Host=localhost;Port=5432;Database=project-tracker;Username=postgres;Password=postgres"
		);

		return new ApplicationDbContext(optionsBuilder.Options);
	}
}
