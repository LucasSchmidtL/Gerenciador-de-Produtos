﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gerenciador_de_Produtos.Data.Migrations
{
    /// <inheritdoc />
    public partial class InsereDescricaoComponente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Componentes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Componentes");
        }
    }
}
