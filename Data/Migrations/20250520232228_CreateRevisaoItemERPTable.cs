using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateRevisaoItemERPTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevisaoItemERPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<long>(type: "bigint", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ItemERPId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisaoItemERPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevisaoItemERPs_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevisaoItemERPs_ItemERPId",
                table: "RevisaoItemERPs",
                column: "ItemERPId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevisaoItemERPs");
        }
    }
}
