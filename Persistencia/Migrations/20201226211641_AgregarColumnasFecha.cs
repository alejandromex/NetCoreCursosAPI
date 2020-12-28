using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class AgregarColumnasFecha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Instructores",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Cursos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Comentarios",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Instructores");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Cursos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Comentarios");
        }
    }
}
