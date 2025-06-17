using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoRelacionamentosItensERP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItensERPCompostos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemERPId_Pai = table.Column<int>(type: "int", nullable: false),
                    ItemERPId_Filho = table.Column<int>(type: "int", nullable: false),
                    Comprimento = table.Column<float>(type: "real", nullable: true),
                    Profundidade = table.Column<float>(type: "real", nullable: true),
                    Altura = table.Column<float>(type: "real", nullable: true),
                    Quantidade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensERPCompostos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensERPCompostos_ItensERP_ItemERPId_Filho",
                        column: x => x.ItemERPId_Filho,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensERPCompostos_ItensERP_ItemERPId_Pai",
                        column: x => x.ItemERPId_Pai,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItensERPVinculados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemERP_SemAcabamentoId = table.Column<int>(type: "int", nullable: true),
                    ItemERP_PintadoId = table.Column<int>(type: "int", nullable: true),
                    ItemERP_GalvanizadoId = table.Column<int>(type: "int", nullable: true),
                    ItemERP_ZincadoId = table.Column<int>(type: "int", nullable: true),
                    ItemERPId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensERPVinculados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensERPVinculados_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItensERPVinculados_ItensERP_ItemERP_GalvanizadoId",
                        column: x => x.ItemERP_GalvanizadoId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensERPVinculados_ItensERP_ItemERP_PintadoId",
                        column: x => x.ItemERP_PintadoId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensERPVinculados_ItensERP_ItemERP_SemAcabamentoId",
                        column: x => x.ItemERP_SemAcabamentoId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItensERPVinculados_ItensERP_ItemERP_ZincadoId",
                        column: x => x.ItemERP_ZincadoId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VariaveisItemERPCompostos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemERPCompostoId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemERPId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariaveisItemERPCompostos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariaveisItemERPCompostos_ItensERPCompostos_ItemERPCompostoId",
                        column: x => x.ItemERPCompostoId,
                        principalTable: "ItensERPCompostos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariaveisItemERPCompostos_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPCompostos_ItemERPId_Filho",
                table: "ItensERPCompostos",
                column: "ItemERPId_Filho");

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPCompostos_ItemERPId_Pai",
                table: "ItensERPCompostos",
                column: "ItemERPId_Pai");

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

            migrationBuilder.CreateIndex(
                name: "IX_ItensERPVinculados_ItemERPId",
                table: "ItensERPVinculados",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_VariaveisItemERPCompostos_ItemERPCompostoId",
                table: "VariaveisItemERPCompostos",
                column: "ItemERPCompostoId");

            migrationBuilder.CreateIndex(
                name: "IX_VariaveisItemERPCompostos_ItemERPId",
                table: "VariaveisItemERPCompostos",
                column: "ItemERPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensERPVinculados");

            migrationBuilder.DropTable(
                name: "VariaveisItemERPCompostos");

            migrationBuilder.DropTable(
                name: "ItensERPCompostos");
        }
    }
}
