using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boutique.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTableDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produs_Tabel_TabelId",
                table: "Produs");

            migrationBuilder.DropTable(
                name: "Tabel");

            migrationBuilder.DropIndex(
                name: "IX_Produs_TabelId",
                table: "Produs");

            migrationBuilder.DropColumn(
                name: "TabelId",
                table: "Produs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TabelId",
                table: "Produs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tabel", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produs_TabelId",
                table: "Produs",
                column: "TabelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produs_Tabel_TabelId",
                table: "Produs",
                column: "TabelId",
                principalTable: "Tabel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
