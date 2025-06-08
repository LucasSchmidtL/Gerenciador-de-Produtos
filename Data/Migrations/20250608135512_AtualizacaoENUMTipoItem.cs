using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AtualizacaoENUMTipoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoItem",
                table: "ItensERP",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TipoItem",
                table: "ItensERP",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");
        }
    }
}
