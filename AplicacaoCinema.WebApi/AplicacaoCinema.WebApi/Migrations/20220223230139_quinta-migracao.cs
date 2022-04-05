using Microsoft.EntityFrameworkCore.Migrations;

namespace AplicacaoCinema.WebApi.Migrations
{
    public partial class quintamigracao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeIngressos",
                schema: "dbo",
                table: "Ingressos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeIngressos",
                schema: "dbo",
                table: "Ingressos");
        }
    }
}
