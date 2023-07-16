using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningSupportSystemAPI.Core.Migrations
{
    /// <inheritdoc />
    public partial class relationshipQuestionAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionId2",
                table: "Answers");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionId2",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionId2",
                table: "Answers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionId2",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId2",
                table: "Answers",
                column: "QuestionId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionId2",
                table: "Answers",
                column: "QuestionId2",
                principalTable: "Questions",
                principalColumn: "Id");
        }
    }
}
