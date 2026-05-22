-- Delete all old attributes for Laptops category (ID = 2)
DELETE FROM Attributes WHERE CategoryId = 2;

-- Insert new attributes with correct order and parameters
INSERT INTO Attributes (Name, CategoryId, Options, [Order]) VALUES
(N'Подкатегории', 2, N'["Игровые","Для учёбы","MacBook"]', 1),
(N'Производители', 2, N'["Acer","Apple","ASUS","Lenovo"]', 2),
(N'Диагональ экрана', 2, N'["18\" и более","17\"","16\"","15\"","14\"","13\""]', 3),
(N'Разрешение экрана', 2, N'["4K (3840x2160)","QHD+ (2560x1600)","QHD (2560x1440)","FHD+ (1920x1200)","FHD (1920x1080)"]', 4),
(N'Тип матрицы', 2, N'["OLED","IPS","Mini LED"]', 5),
(N'Частота обновления экрана', 2, N'["240 Гц","165 Гц","144 Гц","120 Гц","60 Гц"]', 6),
(N'Процессор', 2, N'["Intel (все модели)","AMD (все модели)","Apple (все модели)","Apple M5","Apple M5 Pro","Apple M5 Max","Apple A18 Pro","Intel Core Ultra 9 Series 3","Intel Core Ultra 7 Series 3","AMD Ryzen 9 9xxx","AMD Ryzen 7 9xxx","AMD Ryzen AI Max","AMD Ryzen AI 9 4xx","AMD Ryzen AI 7 4xx","AMD Ryzen AI 5 4xx"]', 7),
(N'Видеокарта', 2, N'["NVIDIA (все модели)","NVIDIA GeForce RTX 5090","NVIDIA GeForce RTX 5080","NVIDIA GeForce RTX 5070 Ti","NVIDIA GeForce RTX 5070","NVIDIA GeForce RTX 5060","NVIDIA GeForce RTX 5050","Интегрированная"]', 8),
(N'Объём памяти видеоадаптера', 2, N'["24 ГБ и более","16 ГБ","12 ГБ","10 ГБ","8 ГБ","6 ГБ","4 ГБ"]', 9),
(N'Объём оперативной памяти', 2, N'["128 ГБ и более","96 ГБ","64 ГБ","48 ГБ","32 ГБ","24 ГБ","16 ГБ","8 ГБ"]', 10),
(N'Тип оперативной памяти', 2, N'["DDR5","DDR4"]', 11),
(N'Объём SSD', 2, N'["2000 ГБ и более","960-1024 ГБ","480-512 ГБ","240-256 ГБ"]', 12),
(N'Предустановленная ОС', 2, N'["Windows 11","macOS","Linux","Chrome OS","Без ОС"]', 13),
(N'Масса', 2, N'["до 1.5 кг","1.5 - 2 кг","2 - 2.5 кг","более 2.5 кг"]', 14);

-- Verify
SELECT [Order], Name FROM Attributes WHERE CategoryId = 2 ORDER BY [Order];
