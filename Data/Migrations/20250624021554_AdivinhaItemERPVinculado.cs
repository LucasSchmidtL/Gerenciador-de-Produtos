using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdivinhaItemERPVinculado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensVinculados_ItensERP_DescricaoId",
                table: "ItensVinculados");

            migrationBuilder.DropIndex(
                name: "IX_ItensVinculados_DescricaoId",
                table: "ItensVinculados");

            migrationBuilder.DropColumn(
                name: "DescricaoId",
                table: "ItensVinculados");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DescricaoId",
                table: "ItensVinculados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItensVinculados_DescricaoId",
                table: "ItensVinculados",
                column: "DescricaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensVinculados_ItensERP_DescricaoId",
                table: "ItensVinculados",
                column: "DescricaoId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
