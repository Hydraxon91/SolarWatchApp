using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SunriseSunsets_CityId",
                table: "SunriseSunsets",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SunriseSunsets_Cities_CityId",
                table: "SunriseSunsets",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SunriseSunsets_Cities_CityId",
                table: "SunriseSunsets");

            migrationBuilder.DropIndex(
                name: "IX_SunriseSunsets_CityId",
                table: "SunriseSunsets");
        }
    }
}
