using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class DesenhoERPNN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desenhos_ItensERP_ItemERPId",
                table: "Desenhos");

            migrationBuilder.DropIndex(
                name: "IX_Desenhos_ItemERPId",
                table: "Desenhos");

            migrationBuilder.DropColumn(
                name: "ItemERPId",
                table: "Desenhos");

            migrationBuilder.CreateTable(
                name: "DesenhoItemERPs",
                columns: table => new
                {
                    DesenhoId = table.Column<long>(type: "bigint", nullable: false),
                    ItemERPId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesenhoItemERPs", x => new { x.DesenhoId, x.ItemERPId });
                    table.ForeignKey(
                        name: "FK_DesenhoItemERPs_Desenhos_DesenhoId",
                        column: x => x.DesenhoId,
                        principalTable: "Desenhos",
                        principalColumn: "DesenhoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesenhoItemERPs_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DesenhoItemERPs_ItemERPId",
                table: "DesenhoItemERPs",
                column: "ItemERPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DesenhoItemERPs");

            migrationBuilder.AddColumn<int>(
                name: "ItemERPId",
                table: "Desenhos",
                type: "int",
                nullable: true);

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
        }
    }
}
