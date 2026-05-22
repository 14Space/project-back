using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frame.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixHddDuplicate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete the duplicate HDD attribute with incorrect spelling (Обьём instead of Объём)
            migrationBuilder.Sql(
                @"DELETE FROM ""Attributes""
                  WHERE ""Name"" LIKE 'Обьём HDD%'
                  AND ""Name"" NOT LIKE 'Объём HDD%';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op for down migration - we're just cleaning up bad data
        }
    }
}
