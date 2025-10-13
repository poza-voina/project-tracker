using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class TaskFlowModelConfiguration : IEntityTypeConfiguration<TaskFlowModel>
{
	public void Configure(EntityTypeBuilder<TaskFlowModel> builder)
	{
		builder
			.ToTable("task_flow");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);

		ConfigureRelations(builder);
	}

	private static void ConfigureRelations(EntityTypeBuilder<TaskFlowModel> builder)
	{
		builder
			.HasMany(x => x.Nodes)
			.WithOne(x => x.TaskFlow)
			.HasForeignKey(x => x.TaskFlowId);

		builder
			.HasMany(x => x.Edges)
			.WithOne(x => x.TaskFlow)
			.HasForeignKey(x => x.TaskFlowId);

		builder
			.HasOne(x => x.ProjectDeletableStatus)
			.WithOne(x => x.DeletableTaskFlow)
			.HasForeignKey<TaskFlowModel>(x => x.ProjectDeletableStatusId);
	}

	private static void BindingColumns(EntityTypeBuilder<TaskFlowModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.ProjectDeletableStatusId)
			.HasColumnName("project_deletable_status_id");
	}
}
