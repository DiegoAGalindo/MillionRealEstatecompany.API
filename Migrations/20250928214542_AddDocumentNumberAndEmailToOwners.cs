using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MillionRealEstatecompany.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDocumentNumberAndEmailToOwners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eliminar datos existentes para evitar conflictos con el índice único
            migrationBuilder.Sql("DELETE FROM \"PropertyTraces\"");
            migrationBuilder.Sql("DELETE FROM \"PropertyImages\"");
            migrationBuilder.Sql("DELETE FROM \"Properties\"");
            migrationBuilder.Sql("DELETE FROM \"Owners\"");

            // Reiniciar las secuencias
            migrationBuilder.Sql("ALTER SEQUENCE \"Owners_IdOwner_seq\" RESTART WITH 1");
            migrationBuilder.Sql("ALTER SEQUENCE \"Properties_IdProperty_seq\" RESTART WITH 1");
            migrationBuilder.Sql("ALTER SEQUENCE \"PropertyImages_IdPropertyImage_seq\" RESTART WITH 1");
            migrationBuilder.Sql("ALTER SEQUENCE \"PropertyTraces_IdPropertyTrace_seq\" RESTART WITH 1");

            migrationBuilder.AddColumn<string>(
                name: "DocumentNumber",
                table: "Owners",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Owners",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_DocumentNumber",
                table: "Owners",
                column: "DocumentNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Owners_DocumentNumber",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "DocumentNumber",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Owners");
        }
    }
}
