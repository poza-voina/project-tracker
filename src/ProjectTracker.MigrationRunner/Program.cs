using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectTracker.Abstractions.Constants;
using ProjectTracker.Abstractions.Extensions;
using ProjectTracker.Infrastructure;

namespace MigrationRunner;

public class Program
{
	private static async Task Main()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddEnvironmentVariables()
			.Build();

		var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
		var connectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.DefaultConnectionStringKey);

		Console.WriteLine(connectionString);

		var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
		optionsBuilder.UseNpgsql(connectionString);

		await using var context = new ApplicationDbContext(optionsBuilder.Options);

		Console.WriteLine("Applying migrations...");
		await context.Database.MigrateAsync();
		Console.WriteLine("Migrations applied successfully.");

		await using var seederContext = new ApplicationDbContext(optionsBuilder.Options);

		await DatabaseSeeder.TrySeedAsync(context);

	}
}