using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResearchManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class uupdateMilestone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Milestones_ResearchTopics_ResearchTopicTopicId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_ResearchTopicTopicId",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "ResearchTopicTopicId",
                table: "Milestones");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_TopicId",
                table: "Milestones",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Milestones_ResearchTopics_TopicId",
                table: "Milestones",
                column: "TopicId",
                principalTable: "ResearchTopics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Milestones_ResearchTopics_TopicId",
                table: "Milestones");

            migrationBuilder.DropIndex(
                name: "IX_Milestones_TopicId",
                table: "Milestones");

            migrationBuilder.AddColumn<int>(
                name: "ResearchTopicTopicId",
                table: "Milestones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_ResearchTopicTopicId",
                table: "Milestones",
                column: "ResearchTopicTopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Milestones_ResearchTopics_ResearchTopicTopicId",
                table: "Milestones",
                column: "ResearchTopicTopicId",
                principalTable: "ResearchTopics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
