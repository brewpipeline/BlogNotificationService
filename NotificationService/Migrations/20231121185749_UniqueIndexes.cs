using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationService.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_subscribers_BlogUserId",
                schema: "notification",
                table: "subscribers",
                column: "BlogUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subscribers_TelegramId",
                schema: "notification",
                table: "subscribers",
                column: "TelegramId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_subscribers_BlogUserId",
                schema: "notification",
                table: "subscribers");

            migrationBuilder.DropIndex(
                name: "IX_subscribers_TelegramId",
                schema: "notification",
                table: "subscribers");
        }
    }
}
