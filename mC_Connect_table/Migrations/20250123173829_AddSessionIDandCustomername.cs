using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mC_Connect_table.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionIDandCustomername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentSessionId",
                table: "OrderViewModel",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "OrderViewModel",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentSessionId",
                table: "OrderViewModel");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "OrderViewModel");
        }
    }
}
