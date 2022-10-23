using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalog.Migrations
{
    public partial class AddAvatarLinkpropertytoUsermodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarLink",
                table: "Users",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarLink",
                table: "Users");
        }
    }
}
