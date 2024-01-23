using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Games.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressDelivery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressDelivery",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressDelivery",
                table: "AspNetUsers");
        }
    }
}
