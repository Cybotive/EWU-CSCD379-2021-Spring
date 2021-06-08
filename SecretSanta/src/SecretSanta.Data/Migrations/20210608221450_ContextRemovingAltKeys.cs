using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretSanta.Data.Migrations
{
    public partial class ContextRemovingAltKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_FirstName_LastName",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Groups_Name",
                table: "Groups");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Gifts_Title",
                table: "Gifts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_FirstName_LastName",
                table: "Users",
                columns: new[] { "FirstName", "LastName" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Groups_Name",
                table: "Groups",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Gifts_Title",
                table: "Gifts",
                column: "Title");
        }
    }
}
