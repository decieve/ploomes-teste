using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ploomes_teste.persistence.Migrations
{
    public partial class TipoLugar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TiposLugar",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { (short)1, "Restaurante" },
                    { (short)2, "Academia" },
                    { (short)3, "Bar" },
                    { (short)4, "Igreja" },
                    { (short)5, "Oficina" },
                    { (short)6, "Escola" },
                    { (short)7, "Mercearia" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)5);

            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)6);

            migrationBuilder.DeleteData(
                table: "TiposLugar",
                keyColumn: "Id",
                keyValue: (short)7);
        }
    }
}
