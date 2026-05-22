using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frame.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLaptopsFiltersBilingual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM Attributes WHERE CategoryId = 2;

                INSERT INTO Attributes (Name, CategoryId, Options, [Order]) VALUES
                (N'Подкатегории / Subcategories', 2, N'[""Игровые / Gaming"",""Для учёбы / For Study"",""MacBook / MacBook""]', 1),
                (N'Производители / Manufacturers', 2, N'[""Acer / Acer"",""Apple / Apple"",""ASUS / ASUS"",""Lenovo / Lenovo""]', 2),
                (N'Диагональ экрана / Screen Diagonal', 2, N'[""18\"""" и более / 18\"""" and more"",""17\"""" / 17\"""""""",""16\"""" / 16\"""""""",""15\"""" / 15\"""""""",""14\"""" / 14\"""""""",""13\"""" / 13\""""""""]', 3),
                (N'Разрешение экрана / Screen Resolution', 2, N'[""4K (3840x2160) / 4K (3840x2160)"",""QHD+ (2560x1600) / QHD+ (2560x1600)"",""QHD (2560x1440) / QHD (2560x1440)"",""FHD+ (1920x1200) / FHD+ (1920x1200)"",""FHD (1920x1080) / FHD (1920x1080)""]', 4),
                (N'Тип матрицы / Panel Type', 2, N'[""OLED / OLED"",""IPS / IPS"",""Mini LED / Mini LED""]', 5),
                (N'Частота обновления экрана / Refresh Rate', 2, N'[""240 Гц / 240 Hz"",""165 Гц / 165 Hz"",""144 Гц / 144 Hz"",""120 Гц / 120 Hz"",""60 Гц / 60 Hz""]', 6),
                (N'Процессор / Processor', 2, N'[""Intel (все модели) / Intel (all models)"",""AMD (все модели) / AMD (all models)"",""Apple (все модели) / Apple (all models)"",""Apple M5 / Apple M5"",""Apple M5 Pro / Apple M5 Pro"",""Apple M5 Max / Apple M5 Max"",""Apple A18 Pro / Apple A18 Pro"",""Intel Core Ultra 9 Series 3 / Intel Core Ultra 9 Series 3"",""Intel Core Ultra 7 Series 3 / Intel Core Ultra 7 Series 3"",""AMD Ryzen 9 9xxx / AMD Ryzen 9 9xxx"",""AMD Ryzen 7 9xxx / AMD Ryzen 7 9xxx"",""AMD Ryzen AI Max / AMD Ryzen AI Max"",""AMD Ryzen AI 9 4xx / AMD Ryzen AI 9 4xx"",""AMD Ryzen AI 7 4xx / AMD Ryzen AI 7 4xx"",""AMD Ryzen AI 5 4xx / AMD Ryzen AI 5 4xx""]', 7),
                (N'Видеокарта / Graphics Card', 2, N'[""NVIDIA (все модели) / NVIDIA (all models)"",""NVIDIA GeForce RTX 5090 / NVIDIA GeForce RTX 5090"",""NVIDIA GeForce RTX 5080 / NVIDIA GeForce RTX 5080"",""NVIDIA GeForce RTX 5070 Ti / NVIDIA GeForce RTX 5070 Ti"",""NVIDIA GeForce RTX 5070 / NVIDIA GeForce RTX 5070"",""NVIDIA GeForce RTX 5060 / NVIDIA GeForce RTX 5060"",""NVIDIA GeForce RTX 5050 / NVIDIA GeForce RTX 5050"",""Интегрированная / Integrated""]', 8),
                (N'Объём памяти видеоадаптера / VRAM Capacity', 2, N'[""24 ГБ и более / 24 GB and more"",""16 ГБ / 16 GB"",""12 ГБ / 12 GB"",""10 ГБ / 10 GB"",""8 ГБ / 8 GB"",""6 ГБ / 6 GB"",""4 ГБ / 4 GB""]', 9),
                (N'Объём оперативной памяти / RAM Capacity', 2, N'[""128 ГБ и более / 128 GB and more"",""96 ГБ / 96 GB"",""64 ГБ / 64 GB"",""48 ГБ / 48 GB"",""32 ГБ / 32 GB"",""24 ГБ / 24 GB"",""16 ГБ / 16 GB"",""8 ГБ / 8 GB""]', 10),
                (N'Тип оперативной памяти / RAM Type', 2, N'[""DDR5 / DDR5"",""DDR4 / DDR4""]', 11),
                (N'Объём SSD / SSD Capacity', 2, N'[""2000 ГБ и более / 2000 GB and more"",""960-1024 ГБ / 960-1024 GB"",""480-512 ГБ / 480-512 GB"",""240-256 ГБ / 240-256 GB""]', 12),
                (N'Предустановленная ОС / Pre-installed OS', 2, N'[""Windows 11 / Windows 11"",""macOS / macOS"",""Linux / Linux"",""Chrome OS / Chrome OS"",""Без ОС / No OS""]', 13),
                (N'Масса / Weight', 2, N'[""до 1.5 кг / up to 1.5 kg"",""1.5 - 2 кг / 1.5 - 2 kg"",""2 - 2.5 кг / 2 - 2.5 kg"",""более 2.5 кг / more than 2.5 kg""]', 14);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM Attributes WHERE CategoryId = 2;");
        }
    }
}
