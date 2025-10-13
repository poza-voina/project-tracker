using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class TaskFlowEdgeModelConfiguration : IEntityTypeConfiguration<TaskFlowEdgeModel>
{
	public void Configure(EntityTypeBuilder<TaskFlowEdgeModel> builder)
	{
		builder
			.ToTable("task_flow_edge");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);
	}

	private static void BindingColumns(EntityTypeBuilder<TaskFlowEdgeModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.FromNodeId)
			.HasColumnName("from_node_id");

		builder
			.Property(x => x.ToNodeId)
			.HasColumnName("to_node_id");

		builder
			.Property(x => x.TaskFlowId)
			.HasColumnName("task_flow_id");
	}
}