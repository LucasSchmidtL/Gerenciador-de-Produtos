using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovidoNpara1PerfilxERP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_ItensERP_ItemERPId",
                table: "Perfis");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_ItensERP_ItemERPId",
                table: "Perfis",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_ItensERP_ItemERPId",
                table: "Perfis");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_ItensERP_ItemERPId",
                table: "Perfis",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
