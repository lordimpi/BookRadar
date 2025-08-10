using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRadar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class HistorialBusqueda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistorialBusquedas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Autor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: true),
                    Editorial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    FechaConsulta = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialBusquedas", x => x.Id);
                });

            migrationBuilder.CreateIndex(
            name: "IX_HistorialBusquedas_Autor_Titulo_FechaConsulta",
            table: "HistorialBusquedas",
            columns: new[] { "Autor", "Titulo", "FechaConsulta" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistorialBusquedas");
        }
    }
}
