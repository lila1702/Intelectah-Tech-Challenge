using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarDealershipManager.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _001_AddFilteredIndexToFabricante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fabricantes_Nome",
                table: "Fabricantes");

            migrationBuilder.DropIndex(
                name: "IX_Concessionarias_Nome",
                table: "Concessionarias");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_CPF",
                table: "Clientes");

            migrationBuilder.CreateIndex(
                name: "IX_Fabricantes_Nome",
                table: "Fabricantes",
                column: "Nome",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Concessionarias_Nome",
                table: "Concessionarias",
                column: "Nome",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CPF",
                table: "Clientes",
                column: "CPF",
                unique: true,
                filter: "[IsDeleted] = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Fabricantes_Nome",
                table: "Fabricantes");

            migrationBuilder.DropIndex(
                name: "IX_Concessionarias_Nome",
                table: "Concessionarias");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_CPF",
                table: "Clientes");

            migrationBuilder.CreateIndex(
                name: "IX_Fabricantes_Nome",
                table: "Fabricantes",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Concessionarias_Nome",
                table: "Concessionarias",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CPF",
                table: "Clientes",
                column: "CPF",
                unique: true);
        }
    }
}
