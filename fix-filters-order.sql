-- Get the Computers category ID
DECLARE @computerId INT;
SELECT @computerId = Id FROM Categories WHERE Name = 'Компьютеры';

-- Delete the duplicate HDD attribute with typo (Обьём instead of Объём)
DELETE FROM Attributes
WHERE CategoryId = @computerId
  AND Name LIKE 'Обьём HDD%';

-- Update the Order field for all filters in Computers category to match the desired order
-- Desired order:
-- 1. Подкатегории / Subcategories
-- 2. Производители / Manufacturers
-- 3. Процессор / Processor
-- 4. Видеокарта / Graphics Card
-- 5. Объём памяти видеоадаптера / VRAM Capacity
-- 6. Объём оперативной памяти / RAM Capacity
-- 7. Тип оперативной памяти / RAM Type
-- 8. Объём SSD / SSD Capacity
-- 9. Объём HDD / HDD Capacity
-- 10. Предустановленная ОС / Pre-installed OS

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

-- Verify the changes
SELECT [Order], Name FROM Attributes
WHERE CategoryId = @computerId
ORDER BY [Order];
