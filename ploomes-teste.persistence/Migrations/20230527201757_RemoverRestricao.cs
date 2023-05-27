using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ploomes_teste.persistence.Migrations
{
    public partial class RemoverRestricao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lugares_Cnpj",
                table: "Lugares");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_LugarId_AvaliadorId",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Lugares",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_LugarId",
                table: "Avaliacoes",
                column: "LugarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_LugarId",
                table: "Avaliacoes");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Lugares",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Lugares_Cnpj",
                table: "Lugares",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_LugarId_AvaliadorId",
                table: "Avaliacoes",
                columns: new[] { "LugarId", "AvaliadorId" },
                unique: true);
        }
    }
}
