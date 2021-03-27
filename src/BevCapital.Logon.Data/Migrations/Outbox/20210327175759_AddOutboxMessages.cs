using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BevCapital.Logon.Data.Migrations.Outbox
{
    public partial class AddOutboxMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Outbox_OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: false),
                    Processed = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outbox_OutboxMessages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Outbox_OutboxMessages");
        }
    }
}
