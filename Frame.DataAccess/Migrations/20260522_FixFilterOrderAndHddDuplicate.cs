using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frame.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixFilterOrderAndHddDuplicate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Get the Computers category ID
            migrationBuilder.Sql(@"
                DECLARE @computerId INT;
                SELECT @computerId = Id FROM Categories WHERE Name = 'Компьютеры';

                -- Delete the duplicate HDD attribute with typo (Обьём instead of Объём)
                DELETE FROM Attributes
                WHERE CategoryId = @computerId AND Name LIKE 'Обьём HDD%';

                -- Update Order for all filters in correct sequence
                UPDATE Attributes SET [Order] = 1 WHERE CategoryId = @computerId AND Name LIKE 'Подкатегории%';
                UPDATE Attributes SET [Order] = 2 WHERE CategoryId = @computerId AND Name LIKE 'Производители%';
                UPDATE Attributes SET [Order] = 3 WHERE CategoryId = @computerId AND Name LIKE 'Процессор%';
                UPDATE Attributes SET [Order] = 4 WHERE CategoryId = @computerId AND Name LIKE 'Видеокарта%';
                UPDATE Attributes SET [Order] = 5 WHERE CategoryId = @computerId AND Name LIKE 'Объём памяти видеоадаптера%';
                UPDATE Attributes SET [Order] = 6 WHERE CategoryId = @computerId AND Name LIKE 'Объём оперативной памяти%';
                UPDATE Attributes SET [Order] = 7 WHERE CategoryId = @computerId AND Name LIKE 'Тип оперативной памяти%';
                UPDATE Attributes SET [Order] = 8 WHERE CategoryId = @computerId AND Name LIKE 'Объём SSD%';
                UPDATE Attributes SET [Order] = 9 WHERE CategoryId = @computerId AND Name LIKE 'Объём HDD%';
                UPDATE Attributes SET [Order] = 10 WHERE CategoryId = @computerId AND Name LIKE 'Предустановленная ОС%';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reset Order values if needed
            migrationBuilder.Sql(@"
                DECLARE @computerId INT;
                SELECT @computerId = Id FROM Categories WHERE Name = 'Компьютеры';
                UPDATE Attributes SET [Order] = 0 WHERE CategoryId = @computerId;
            ");
        }
    }
}
