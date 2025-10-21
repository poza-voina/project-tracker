using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectTracker.History.Infrastructure;

// TODO Что-то сделать со строкой
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
	public ApplicationDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

		optionsBuilder.UseNpgsql(
			"Host=localhost;Port=5432;Database=project-tracker-history;Username=postgres;Password=postgres"
		);

		return new ApplicationDbContext(optionsBuilder.Options);
	}
}
