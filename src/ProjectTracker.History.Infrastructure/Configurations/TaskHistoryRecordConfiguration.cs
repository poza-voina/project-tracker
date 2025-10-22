using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.History.Infrastructure.Models;

namespace ProjectTracker.History.Infrastructure.Configurations;

public class TaskHistoryRecordModelConfiguration : IEntityTypeConfiguration<TaskHistoryRecordModel>
{
	public void Configure(EntityTypeBuilder<TaskHistoryRecordModel> builder)
	{
		builder
			.ToTable("task_history_record");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);

		ConfigureRelations(builder);
	}

	private static void ConfigureRelations(EntityTypeBuilder<TaskHistoryRecordModel> builder)
	{
		builder
			.HasOne(x => x.Meta)
			.WithMany(x => x.TaskHistoryRecords)
			.HasForeignKey(x => x.MetaId);
	}

	private static void BindingColumns(EntityTypeBuilder<TaskHistoryRecordModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.MetaId)
			.HasColumnName("meta_id");

		builder
			.Property(x => x.ChangeEventType)
			.HasColumnName("change_event_type");

		builder
			.Property(x => x.Property)
			.HasColumnName("property");

		builder
			.Property(x => x.OldValue)
			.HasColumnName("old_value");

		builder
			.Property(x => x.NewValue)
			.HasColumnName("new_value");

		builder
			.Property(x => x.CreatedAt)
			.HasColumnName("created_at")
			.HasDefaultValueSql("now() at time zone 'utc'");
	}
}
