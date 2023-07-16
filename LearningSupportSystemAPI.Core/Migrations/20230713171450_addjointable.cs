using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningSupportSystemAPI.Core.Migrations
{
    /// <inheritdoc />
    public partial class addjointable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentClassToDoItem");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "ToDoItems",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateTable(
                name: "StudentTask",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ToDoItemId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentTask_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTask_ToDoItems_ToDoItemId",
                        column: x => x.ToDoItemId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentTask_StudentId",
                table: "StudentTask",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTask_ToDoItemId",
                table: "StudentTask",
                column: "ToDoItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentTask");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompleted",
                table: "ToDoItems",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "StudentClassToDoItem",
                columns: table => new
                {
                    ToDoItemsId = table.Column<int>(type: "int", nullable: false),
                    StudentsStudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StudentsClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentClassToDoItem", x => new { x.ToDoItemsId, x.StudentsStudentId, x.StudentsClassId });
                    table.ForeignKey(
                        name: "FK_StudentClassToDoItem_StudentClass_StudentsStudentId_StudentsClassId",
                        columns: x => new { x.StudentsStudentId, x.StudentsClassId },
                        principalTable: "StudentClass",
                        principalColumns: new[] { "StudentId", "ClassId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentClassToDoItem_ToDoItems_ToDoItemsId",
                        column: x => x.ToDoItemsId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentClassToDoItem_StudentsStudentId_StudentsClassId",
                table: "StudentClassToDoItem",
                columns: new[] { "StudentsStudentId", "StudentsClassId" });
        }
    }
}
