using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoItensVinculados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERPId",
                table: "ItensERPVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_VinculadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensERPVinculados",
                table: "ItensERPVinculados");

            migrationBuilder.RenameTable(
                name: "ItensERPVinculados",
                newName: "ItensVinculados");

            migrationBuilder.RenameIndex(
                name: "IX_ItensERPVinculados_VinculadoId",
                table: "ItensVinculados",
                newName: "IX_ItensVinculados_VinculadoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensERPVinculados_ItemERPId",
                table: "ItensVinculados",
                newName: "IX_ItensVinculados_ItemERPId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensVinculados",
                table: "ItensVinculados",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVinculados_ItensERP_ItemERPId",
                table: "ItensVinculados",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVinculados_ItensERP_VinculadoId",
                table: "ItensVinculados",
                column: "VinculadoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensVinculados_ItensERP_ItemERPId",
                table: "ItensVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensVinculados_ItensERP_VinculadoId",
                table: "ItensVinculados");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensVinculados",
                table: "ItensVinculados");

            migrationBuilder.RenameTable(
                name: "ItensVinculados",
                newName: "ItensERPVinculados");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVinculados_VinculadoId",
                table: "ItensERPVinculados",
                newName: "IX_ItensERPVinculados_VinculadoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensVinculados_ItemERPId",
                table: "ItensERPVinculados",
                newName: "IX_ItensERPVinculados_ItemERPId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensERPVinculados",
                table: "ItensERPVinculados",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERPId",
                table: "ItensERPVinculados",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_VinculadoId",
                table: "ItensERPVinculados",
                column: "VinculadoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
