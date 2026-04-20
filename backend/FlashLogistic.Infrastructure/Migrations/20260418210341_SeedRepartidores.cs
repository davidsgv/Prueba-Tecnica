using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashLogistic.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRepartidores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Guid id = Guid.NewGuid();

            migrationBuilder.Sql($"""
                INSERT INTO Repartidores (Id, Nombre) VALUES
                (NEWID(), 'David'),
                (NEWID(), 'Fabian'),
                (NEWID(), 'Ana'),
                (NEWID(), 'Edgar'),
                (NEWID(), 'Ernesto'),
                (NEWID(), 'Camilo');
            """);

            migrationBuilder.Sql($"""
                DECLARE @Id UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Repartidores WHERE Nombre = 'David');
                INSERT INTO Paquetes (Id, Descripcion, Peso, Codigo, Estado, Prioridad, RepartidorId) VALUES
                (NEWID(), 'Paquete de prueba con una descripción larga', 2.5, 'SEED-1', 1, 1, @Id);
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
