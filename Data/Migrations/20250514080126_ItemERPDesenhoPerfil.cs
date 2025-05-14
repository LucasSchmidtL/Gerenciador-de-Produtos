using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemERPDesenhoPerfil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesenhoId",
                table: "ItensERP");

            migrationBuilder.AddColumn<int>(
                name: "ItemERPId",
                table: "Perfis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemERPId",
                table: "Desenhos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_ItemERPId",
                table: "Perfis",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_Desenhos_ItemERPId",
                table: "Desenhos",
                column: "ItemERPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Desenhos_ItensERP_ItemERPId",
                table: "Desenhos",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_ItensERP_ItemERPId",
                table: "Perfis",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desenhos_ItensERP_ItemERPId",
                table: "Desenhos");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_ItensERP_ItemERPId",
                table: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Perfis_ItemERPId",
                table: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_Desenhos_ItemERPId",
                table: "Desenhos");

            migrationBuilder.DropColumn(
                name: "ItemERPId",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "ItemERPId",
                table: "Desenhos");

            migrationBuilder.AddColumn<long>(
                name: "DesenhoId",
                table: "ItensERP",
                type: "bigint",
                nullable: true);
        }
    }
}
