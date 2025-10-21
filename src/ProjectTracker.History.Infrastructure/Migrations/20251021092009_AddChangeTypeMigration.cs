using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.History.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddChangeTypeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "change_event_type",
                table: "task_history_record",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "change_event_type",
                table: "task_history_record");
        }
    }
}
