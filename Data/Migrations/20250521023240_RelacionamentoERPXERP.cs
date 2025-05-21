using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoERPXERP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemERPRelacionados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemERPId = table.Column<int>(type: "int", nullable: false),
                    RelacionadoId = table.Column<int>(type: "int", nullable: false),
                    DesenhoId = table.Column<long>(type: "bigint", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemERPRelacionados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemERPRelacionados_Desenhos_DesenhoId",
                        column: x => x.DesenhoId,
                        principalTable: "Desenhos",
                        principalColumn: "DesenhoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemERPRelacionados_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemERPRelacionados_ItensERP_RelacionadoId",
                        column: x => x.RelacionadoId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemERPRelacionados_DesenhoId",
                table: "ItemERPRelacionados",
                column: "DesenhoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemERPRelacionados_ItemERPId",
                table: "ItemERPRelacionados",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemERPRelacionados_RelacionadoId",
                table: "ItemERPRelacionados",
                column: "RelacionadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemERPRelacionados");
        }
    }
}
