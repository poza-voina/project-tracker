using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectTracker.Infrastructure.Models;

namespace ProjectTracker.Infrastructure.Configurations;

public class ReportModelConfiguration : IEntityTypeConfiguration<ReportModel>
{
	public void Configure(EntityTypeBuilder<ReportModel> builder)
	{
		builder
			.ToTable("report");

		builder
			.HasKey(x => x.Id);

		BindingColumns(builder);
	}

	private static void BindingColumns(EntityTypeBuilder<ReportModel> builder)
	{
		builder
			.Property(x => x.Id)
			.HasColumnName("id");

		builder
			.Property(x => x.Url)
			.HasColumnName("url");

		builder
			.Property(x => x.Type)
			.HasColumnName("type")
			.HasConversion<string>();

		builder
			.Property(x => x.CreatedAt)
			.HasColumnName("created_at");

		builder
			.Property(x => x.Status)
			.HasColumnName("status")
			.HasConversion<string>();
	}
}