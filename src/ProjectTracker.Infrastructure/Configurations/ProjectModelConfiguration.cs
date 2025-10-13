using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class ProjectModelConfiguration : IEntityTypeConfiguration<ProjectModel>
{
	public void Configure(EntityTypeBuilder<ProjectModel> builder)
	{
		builder
			.ToTable("project");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);

		ConfigureRelations(builder);
	}

	private static void ConfigureRelations(EntityTypeBuilder<ProjectModel> builder)
	{
		builder
			.HasOne(x => x.ProjectManager)
			.WithMany(x => x.ManagedProjects)
			.HasForeignKey(x => x.ProjectManagerId);

		builder
			.HasOne(x => x.Manager)
			.WithMany(x => x.SupervisedProjects)
			.HasForeignKey(x => x.ManagerId);

		builder
			.HasOne(x => x.TaskFlow)
			.WithMany(x => x.Projects)
			.HasForeignKey(x => x.TaskFlowId);
	}

	private static void BindingColumns(EntityTypeBuilder<ProjectModel> builder)
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
			.Property(x => x.ProjectManagerId)
			.IsRequired()
			.HasColumnName("project_manager_id");

		builder
			.Property(x => x.ManagerId)
			.HasColumnName("manager_id");

		builder
			.Property(x => x.TaskFlowId)
			.HasColumnName("task_flow_id")
			.IsRequired();
	}
}
