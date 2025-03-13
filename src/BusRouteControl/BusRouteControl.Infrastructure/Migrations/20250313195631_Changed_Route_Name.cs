using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusRouteControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changed_Route_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Routes",
                newName: "BusRoutes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "BusRoutes",
                newName: "Routes");
        }
    }
}
