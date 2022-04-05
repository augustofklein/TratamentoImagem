using Microsoft.EntityFrameworkCore.Migrations;

namespace AplicacaoCinema.WebApi.Migrations
{
    public partial class quartamigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeIngressos",
                schema: "dbo",
                table: "Sessoes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeIngressos",
                schema: "dbo",
                table: "Sessoes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
