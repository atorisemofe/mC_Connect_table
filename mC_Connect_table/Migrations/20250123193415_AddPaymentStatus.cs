using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mC_Connect_table.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaymentStatus",
                table: "OrderViewModel",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "OrderViewModel",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderViewModel");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "OrderViewModel");
        }
    }
}
