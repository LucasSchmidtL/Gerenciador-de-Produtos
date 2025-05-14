using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class ItemERPTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_ItensERP_ItemERPId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ItemERPId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ItemERPId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "ItemERPTag",
                columns: table => new
                {
                    ItemERPsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemERPTag", x => new { x.ItemERPsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ItemERPTag_ItensERP_ItemERPsId",
                        column: x => x.ItemERPsId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemERPTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemERPTag_TagsId",
                table: "ItemERPTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemERPTag");

            migrationBuilder.AddColumn<int>(
                name: "ItemERPId",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ItemERPId",
                table: "Tags",
                column: "ItemERPId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_ItensERP_ItemERPId",
                table: "Tags",
                column: "ItemERPId",
                principalTable: "ItensERP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
