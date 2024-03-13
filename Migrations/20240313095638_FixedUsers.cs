using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BU2Todo.Migrations
{
    /// <inheritdoc />
    public partial class FixedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoLists",
                table: "TodoLists");

            migrationBuilder.RenameTable(
                name: "TodoLists",
                newName: "TodoItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoItems_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TodoItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_TodoItems_TodoListId",
                table: "Todos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoItems",
                table: "TodoItems");

            migrationBuilder.RenameTable(
                name: "TodoItems",
                newName: "TodoLists");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoLists",
                table: "TodoLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_TodoLists_TodoListId",
                table: "Todos",
                column: "TodoListId",
                principalTable: "TodoLists",
                principalColumn: "Id");
        }
    }
}
