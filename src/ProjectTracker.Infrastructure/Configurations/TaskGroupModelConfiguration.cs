using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class TaskGroupModelConfiguration : IEntityTypeConfiguration<TaskGroupModel>
{
	public void Configure(EntityTypeBuilder<TaskGroupModel> builder)
	{
		builder
			.ToTable("task_group");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);
	}

	private static void BindingColumns(EntityTypeBuilder<TaskGroupModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.Name)
			.IsRequired()
			.HasColumnName("name");
	}
}
