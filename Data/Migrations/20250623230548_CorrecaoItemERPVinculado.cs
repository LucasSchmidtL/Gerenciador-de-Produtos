using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoItemERPVinculado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemERPId_Filho",
                table: "ItensERPCompostos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemERPId_Pai",
                table: "ItensERPCompostos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERPId",
                table: "ItensERPVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_GalvanizadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_PintadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_SemAcabamentoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_ZincadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensERPVinculados_ItemERP_GalvanizadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensERPVinculados_ItemERP_PintadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensERPVinculados_ItemERP_SemAcabamentoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensERPVinculados_ItemERP_ZincadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropColumn(
                name: "ItemERP_GalvanizadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropColumn(
                name: "ItemERP_PintadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropColumn(
                name: "ItemERP_SemAcabamentoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropColumn(
                name: "ItemERP_ZincadoId",
                table: "ItensERPVinculados");

            migrationBuilder.RenameColumn(
                name: "ItemERPId_Pai",
                table: "ItensERPCompostos",
                newName: "ItemPaiId");

            migrationBuilder.RenameColumn(
                name: "ItemERPId_Filho",
                table: "ItensERPCompostos",
                newName: "ItemFilhoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensERPCompostos_ItemERPId_Pai",
                table: "ItensERPCompostos",
                newName: "IX_ItensERPCompostos_ItemPaiId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensERPCompostos_ItemERPId_Filho",
                table: "ItensERPCompostos",
                newName: "IX_ItensERPCompostos_ItemFilhoId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemERPId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "ItensERPVinculados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VinculadoId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemERPId",
                table: "ItensERPCompostos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unidade",
                table: "ItensERPCompostos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPVinculados_VinculadoId",
                table: "ItensERPVinculados",
                column: "VinculadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPCompostos_ItemERPId",
                table: "ItensERPCompostos",
                column: "ItemERPId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemERPId",
                table: "ItensERPCompostos",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemFilhoId",
                table: "ItensERPCompostos",
                column: "ItemFilhoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemPaiId",
                table: "ItensERPCompostos",
                column: "ItemPaiId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemERPId",
                table: "ItensERPCompostos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemFilhoId",
                table: "ItensERPCompostos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemPaiId",
                table: "ItensERPCompostos");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERPId",
                table: "ItensERPVinculados");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_VinculadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensERPVinculados_VinculadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensERPCompostos_ItemERPId",
                table: "ItensERPCompostos");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "ItensERPVinculados");

            migrationBuilder.DropColumn(
                name: "VinculadoId",
                table: "ItensERPVinculados");

            migrationBuilder.DropColumn(
                name: "ItemERPId",
                table: "ItensERPCompostos");

            migrationBuilder.DropColumn(
                name: "Unidade",
                table: "ItensERPCompostos");

            migrationBuilder.RenameColumn(
                name: "ItemPaiId",
                table: "ItensERPCompostos",
                newName: "ItemERPId_Pai");

            migrationBuilder.RenameColumn(
                name: "ItemFilhoId",
                table: "ItensERPCompostos",
                newName: "ItemERPId_Filho");

            migrationBuilder.RenameIndex(
                name: "IX_ItensERPCompostos_ItemPaiId",
                table: "ItensERPCompostos",
                newName: "IX_ItensERPCompostos_ItemERPId_Pai");

            migrationBuilder.RenameIndex(
                name: "IX_ItensERPCompostos_ItemFilhoId",
                table: "ItensERPCompostos",
                newName: "IX_ItensERPCompostos_ItemERPId_Filho");

            migrationBuilder.AlterColumn<int>(
                name: "ItemERPId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ItemERP_GalvanizadoId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemERP_PintadoId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemERP_SemAcabamentoId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ItemERP_ZincadoId",
                table: "ItensERPVinculados",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPVinculados_ItemERP_GalvanizadoId",
                table: "ItensERPVinculados",
                column: "ItemERP_GalvanizadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPVinculados_ItemERP_PintadoId",
                table: "ItensERPVinculados",
                column: "ItemERP_PintadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPVinculados_ItemERP_SemAcabamentoId",
                table: "ItensERPVinculados",
                column: "ItemERP_SemAcabamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPVinculados_ItemERP_ZincadoId",
                table: "ItensERPVinculados",
                column: "ItemERP_ZincadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemERPId_Filho",
                table: "ItensERPCompostos",
                column: "ItemERPId_Filho",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPCompostos_ItensERP_ItemERPId_Pai",
                table: "ItensERPCompostos",
                column: "ItemERPId_Pai",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERPId",
                table: "ItensERPVinculados",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_GalvanizadoId",
                table: "ItensERPVinculados",
                column: "ItemERP_GalvanizadoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_PintadoId",
                table: "ItensERPVinculados",
                column: "ItemERP_PintadoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_SemAcabamentoId",
                table: "ItensERPVinculados",
                column: "ItemERP_SemAcabamentoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensERPVinculados_ItensERP_ItemERP_ZincadoId",
                table: "ItensERPVinculados",
                column: "ItemERP_ZincadoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
