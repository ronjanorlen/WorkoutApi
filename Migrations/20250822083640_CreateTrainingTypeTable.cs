using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateTrainingTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingType",
                table: "Workouts");

            migrationBuilder.AddColumn<int>(
                name: "TrainingTypeId",
                table: "Workouts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrainingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrainingTypeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workouts_TrainingTypeId",
                table: "Workouts",
                column: "TrainingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workouts_TrainingTypes_TrainingTypeId",
                table: "Workouts",
                column: "TrainingTypeId",
                principalTable: "TrainingTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workouts_TrainingTypes_TrainingTypeId",
                table: "Workouts");

            migrationBuilder.DropTable(
                name: "TrainingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Workouts_TrainingTypeId",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "TrainingTypeId",
                table: "Workouts");

            migrationBuilder.AddColumn<string>(
                name: "TrainingType",
                table: "Workouts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
