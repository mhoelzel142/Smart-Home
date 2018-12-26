using Microsoft.EntityFrameworkCore.Migrations;

namespace Thermostat.Model.Migrations
{
    public partial class AddTemperatureToDevices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceTemperature",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceTemperature",
                table: "Devices");
        }
    }
}
