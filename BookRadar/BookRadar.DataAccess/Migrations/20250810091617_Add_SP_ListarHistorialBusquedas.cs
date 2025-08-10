using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookRadar.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Add_SP_ListarHistorialBusquedas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE ListarHistorialBusquedas 
                    @PageNumber INT,
                    @PageSize INT,
                    @TotalRows INT OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT @TotalRows = COUNT(*) 
                    FROM HistorialBusquedas;

                    SELECT Id, Autor, Titulo, AnioPublicacion, Editorial, FechaConsulta
                    FROM HistorialBusquedas
                    ORDER BY FechaConsulta DESC
                    OFFSET (@PageNumber - 1) * @PageSize ROWS
                    FETCH NEXT @PageSize ROWS ONLY;
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS ListarHistorialBusquedas;
                ");
        }
    }
}
