using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AplicacaoAnuncio.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Servicos",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomeServico = table.Column<string>(type: "varchar(50)", nullable: true),
                    Descricao = table.Column<string>(type: "varchar(50)", nullable: true),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(38,17)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Servicos",
                schema: "dbo");
        }
    }
}
