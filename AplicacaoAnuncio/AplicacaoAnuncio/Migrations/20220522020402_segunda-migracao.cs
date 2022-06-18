using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AplicacaoAnuncio.Migrations
{
    public partial class segundamigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UsuarioId",
                schema: "dbo",
                table: "Servicos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cpf = table.Column<string>(type: "varchar(18)", nullable: true),
                    Nome = table.Column<string>(type: "varchar(50)", nullable: true),
                    DataNascimento = table.Column<string>(type: "char(10)", nullable: true),
                    Sexo = table.Column<string>(type: "char(1)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataUltimaAlteracao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Servicos_UsuarioId",
                schema: "dbo",
                table: "Servicos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Servicos_Usuarios_UsuarioId",
                schema: "dbo",
                table: "Servicos",
                column: "UsuarioId",
                principalSchema: "dbo",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Servicos_Usuarios_UsuarioId",
                schema: "dbo",
                table: "Servicos");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Servicos_UsuarioId",
                schema: "dbo",
                table: "Servicos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                schema: "dbo",
                table: "Servicos");
        }
    }
}
