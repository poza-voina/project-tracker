using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.History.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_task_history_meta_task_id",
                table: "task_history_meta",
                column: "task_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_task_history_meta_task_id",
                table: "task_history_meta");
        }
    }
}
