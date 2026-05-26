using Frame.Domain.Entities;
using Attribute = Frame.Domain.Entities.Attribute;

namespace Frame.DataAccess
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.Migrate(context.Database);

            if (!context.Categories.Any())
            {
                var computers = new Category { Name = "Компьютеры"};
                var laptops = new Category { Name = "Ноутбуки"};
                var parts = new Category { Name = "Комплектующие"};
                
                var monitors = new Category { Name = "Мониторы" };
                var peripherals = new Category { Name = "Периферия" };
                
                context.Categories.AddRange(computers, laptops, parts, monitors, peripherals);
                context.SaveChanges();
            }

            if (!context.Brands.Any())
            {
                var brands = new[]
                {
                    new Brand { Name = "Apple" },
                    new Brand { Name = "ASUS" },
                    new Brand { Name = "NVIDIA" },
                    new Brand { Name = "AMD" },
                    new Brand { Name = "Intel" },
                    new Brand { Name = "MSI" },
                    new Brand { Name = "Samsung" },
                    new Brand { Name = "Corsair" },
                    new Brand { Name = "be quiet!" },
                    new Brand { Name = "Fractal Design" },
                    new Brand { Name = "SeaSonic" }
                };
                context.Brands.AddRange(brands);
                context.SaveChanges();
            }

            if (!context.Attributes.Any())
            {
                var attributes = new[]
                {
                    new Attribute { Name = "Сокет" },
                    new Attribute { Name = "Чипсет" },
                    new Attribute { Name = "Тип памяти" },
                    new Attribute { Name = "Мощность БП" },
                    new Attribute { Name = "Сертификат 80 PLUS" },
                    new Attribute { Name = "Типоразмер корпуса" },
                    new Attribute { Name = "Скорость чтения SSD" },
                    new Attribute { Name = "Тайминги RAM" }
                };
                context.Attributes.AddRange(attributes);
                context.SaveChanges();
            }
        }
    }
}
