using Microsoft.EntityFrameworkCore.Migrations;

namespace ServidorApostas.Migrations
{
    public partial class Passo1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Utilizador",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        nome = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Utilizador", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Apostas",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        chave = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        data = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
            //        registada = table.Column<bool>(type: "bit", nullable: false),
            //        utilizadorid = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Apostas", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Apostas_Utilizador_utilizadorid",
            //            column: x => x.utilizadorid,
            //            principalTable: "Utilizador",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Apostas_utilizadorid",
            //    table: "Apostas",
            //    column: "utilizadorid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "Apostas");

            //migrationBuilder.DropTable(
            //    name: "Utilizador");
        }
    }
}
