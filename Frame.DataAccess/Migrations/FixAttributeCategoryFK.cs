using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frame.DataAccess.Migrations
{
    public partial class FixAttributeCategoryFK_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert any placeholder 0 values to NULL to satisfy FK
            migrationBuilder.Sql("UPDATE [Attributes] SET [CategoryId] = NULL WHERE [CategoryId] = 0");

            // Alter column to be nullable
            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Attributes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            // Add foreign key with SetNull on delete
            migrationBuilder.AddForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove foreign key
            migrationBuilder.DropForeignKey(
                name: "FK_Attributes_Categories_CategoryId",
                table: "Attributes");

            // Revert column to non‑nullable with default 0
            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Attributes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Optionally reset any NULLs back to 0 (not strictly needed for down migration)
            migrationBuilder.Sql("UPDATE [Attributes] SET [CategoryId] = 0 WHERE [CategoryId] IS NULL");
        }
    }
}
