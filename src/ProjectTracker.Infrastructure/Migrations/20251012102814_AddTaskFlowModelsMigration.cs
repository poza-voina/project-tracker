using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProjectTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskFlowModelsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "task");

            migrationBuilder.AddColumn<long>(
                name: "task_flow_node_id",
                table: "task",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "task",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<long>(
                name: "task_flow_id",
                table: "project",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "task_flow",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_flow", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_flow_node",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    task_flow_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_flow_node", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_flow_node_task_flow_task_flow_id",
                        column: x => x.task_flow_id,
                        principalTable: "task_flow",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_flow_edge",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    from_node_id = table.Column<long>(type: "bigint", nullable: true),
                    to_node_id = table.Column<long>(type: "bigint", nullable: true),
                    task_flow_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_flow_edge", x => x.id);
                    table.ForeignKey(
                        name: "FK_task_flow_edge_task_flow_node_from_node_id",
                        column: x => x.from_node_id,
                        principalTable: "task_flow_node",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_task_flow_edge_task_flow_node_to_node_id",
                        column: x => x.to_node_id,
                        principalTable: "task_flow_node",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_task_flow_edge_task_flow_task_flow_id",
                        column: x => x.task_flow_id,
                        principalTable: "task_flow",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_task_task_flow_node_id",
                table: "task",
                column: "task_flow_node_id");

            migrationBuilder.CreateIndex(
                name: "IX_project_task_flow_id",
                table: "project",
                column: "task_flow_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_flow_edge_from_node_id",
                table: "task_flow_edge",
                column: "from_node_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_flow_edge_task_flow_id",
                table: "task_flow_edge",
                column: "task_flow_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_flow_edge_to_node_id",
                table: "task_flow_edge",
                column: "to_node_id");

            migrationBuilder.CreateIndex(
                name: "IX_task_flow_node_task_flow_id",
                table: "task_flow_node",
                column: "task_flow_id");

            migrationBuilder.AddForeignKey(
                name: "FK_project_task_flow_task_flow_id",
                table: "project",
                column: "task_flow_id",
                principalTable: "task_flow",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_task_task_flow_node_task_flow_node_id",
                table: "task",
                column: "task_flow_node_id",
                principalTable: "task_flow_node",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_project_task_flow_task_flow_id",
                table: "project");

            migrationBuilder.DropForeignKey(
                name: "FK_task_task_flow_node_task_flow_node_id",
                table: "task");

            migrationBuilder.DropTable(
                name: "task_flow_edge");

            migrationBuilder.DropTable(
                name: "task_flow_node");

            migrationBuilder.DropTable(
                name: "task_flow");

            migrationBuilder.DropIndex(
                name: "IX_task_task_flow_node_id",
                table: "task");

            migrationBuilder.DropIndex(
                name: "IX_project_task_flow_id",
                table: "project");

            migrationBuilder.DropColumn(
                name: "task_flow_node_id",
                table: "task");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "task");

            migrationBuilder.DropColumn(
                name: "task_flow_id",
                table: "project");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "task",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
