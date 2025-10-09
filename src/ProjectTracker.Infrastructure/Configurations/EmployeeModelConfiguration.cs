using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class EmployeeModelConfiguration : IEntityTypeConfiguration<EmployeeModel>
{
	public void Configure(EntityTypeBuilder<EmployeeModel> builder)
	{
		builder
			.ToTable("employee");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);
	}

	private static void BindingColumns(EntityTypeBuilder<EmployeeModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.LastName)
			.IsRequired()
			.HasColumnName("last_name");

		builder
			.Property(x => x.FirstName)
			.IsRequired()
			.HasColumnName("first_name");

		builder
			.Property(x => x.Patronymic)
			.HasColumnName("patronymic");

		builder
			.Property(x => x.Username)
			.IsRequired()
			.HasColumnName("username");

		builder
			.Property(x => x.Role)
			.IsRequired()
			.HasColumnName("role")
			.HasConversion<string>();
	}
}