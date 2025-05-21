using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class AcoItemERP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Aco",
                table: "ItensERP",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aco",
                table: "ItensERP");
        }
    }
}
