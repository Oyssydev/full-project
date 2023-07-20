using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConverterAPI.Migrations
{
    /// <inheritdoc />
    public partial class Remove_AddClassroomEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassroomclassID",
                table: "Etudiants",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    classID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    classLocation = table.Column<int>(type: "int", nullable: false),
                    classNbTables = table.Column<int>(type: "int", nullable: false),
                    classNbChaires = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.classID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Etudiants_ClassroomclassID",
                table: "Etudiants",
                column: "ClassroomclassID");

            migrationBuilder.AddForeignKey(
                name: "FK_Etudiants_Classrooms_ClassroomclassID",
                table: "Etudiants",
                column: "ClassroomclassID",
                principalTable: "Classrooms",
                principalColumn: "classID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etudiants_Classrooms_ClassroomclassID",
                table: "Etudiants");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Etudiants_ClassroomclassID",
                table: "Etudiants");

            migrationBuilder.DropColumn(
                name: "ClassroomclassID",
                table: "Etudiants");
        }
    }
}
