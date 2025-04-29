using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecaoNormaEquacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equacao",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Funcao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormaId = table.Column<long>(type: "bigint", nullable: true),
                    Entrada = table.Column<long>(type: "bigint", nullable: true),
                    Saida = table.Column<long>(type: "bigint", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Consideracoes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Norma",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Equacoes = table.Column<long>(type: "bigint", nullable: true),
                    Revisao = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Norma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Secao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimetricoX = table.Column<bool>(type: "bit", nullable: false),
                    SimetricoY = table.Column<bool>(type: "bit", nullable: false),
                    PontoSimetrico = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Secao", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Equacao");

            migrationBuilder.DropTable(
                name: "Norma");

            migrationBuilder.DropTable(
                name: "Secao");
        }
    }
}
