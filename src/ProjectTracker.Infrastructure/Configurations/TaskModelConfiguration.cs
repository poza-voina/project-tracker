using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class TaskModelConfiguration : IEntityTypeConfiguration<TaskModel>
{
	public void Configure(EntityTypeBuilder<TaskModel> builder)
	{
		builder
			.ToTable("task");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);

		ConfigureRelations(builder);
	}

	private static void ConfigureRelations(EntityTypeBuilder<TaskModel> builder)
	{
		builder
			.HasOne(x => x.TaskGroup)
			.WithMany(x => x.Tasks)
			.HasForeignKey(x => x.GroupId);

		builder
			.HasOne(x => x.Project)
			.WithMany(x => x.Tasks)
			.HasForeignKey(x => x.ProjectId);

		builder
			.HasMany(x => x.Performers)
			.WithMany(x => x.PerformedTasks)
			.UsingEntity<PerformerTaskModel>(
				x => x
					.HasOne(x => x.Employee)
					.WithMany()
					.HasForeignKey(x => x.EmployeeId),
				x => x
					.HasOne(x => x.Task)
					.WithMany()
					.HasForeignKey(x => x.TaskId),
				x =>
				{
					x.ToTable("task_performer");
					x.HasKey(x => new { x.TaskId, x.EmployeeId });
				});

		builder
			.HasMany(x => x.Observers)
			.WithMany(x => x.ObservedTasks)
			.UsingEntity<ObserverTaskModel>(
				x => x
					.HasOne(x => x.Employee)
					.WithMany()
					.HasForeignKey(x => x.EmployeeId),
				x => x
					.HasOne(x => x.Task)
					.WithMany()
					.HasForeignKey(x => x.TaskId),
				x =>
				{
					x.ToTable("task_observer");
					x.HasKey(x => new { x.TaskId, x.EmployeeId });
				});

		builder
			.HasOne(x => x.Status)
			.WithMany(x => x.TaskModels)
			.HasForeignKey(x => x.TaskFlowNodeId);

	}

	private static void BindingColumns(EntityTypeBuilder<TaskModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.Name)
			.IsRequired()
			.HasColumnName("name");

		builder
			.Property(x => x.Description)
			.HasColumnName("description");

		builder
			.Property(x => x.ProjectId)
			.IsRequired()
			.HasColumnName("project_id");

		builder
			.Property(x => x.GroupId)
			.HasColumnName("group_id");

		builder
			.Property(x => x.Deadline)
			.HasColumnName("deadline");

		builder
			.Property(x => x.CreatedAt)
			.HasColumnName("created_at");

		builder
			.Property(x => x.TaskFlowNodeId)
			.IsRequired()
			.HasColumnName("task_flow_node_id");

		builder
			.Property(x => x.Version)
			.HasColumnName("xmin")
			.HasColumnType("xid")
			.ValueGeneratedOnAddOrUpdate()
			.IsConcurrencyToken();

		builder
			.Property(x => x.Priority)
			.IsRequired()
			.HasColumnName("priority")
			.HasConversion<string>();
	}
}