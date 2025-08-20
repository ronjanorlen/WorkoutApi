using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTrainingTypeToWorkout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainingType",
                table: "Workouts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingType",
                table: "Workouts");
        }
    }
}
