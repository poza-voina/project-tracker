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
	}

	private static void BindingColumns(EntityTypeBuilder<TaskFlowModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");
	}
}
