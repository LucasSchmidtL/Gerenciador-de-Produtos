using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdivinhaItemERPVinculado3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DesenhoId",
                table: "ItensVinculados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DesenhoId1",
                table: "ItensVinculados",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensVinculados_DesenhoId1",
                table: "ItensVinculados",
                column: "DesenhoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVinculados_Desenhos_DesenhoId1",
                table: "ItensVinculados",
                column: "DesenhoId1",
                principalTable: "Desenhos",
                principalColumn: "DesenhoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensVinculados_Desenhos_DesenhoId1",
                table: "ItensVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensVinculados_DesenhoId1",
                table: "ItensVinculados");

            migrationBuilder.DropColumn(
                name: "DesenhoId",
                table: "ItensVinculados");

            migrationBuilder.DropColumn(
                name: "DesenhoId1",
                table: "ItensVinculados");
        }
    }
}
