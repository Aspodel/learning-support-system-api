using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningSupportSystemAPI.Core.Migrations
{
    /// <inheritdoc />
    public partial class discussionfeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Submissions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Discussions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Discussions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Discussions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Discussions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Discussions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Submissions_GroupId",
                table: "Submissions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_ClassId",
                table: "Discussions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_CreatorId",
                table: "Discussions",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Discussions_GroupId",
                table: "Discussions",
                column: "GroupId",
                unique: true,
                filter: "[GroupId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_AspNetUsers_CreatorId",
                table: "Discussions",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_Classes_ClassId",
                table: "Discussions",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discussions_Groups_GroupId",
                table: "Discussions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Submissions_Groups_GroupId",
                table: "Submissions",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_AspNetUsers_CreatorId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_Classes_ClassId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Discussions_Groups_GroupId",
                table: "Discussions");

            migrationBuilder.DropForeignKey(
                name: "FK_Submissions_Groups_GroupId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Submissions_GroupId",
                table: "Submissions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_ClassId",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_CreatorId",
                table: "Discussions");

            migrationBuilder.DropIndex(
                name: "IX_Discussions_GroupId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Discussions");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Discussions");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Discussions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
