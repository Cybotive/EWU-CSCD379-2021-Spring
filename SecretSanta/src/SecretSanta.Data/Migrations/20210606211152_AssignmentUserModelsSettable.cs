using Microsoft.EntityFrameworkCore.Migrations;

namespace SecretSanta.Data.Migrations
{
    public partial class AssignmentUserModelsSettable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "Assignment",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "GiverId",
                table: "Assignment",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_GiverId",
                table: "Assignment",
                column: "GiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_ReceiverId",
                table: "Assignment",
                column: "ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Users_GiverId",
                table: "Assignment",
                column: "GiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Assignment_Users_ReceiverId",
                table: "Assignment",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Users_GiverId",
                table: "Assignment");

            migrationBuilder.DropForeignKey(
                name: "FK_Assignment_Users_ReceiverId",
                table: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_GiverId",
                table: "Assignment");

            migrationBuilder.DropIndex(
                name: "IX_Assignment_ReceiverId",
                table: "Assignment");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiverId",
                table: "Assignment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GiverId",
                table: "Assignment",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
