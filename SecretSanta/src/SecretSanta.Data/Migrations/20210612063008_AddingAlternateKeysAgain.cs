using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretSanta.Data.Migrations
{
    public partial class AddingAlternateKeysAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Gifts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Priority",
                table: "Gifts",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
