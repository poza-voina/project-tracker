using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectTracker.History.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskMetaModelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "meta_id",
                table: "task_history_record",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "task_history_meta",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    task_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_history_meta", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_history_record_meta_id",
                table: "task_history_record",
                column: "meta_id");

            migrationBuilder.AddForeignKey(
                name: "FK_task_history_record_task_history_meta_meta_id",
                table: "task_history_record",
                column: "meta_id",
                principalTable: "task_history_meta",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_task_history_record_task_history_meta_meta_id",
                table: "task_history_record");

            migrationBuilder.DropTable(
                name: "task_history_meta");

            migrationBuilder.DropIndex(
                name: "IX_task_history_record_meta_id",
                table: "task_history_record");

            migrationBuilder.DropColumn(
                name: "meta_id",
                table: "task_history_record");
        }
    }
}
