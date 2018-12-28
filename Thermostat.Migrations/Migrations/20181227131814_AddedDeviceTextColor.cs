using Microsoft.EntityFrameworkCore.Migrations;

namespace Thermostat.Migrations.Migrations
{
    public partial class AddedDeviceTextColor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceTextColor",
                table: "Devices",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceTextColor",
                table: "Devices");
        }
    }
}
