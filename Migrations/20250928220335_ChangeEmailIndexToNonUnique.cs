using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillionRealEstatecompany.API.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEmailIndexToNonUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Owners_Email",
                table: "Owners");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_Email",
                table: "Owners",
                column: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Owners_Email",
                table: "Owners");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_Email",
                table: "Owners",
                column: "Email",
                unique: true);
        }
    }
}
