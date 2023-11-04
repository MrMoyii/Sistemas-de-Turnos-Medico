using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sistemas_de_Turnos_Medico.Migrations
{
    /// <inheritdoc />
    public partial class EstadosCitas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoCitaId",
                table: "Doctores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoId",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doctores_EstadoCitaId",
                table: "Doctores",
                column: "EstadoCitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Citas_EstadoId",
                table: "Citas",
                column: "EstadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Estados_EstadoId",
                table: "Citas",
                column: "EstadoId",
                principalTable: "Estados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctores_Estados_EstadoCitaId",
                table: "Doctores",
                column: "EstadoCitaId",
                principalTable: "Estados",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Estados_EstadoId",
                table: "Citas");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctores_Estados_EstadoCitaId",
                table: "Doctores");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropIndex(
                name: "IX_Doctores_EstadoCitaId",
                table: "Doctores");

            migrationBuilder.DropIndex(
                name: "IX_Citas_EstadoId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "EstadoCitaId",
                table: "Doctores");

            migrationBuilder.DropColumn(
                name: "EstadoId",
                table: "Citas");
        }
    }
}
