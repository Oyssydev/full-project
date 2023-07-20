using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConverterAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etudiants_Classrooms_ClassroomclassID",
                table: "Etudiants");

            migrationBuilder.AlterColumn<int>(
                name: "ClassroomclassID",
                table: "Etudiants",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "classLocation",
                table: "Classrooms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Etudiants_Classrooms_ClassroomclassID",
                table: "Etudiants",
                column: "ClassroomclassID",
                principalTable: "Classrooms",
                principalColumn: "classID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etudiants_Classrooms_ClassroomclassID",
                table: "Etudiants");

            migrationBuilder.AlterColumn<int>(
                name: "ClassroomclassID",
                table: "Etudiants",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "classLocation",
                table: "Classrooms",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Etudiants_Classrooms_ClassroomclassID",
                table: "Etudiants",
                column: "ClassroomclassID",
                principalTable: "Classrooms",
                principalColumn: "classID");
        }
    }
}
