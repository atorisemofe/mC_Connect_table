using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mC_Connect_table.Migrations.RestaurantTables
{
    /// <inheritdoc />
    public partial class RemoveTableStatus1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "TableStatus",
            table: "RestaurantTables");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "TableStatus",
            table: "RestaurantTables",
            type: "TEXT",
            nullable: true);
        }
    }
}
