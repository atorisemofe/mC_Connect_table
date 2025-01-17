using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mC_Connect_table.Migrations
{
    /// <inheritdoc />
    public partial class AddMctIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MctId",
                table: "OrderViewModel",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MctId",
                table: "OrderViewModel");
        }
    }
}
