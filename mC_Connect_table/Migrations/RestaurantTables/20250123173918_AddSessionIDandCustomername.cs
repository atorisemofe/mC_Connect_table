using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mC_Connect_table.Migrations.RestaurantTables
{
    /// <inheritdoc />
    public partial class AddSessionIDandCustomername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentSessionId",
                table: "RestaurantTables",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "RestaurantTables",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSessionId",
                table: "RestaurantTables");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "RestaurantTables");
        }
    }
}
