using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultTimeAndDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_project_employee_manager_id",
                table: "project");

            migrationBuilder.DropForeignKey(
                name: "FK_project_employee_project_manager_id",
                table: "project");

            migrationBuilder.DropForeignKey(
                name: "FK_task_task_group_group_id",
                table: "task");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "task",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "report",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now() at time zone 'utc'",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddForeignKey(
                name: "FK_project_employee_manager_id",
                table: "project",
                column: "manager_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_project_employee_project_manager_id",
                table: "project",
                column: "project_manager_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_task_task_group_group_id",
                table: "task",
                column: "group_id",
                principalTable: "task_group",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_project_employee_manager_id",
                table: "project");

            migrationBuilder.DropForeignKey(
                name: "FK_project_employee_project_manager_id",
                table: "project");

            migrationBuilder.DropForeignKey(
                name: "FK_task_task_group_group_id",
                table: "task");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "task",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "report",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "now() at time zone 'utc'");

            migrationBuilder.AddForeignKey(
                name: "FK_project_employee_manager_id",
                table: "project",
                column: "manager_id",
                principalTable: "employee",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_project_employee_project_manager_id",
                table: "project",
                column: "project_manager_id",
                principalTable: "employee",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_task_task_group_group_id",
                table: "task",
                column: "group_id",
                principalTable: "task_group",
                principalColumn: "id");
        }
    }
}
