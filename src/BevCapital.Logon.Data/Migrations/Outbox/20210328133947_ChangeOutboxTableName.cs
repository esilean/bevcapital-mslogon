using Microsoft.EntityFrameworkCore.Migrations;

namespace BevCapital.Logon.Data.Migrations.Outbox
{
    public partial class ChangeOutboxTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Outbox_OutboxMessages",
                table: "Outbox_OutboxMessages");

            migrationBuilder.RenameTable(
                name: "Outbox_OutboxMessages",
                newName: "Logon_OutboxMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logon_OutboxMessages",
                table: "Logon_OutboxMessages",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logon_OutboxMessages",
                table: "Logon_OutboxMessages");

            migrationBuilder.RenameTable(
                name: "Logon_OutboxMessages",
                newName: "Outbox_OutboxMessages");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Outbox_OutboxMessages",
                table: "Outbox_OutboxMessages",
                column: "Id");
        }
    }
}
