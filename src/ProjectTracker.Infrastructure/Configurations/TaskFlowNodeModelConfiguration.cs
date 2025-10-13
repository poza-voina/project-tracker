using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class TaskFlowNodeModelConfiguration : IEntityTypeConfiguration<TaskFlowNodeModel>
{
	public void Configure(EntityTypeBuilder<TaskFlowNodeModel> builder)
	{
		builder
			.ToTable("task_flow_node");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);

		ConfigureRelations(builder);
	}

	private static void ConfigureRelations(EntityTypeBuilder<TaskFlowNodeModel> builder)
	{
		builder
			.HasMany(x => x.FromEdges)
			.WithOne(x => x.FromNode)
			.HasForeignKey(x => x.FromNodeId);

		builder
			.HasMany(x => x.ToEdges)
			.WithOne(x => x.ToNode)
			.HasForeignKey(x => x.ToNodeId);
	}

	private static void BindingColumns(EntityTypeBuilder<TaskFlowNodeModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.Name)
			.HasColumnName("name");

		builder
			.Property(x => x.Status)
			.HasColumnName("status")
			.HasConversion<string>();

		builder
			.Property(x => x.TaskFlowId)
			.HasColumnName("task_flow_id");
	}
}