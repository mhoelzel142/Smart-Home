using Microsoft.EntityFrameworkCore.Migrations;

namespace Thermostat.Migrations.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceName = table.Column<string>(nullable: true),
                    DeviceIp = table.Column<string>(nullable: true),
                    DeviceMac = table.Column<string>(nullable: true),
                    DeviceTemperature = table.Column<string>(nullable: true),
                    DeviceHumidity = table.Column<string>(nullable: true),
                    DeviceTileColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
