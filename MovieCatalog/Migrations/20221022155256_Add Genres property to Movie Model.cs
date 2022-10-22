using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalog.Migrations
{
    public partial class AddGenrespropertytoMovieModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MovieId",
                table: "genres",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_genres_MovieId",
                table: "genres",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_genres_movies_MovieId",
                table: "genres",
                column: "MovieId",
                principalTable: "movies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_genres_movies_MovieId",
                table: "genres");

            migrationBuilder.DropIndex(
                name: "IX_genres_MovieId",
                table: "genres");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "genres");
        }
    }
}
