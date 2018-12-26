using Microsoft.EntityFrameworkCore.Migrations;

namespace Thermostat.Model.Migrations
{
    public partial class AddedHumidityColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceHumidity",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceHumidity",
                table: "Devices");
        }
    }
}
