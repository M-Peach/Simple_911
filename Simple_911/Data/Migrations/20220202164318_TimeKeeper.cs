using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Simple_911.Data.Migrations
{
    public partial class TimeKeeper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeKeeperId",
                table: "Incidents",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TimeKeeper",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TCreated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TDispatched = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TEnroute = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TOnscene = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TTransporting = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    THospital = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TInService = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TOOS = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    TAssist = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IncidentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeKeeper", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_TimeKeeperId",
                table: "Incidents",
                column: "TimeKeeperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_TimeKeeper_TimeKeeperId",
                table: "Incidents",
                column: "TimeKeeperId",
                principalTable: "TimeKeeper",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_TimeKeeper_TimeKeeperId",
                table: "Incidents");

            migrationBuilder.DropTable(
                name: "TimeKeeper");

            migrationBuilder.DropIndex(
                name: "IX_Incidents_TimeKeeperId",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "TimeKeeperId",
                table: "Incidents");
        }
    }
}
