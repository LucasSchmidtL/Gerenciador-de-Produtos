using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class PerfilVariosDesenhos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desenho",
                table: "Perfis");

            migrationBuilder.AddColumn<long>(
                name: "DesenhoId",
                table: "Perfis",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_DesenhoId",
                table: "Perfis",
                column: "DesenhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_Desenhos_DesenhoId",
                table: "Perfis",
                column: "DesenhoId",
                principalTable: "Desenhos",
                principalColumn: "DesenhoId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_Desenhos_DesenhoId",
                table: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Perfis_DesenhoId",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "DesenhoId",
                table: "Perfis");

            migrationBuilder.AddColumn<string>(
                name: "Desenho",
                table: "Perfis",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
