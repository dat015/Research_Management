using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class update3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "milestone_id",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_milestone_id",
                table: "Issues",
                column: "milestone_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Milestones_milestone_id",
                table: "Issues",
                column: "milestone_id",
                principalTable: "Milestones",
                principalColumn: "MilestoneId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Milestones_milestone_id",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_milestone_id",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "milestone_id",
                table: "Issues");
        }
    }
}
