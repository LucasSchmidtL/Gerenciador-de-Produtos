using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class PerfilVariosDesenhos2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_Desenhos_DesenhoId",
                table: "Perfis");

            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "Desenhos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DesenhoPerfil",
                columns: table => new
                {
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    DesenhoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesenhoPerfil", x => new { x.PerfilId, x.DesenhoId });
                    table.ForeignKey(
                        name: "FK_DesenhoPerfil_Desenhos_DesenhoId",
                        column: x => x.DesenhoId,
                        principalTable: "Desenhos",
                        principalColumn: "DesenhoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DesenhoPerfil_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Desenhos_PerfilId",
                table: "Desenhos",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_DesenhoPerfil_DesenhoId",
                table: "DesenhoPerfil",
                column: "DesenhoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Desenhos_Perfis_PerfilId",
                table: "Desenhos",
                column: "PerfilId",
                principalTable: "Perfis",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_Desenhos_DesenhoId",
                table: "Perfis",
                column: "DesenhoId",
                principalTable: "Desenhos",
                principalColumn: "DesenhoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Desenhos_Perfis_PerfilId",
                table: "Desenhos");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_Desenhos_DesenhoId",
                table: "Perfis");

            migrationBuilder.DropTable(
                name: "DesenhoPerfil");

            migrationBuilder.DropIndex(
                name: "IX_Desenhos_PerfilId",
                table: "Desenhos");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Desenhos");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_Desenhos_DesenhoId",
                table: "Perfis",
                column: "DesenhoId",
                principalTable: "Desenhos",
                principalColumn: "DesenhoId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
