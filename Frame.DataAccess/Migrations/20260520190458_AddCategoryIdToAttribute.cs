using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frame.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Attributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Attributes_CategoryId",
                table: "Attributes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes");

            migrationBuilder.DropIndex(
                name: "IX_Attributes_CategoryId",
                table: "Attributes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Attributes");
        }
    }
}
