using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsereNomeRemoveNridMridComponente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MrId",
                table: "Componentes");

            migrationBuilder.DropColumn(
                name: "NrId",
                table: "Componentes");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Componentes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Componentes_Nome",
                table: "Componentes",
                column: "Nome",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Componentes_Nome",
                table: "Componentes");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Componentes");

            migrationBuilder.AddColumn<long>(
                name: "MrId",
                table: "Componentes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NrId",
                table: "Componentes",
                type: "bigint",
                nullable: true);
        }
    }
}
