using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mC_Connect_table.Migrations
{
    /// <inheritdoc />
    public partial class AddNotesField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "OrderViewModel",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "OrderViewModel");
        }
    }
}
