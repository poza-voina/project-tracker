using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectTracker.Abstractions.Extensions;
using Tracker = ProjectTracker.Infrastructure;
using History = ProjectTracker.History.Infrastructure;

namespace MigrationRunner;

public class Program
{
	private static async Task Main()
	{
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddEnvironmentVariables()
			.Build();

		await ProcessProjectTrackerMigration(configuration);

		await ProcessProjectTrackerHistoryMigration(configuration);
	}

	private static async Task ProcessProjectTrackerHistoryMigration(IConfigurationRoot configuration)
	{
		var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
		var projectTrackerConnectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.ProjectTrackerHistoryConnectionKey);

		Console.WriteLine(projectTrackerConnectionString);

		var optionsBuilder = new DbContextOptionsBuilder<History.ApplicationDbContext>();
		optionsBuilder.UseNpgsql(projectTrackerConnectionString);

		await using var context = new History.ApplicationDbContext(optionsBuilder.Options);

		Console.WriteLine("Applying migrations...");
		await context.Database.MigrateAsync();
		Console.WriteLine("Migrations applied successfully.");
	}

	private static async Task ProcessProjectTrackerMigration(IConfigurationRoot configuration)
	{
		var connectionSection = configuration.GetRequiredSection(EnvironmentConstants.ConnectionSection);
		var projectTrackerConnectionString = connectionSection.GetRequiredValue<string>(EnvironmentConstants.ProjectTrackerConnectionKey);

		Console.WriteLine(projectTrackerConnectionString);

		var optionsBuilder = new DbContextOptionsBuilder<Tracker.ApplicationDbContext>();
		optionsBuilder.UseNpgsql(projectTrackerConnectionString);

		await using var context = new Tracker.ApplicationDbContext(optionsBuilder.Options);

		Console.WriteLine("Applying migrations...");
		await context.Database.MigrateAsync();
		Console.WriteLine("Migrations applied successfully.");

		await using var seederContext = new Tracker.ApplicationDbContext(optionsBuilder.Options);

		await Tracker.DatabaseSeeder.TrySeedAsync(context);
	}
}