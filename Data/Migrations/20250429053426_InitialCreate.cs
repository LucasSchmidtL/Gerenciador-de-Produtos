using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Agrupadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grupo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesenvolvimentoId = table.Column<int>(type: "int", nullable: true),
                    ItemERPId = table.Column<int>(type: "int", nullable: true),
                    AgrupadorPaiId = table.Column<int>(type: "int", nullable: true),
                    Nivel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agrupadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Componentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nivel = table.Column<int>(type: "int", nullable: true),
                    NrId = table.Column<long>(type: "bigint", nullable: true),
                    MrId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Componentes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Desenhos",
                columns: table => new
                {
                    DesenhoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revisao = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Classificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SolicitacaoAlteracaoId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Desenhos", x => x.DesenhoId);
                });

            migrationBuilder.CreateTable(
                name: "ItensERP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ERP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoItem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemERPIdOriginal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesenhoId = table.Column<long>(type: "bigint", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Revisao = table.Column<long>(type: "bigint", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Acabamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChapaAberta = table.Column<long>(type: "bigint", nullable: true),
                    AreaSuperficial = table.Column<float>(type: "real", nullable: true),
                    PesoLiquidoMetro = table.Column<float>(type: "real", nullable: true),
                    PesoBrutoMetro = table.Column<float>(type: "real", nullable: true),
                    PerimetroSolda = table.Column<float>(type: "real", nullable: true),
                    SRId = table.Column<long>(type: "bigint", nullable: true),
                    DesenvolvimentoId = table.Column<int>(type: "int", nullable: true),
                    QuantidadeDobras = table.Column<int>(type: "int", nullable: true),
                    MateriaPrimaId = table.Column<int>(type: "int", nullable: true),
                    TagId = table.Column<long>(type: "bigint", nullable: true),
                    Altura = table.Column<float>(type: "real", nullable: true),
                    Comprimento = table.Column<float>(type: "real", nullable: true),
                    Profundidade = table.Column<float>(type: "real", nullable: true),
                    ComprimentoMaximo = table.Column<float>(type: "real", nullable: true),
                    Passo = table.Column<int>(type: "int", nullable: true),
                    Classificacao = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensERP", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ERP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Desenho = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoSecao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Peso = table.Column<float>(type: "real", nullable: true),
                    AreaBruta = table.Column<float>(type: "real", nullable: true),
                    AreaLiq = table.Column<float>(type: "real", nullable: true),
                    AreaEq = table.Column<float>(type: "real", nullable: true),
                    Ix = table.Column<float>(type: "real", nullable: true),
                    Sxt = table.Column<float>(type: "real", nullable: true),
                    Sxb = table.Column<float>(type: "real", nullable: true),
                    Zx = table.Column<float>(type: "real", nullable: true),
                    Rx = table.Column<float>(type: "real", nullable: true),
                    yt = table.Column<float>(type: "real", nullable: true),
                    yb = table.Column<float>(type: "real", nullable: true),
                    Ixy = table.Column<float>(type: "real", nullable: true),
                    Iy = table.Column<float>(type: "real", nullable: true),
                    Syl = table.Column<float>(type: "real", nullable: true),
                    Syr = table.Column<float>(type: "real", nullable: true),
                    Zy = table.Column<float>(type: "real", nullable: true),
                    ry = table.Column<float>(type: "real", nullable: true),
                    xl = table.Column<float>(type: "real", nullable: true),
                    xr = table.Column<float>(type: "real", nullable: true),
                    xo = table.Column<float>(type: "real", nullable: true),
                    yo = table.Column<float>(type: "real", nullable: true),
                    jx = table.Column<float>(type: "real", nullable: true),
                    jy = table.Column<float>(type: "real", nullable: true),
                    Cw = table.Column<float>(type: "real", nullable: true),
                    J = table.Column<float>(type: "real", nullable: true),
                    Ixe = table.Column<float>(type: "real", nullable: true),
                    Sxet = table.Column<float>(type: "real", nullable: true),
                    Sxeb = table.Column<float>(type: "real", nullable: true),
                    lye = table.Column<float>(type: "real", nullable: true),
                    Syel = table.Column<float>(type: "real", nullable: true),
                    Syer = table.Column<float>(type: "real", nullable: true),
                    p1 = table.Column<float>(type: "real", nullable: true),
                    p2 = table.Column<float>(type: "real", nullable: true),
                    p3 = table.Column<float>(type: "real", nullable: true),
                    SimetricoX = table.Column<bool>(type: "bit", nullable: true),
                    SimetricoY = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Familia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeComercial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Precificacao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesenvolvimentoId = table.Column<int>(type: "int", nullable: true),
                    Nivel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VariaveisAgrupadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgrupadorId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariaveisAgrupadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariaveisAgrupadores_Agrupadores_AgrupadorId",
                        column: x => x.AgrupadorId,
                        principalTable: "Agrupadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgrupadorComponentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgrupadorId = table.Column<int>(type: "int", nullable: false),
                    ComponenteId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<float>(type: "real", nullable: true),
                    Comprimento = table.Column<float>(type: "real", nullable: true),
                    Profundidade = table.Column<float>(type: "real", nullable: true),
                    Altura = table.Column<float>(type: "real", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgrupadorComponentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgrupadorComponentes_Agrupadores_AgrupadorId",
                        column: x => x.AgrupadorId,
                        principalTable: "Agrupadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgrupadorComponentes_Componentes_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariaveisComponentes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComponenteId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariaveisComponentes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariaveisComponentes_Componentes_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgrupadorItemERPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgrupadorId = table.Column<int>(type: "int", nullable: false),
                    ItemERPId = table.Column<int>(type: "int", nullable: false),
                    Comprimento = table.Column<float>(type: "real", nullable: true),
                    Altura = table.Column<float>(type: "real", nullable: true),
                    Profundidade = table.Column<float>(type: "real", nullable: true),
                    Quantidade = table.Column<float>(type: "real", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgrupadorItemERPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgrupadorItemERPs_Agrupadores_AgrupadorId",
                        column: x => x.AgrupadorId,
                        principalTable: "Agrupadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgrupadorItemERPs_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponenteItemERPs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponenteId = table.Column<int>(type: "int", nullable: false),
                    ItemERPId = table.Column<int>(type: "int", nullable: false),
                    Comprimento = table.Column<float>(type: "real", nullable: true),
                    Profundidade = table.Column<float>(type: "real", nullable: true),
                    Altura = table.Column<float>(type: "real", nullable: true),
                    Quantidade = table.Column<float>(type: "real", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponenteItemERPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponenteItemERPs_Componentes_ComponenteId",
                        column: x => x.ComponenteId,
                        principalTable: "Componentes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComponenteItemERPs_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemERPId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_ItensERP_ItemERPId",
                        column: x => x.ItemERPId,
                        principalTable: "ItensERP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutoAgrupadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    AgrupadorId = table.Column<int>(type: "int", nullable: false),
                    Variavel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoAgrupadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdutoAgrupadores_Agrupadores_AgrupadorId",
                        column: x => x.AgrupadorId,
                        principalTable: "Agrupadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoAgrupadores_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariaveisProdutos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariaveisProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariaveisProdutos_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgrupadorComponentes_AgrupadorId",
                table: "AgrupadorComponentes",
                column: "AgrupadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AgrupadorComponentes_ComponenteId",
                table: "AgrupadorComponentes",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_AgrupadorItemERPs_AgrupadorId",
                table: "AgrupadorItemERPs",
                column: "AgrupadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AgrupadorItemERPs_ItemERPId",
                table: "AgrupadorItemERPs",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponenteItemERPs_ComponenteId",
                table: "ComponenteItemERPs",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponenteItemERPs_ItemERPId",
                table: "ComponenteItemERPs",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoAgrupadores_AgrupadorId",
                table: "ProdutoAgrupadores",
                column: "AgrupadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoAgrupadores_ProdutoId",
                table: "ProdutoAgrupadores",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ItemERPId",
                table: "Tags",
                column: "ItemERPId");

            migrationBuilder.CreateIndex(
                name: "IX_VariaveisAgrupadores_AgrupadorId",
                table: "VariaveisAgrupadores",
                column: "AgrupadorId");

            migrationBuilder.CreateIndex(
                name: "IX_VariaveisComponentes_ComponenteId",
                table: "VariaveisComponentes",
                column: "ComponenteId");

            migrationBuilder.CreateIndex(
                name: "IX_VariaveisProdutos_ProdutoId",
                table: "VariaveisProdutos",
                column: "ProdutoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgrupadorComponentes");

            migrationBuilder.DropTable(
                name: "AgrupadorItemERPs");

            migrationBuilder.DropTable(
                name: "ComponenteItemERPs");

            migrationBuilder.DropTable(
                name: "Desenhos");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropTable(
                name: "ProdutoAgrupadores");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "VariaveisAgrupadores");

            migrationBuilder.DropTable(
                name: "VariaveisComponentes");

            migrationBuilder.DropTable(
                name: "VariaveisProdutos");

            migrationBuilder.DropTable(
                name: "ItensERP");

            migrationBuilder.DropTable(
                name: "Agrupadores");

            migrationBuilder.DropTable(
                name: "Componentes");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }
    }
}
