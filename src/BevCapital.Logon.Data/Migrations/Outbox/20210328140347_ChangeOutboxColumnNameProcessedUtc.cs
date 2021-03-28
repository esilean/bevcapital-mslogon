using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BevCapital.Logon.Data.Migrations.Outbox
{
    public partial class ChangeOutboxColumnNameProcessedUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processed",
                table: "Logon_OutboxMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessedAtUtc",
                table: "Logon_OutboxMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedAtUtc",
                table: "Logon_OutboxMessages");

            migrationBuilder.AddColumn<DateTime>(
                name: "Processed",
                table: "Logon_OutboxMessages",
                type: "datetime(6)",
                nullable: true);
        }
    }
}
