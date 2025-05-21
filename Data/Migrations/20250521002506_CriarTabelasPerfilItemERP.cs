using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelasPerfilItemERP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerfilItemERPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemERPId = table.Column<int>(type: "int", nullable: false),
                    PerfilId = table.Column<int>(type: "int", nullable: false),
                    Aco = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilItemERPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilItemERPs_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilItemERPs_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RevisaoPerfilItemERPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfilItemERPId = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisaoPerfilItemERPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevisaoPerfilItemERPs_PerfilItemERPs_PerfilItemERPId",
                        column: x => x.PerfilItemERPId,
                        principalTable: "PerfilItemERPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerfilItemERPs_ItemERPId",
                table: "PerfilItemERPs",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilItemERPs_PerfilId",
                table: "PerfilItemERPs",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_RevisaoPerfilItemERPs_PerfilItemERPId",
                table: "RevisaoPerfilItemERPs",
                column: "PerfilItemERPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevisaoPerfilItemERPs");

            migrationBuilder.DropTable(
                name: "PerfilItemERPs");
        }
    }
}
