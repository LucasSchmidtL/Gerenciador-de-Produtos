using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecaoPerfilListaSuspensa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoSecao",
                table: "Perfis");

            migrationBuilder.AddColumn<int>(
                name: "SecaoId",
                table: "Perfis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_SecaoId",
                table: "Perfis",
                column: "SecaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_Secao_SecaoId",
                table: "Perfis",
                column: "SecaoId",
                principalTable: "Secao",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_Secao_SecaoId",
                table: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Perfis_SecaoId",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "SecaoId",
                table: "Perfis");

            migrationBuilder.AddColumn<string>(
                name: "TipoSecao",
                table: "Perfis",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
