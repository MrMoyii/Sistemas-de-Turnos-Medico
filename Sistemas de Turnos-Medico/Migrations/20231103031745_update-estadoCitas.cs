using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemas_de_Turnos_Medico.Migrations
{
    /// <inheritdoc />
    public partial class updateestadoCitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctores_Estados_EstadoCitaId",
                table: "Doctores");

            migrationBuilder.DropIndex(
                name: "IX_Doctores_EstadoCitaId",
                table: "Doctores");

            migrationBuilder.DropColumn(
                name: "EstadoCitaId",
                table: "Doctores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoCitaId",
                table: "Doctores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctores_EstadoCitaId",
                table: "Doctores",
                column: "EstadoCitaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctores_Estados_EstadoCitaId",
                table: "Doctores",
                column: "EstadoCitaId",
                principalTable: "Estados",
                principalColumn: "Id");
        }
    }
}
