using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ploomes_teste.persistence.Migrations
{
    public partial class TipoLugarReferencia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_TiposLugar_TipoLugarId",
                table: "Lugares");

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_TiposLugar_TipoLugarId",
                table: "Lugares",
                column: "TipoLugarId",
                principalTable: "TiposLugar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lugares_TiposLugar_TipoLugarId",
                table: "Lugares");

            migrationBuilder.AddForeignKey(
                name: "FK_Lugares_TiposLugar_TipoLugarId",
                table: "Lugares",
                column: "TipoLugarId",
                principalTable: "TiposLugar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
