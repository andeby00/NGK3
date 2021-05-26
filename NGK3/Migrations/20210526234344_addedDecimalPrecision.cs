using Microsoft.EntityFrameworkCore.Migrations;

namespace NGK3.Migrations
{
    public partial class addedDecimalPrecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TemperatureC",
                table: "WeatherForecasts",
                type: "decimal(10,1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "AirPressure",
                table: "WeatherForecasts",
                type: "decimal(10,1)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TemperatureC",
                table: "WeatherForecasts",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,1)");

            migrationBuilder.AlterColumn<double>(
                name: "AirPressure",
                table: "WeatherForecasts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,1)");
        }
    }
}
