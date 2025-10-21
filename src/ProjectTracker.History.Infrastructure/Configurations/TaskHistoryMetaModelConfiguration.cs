using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.History.Infrastructure.Models;

namespace ProjectTracker.History.Infrastructure.Configurations;

public class TaskHistoryMetaModelConfiguration : IEntityTypeConfiguration<TaskHistoryMetaModel>
{
	public void Configure(EntityTypeBuilder<TaskHistoryMetaModel> builder)
	{
		builder
			.ToTable("task_history_meta");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);

		ConfigureRelations(builder);

		ConfigureIndexes(builder);
	}

	private static void ConfigureRelations(EntityTypeBuilder<TaskHistoryMetaModel> builder)
	{
		builder
			.HasMany(x => x.TaskHistoryRecords)
			.WithOne(x => x.Meta)
			.HasForeignKey(x => x.MetaId);
	}

	private static void ConfigureIndexes(EntityTypeBuilder<TaskHistoryMetaModel> builder)
	{
		builder
			.HasIndex(x => x.TaskId)
			.IsUnique();
	}

	private static void BindingColumns(EntityTypeBuilder<TaskHistoryMetaModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.TaskId)
			.HasColumnName("task_id");
	}
}


