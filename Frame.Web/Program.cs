using Frame.DataAccess;
using Frame.BusinessLogic.Interfaces;
using Frame.BusinessLogic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Frame.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Frame.DataAccess")));

// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IFileService>(sp => new FileService());
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,

            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    });

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Введите JWT токен в формате: Bearer {ваш_токен}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Accessible at /swagger
}

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate();

        // ─── USERS ───────────────────────────────────────────────────
        if (!context.Users.Any(u => u.Email == "14t.space@gmail.com"))
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin123")));
            context.Users.Add(new Frame.Domain.Entities.User { Username = "Admin", Email = "14t.space@gmail.com", Role = "Admin", PasswordHash = hash });
        }
        if (!context.Users.Any(u => u.Email == "m12.claude.green@gmail.com"))
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("manager123")));
            context.Users.Add(new Frame.Domain.Entities.User { Username = "Manager", Email = "m12.claude.green@gmail.com", Role = "Manager", PasswordHash = hash });
        }

        // ─── CATEGORIES ──────────────────────────────────────────────
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Frame.Domain.Entities.Category { Name = "Компьютеры" },
                new Frame.Domain.Entities.Category { Name = "Ноутбуки" },
                new Frame.Domain.Entities.Category { Name = "Процессоры" },
                new Frame.Domain.Entities.Category { Name = "Видеокарты" },
                new Frame.Domain.Entities.Category { Name = "Материнские платы" },
                new Frame.Domain.Entities.Category { Name = "Оперативная память" },
                new Frame.Domain.Entities.Category { Name = "Дисковые накопители" },
                new Frame.Domain.Entities.Category { Name = "Корпуса" },
                new Frame.Domain.Entities.Category { Name = "Системы охлаждения" },
                new Frame.Domain.Entities.Category { Name = "Блоки питания" }
            );
        }

        // ─── BRANDS ──────────────────────────────────────────────────
        if (!context.Brands.Any())
        {
            context.Brands.AddRange(
                // Производители ПК и ноутбуков
                new Frame.Domain.Entities.Brand { Name = "Apple" },
                new Frame.Domain.Entities.Brand { Name = "ASUS" },
                new Frame.Domain.Entities.Brand { Name = "Lenovo" },
                new Frame.Domain.Entities.Brand { Name = "HP" },
                new Frame.Domain.Entities.Brand { Name = "Dell" },
                new Frame.Domain.Entities.Brand { Name = "Acer" },
                new Frame.Domain.Entities.Brand { Name = "MSI" },
                new Frame.Domain.Entities.Brand { Name = "Gigabyte" },
                new Frame.Domain.Entities.Brand { Name = "Samsung" },
                // Производители процессоров
                new Frame.Domain.Entities.Brand { Name = "Intel" },
                new Frame.Domain.Entities.Brand { Name = "AMD" },
                // Производители видеокарт
                new Frame.Domain.Entities.Brand { Name = "NVIDIA" },
                new Frame.Domain.Entities.Brand { Name = "Sapphire" },
                new Frame.Domain.Entities.Brand { Name = "PowerColor" },
                new Frame.Domain.Entities.Brand { Name = "XFX" },
                new Frame.Domain.Entities.Brand { Name = "Zotac" },
                new Frame.Domain.Entities.Brand { Name = "Palit" },
                new Frame.Domain.Entities.Brand { Name = "Galax" },
                new Frame.Domain.Entities.Brand { Name = "EVGA" },
                // Производители материнских плат
                new Frame.Domain.Entities.Brand { Name = "ASRock" },
                new Frame.Domain.Entities.Brand { Name = "Biostar" },
                // Производители памяти
                new Frame.Domain.Entities.Brand { Name = "Corsair" },
                new Frame.Domain.Entities.Brand { Name = "Kingston" },
                new Frame.Domain.Entities.Brand { Name = "G.Skill" },
                new Frame.Domain.Entities.Brand { Name = "Crucial" },
                new Frame.Domain.Entities.Brand { Name = "TeamGroup" },
                new Frame.Domain.Entities.Brand { Name = "HyperX" },
                // Производители накопителей
                new Frame.Domain.Entities.Brand { Name = "WD" },
                new Frame.Domain.Entities.Brand { Name = "Seagate" },
                new Frame.Domain.Entities.Brand { Name = "Toshiba" },
                new Frame.Domain.Entities.Brand { Name = "Sandisk" },
                new Frame.Domain.Entities.Brand { Name = "Patriot" },
                // Производители охлаждения
                new Frame.Domain.Entities.Brand { Name = "Noctua" },
                new Frame.Domain.Entities.Brand { Name = "be quiet!" },
                new Frame.Domain.Entities.Brand { Name = "DeepCool" },
                new Frame.Domain.Entities.Brand { Name = "Cooler Master" },
                new Frame.Domain.Entities.Brand { Name = "Thermalright" },
                new Frame.Domain.Entities.Brand { Name = "Arctic" },
                // Производители корпусов
                new Frame.Domain.Entities.Brand { Name = "Fractal Design" },
                new Frame.Domain.Entities.Brand { Name = "Lian Li" },
                new Frame.Domain.Entities.Brand { Name = "NZXT" },
                // Производители блоков питания
                new Frame.Domain.Entities.Brand { Name = "SeaSonic" },
                new Frame.Domain.Entities.Brand { Name = "Thermaltake" },
                new Frame.Domain.Entities.Brand { Name = "FSP" }
            );
        }

        // ─── ATTRIBUTES (filter specs) ────────────────────────────────
        if (!context.Attributes.Any())
        {
            context.Attributes.AddRange(
                // ── Общие (Компьютеры и Ноутбуки) ──
                new Frame.Domain.Entities.Attribute { Name = "Подкатегория" },
                new Frame.Domain.Entities.Attribute { Name = "Процессор" },
                new Frame.Domain.Entities.Attribute { Name = "Видеокарта" },
                new Frame.Domain.Entities.Attribute { Name = "Объём VRAM" },
                new Frame.Domain.Entities.Attribute { Name = "Объём RAM" },
                new Frame.Domain.Entities.Attribute { Name = "Тип RAM" },
                new Frame.Domain.Entities.Attribute { Name = "Объём SSD" },
                new Frame.Domain.Entities.Attribute { Name = "Объём HDD" },
                new Frame.Domain.Entities.Attribute { Name = "Операционная система" },

                // ── Процессоры ──
                new Frame.Domain.Entities.Attribute { Name = "Сокет" },
                new Frame.Domain.Entities.Attribute { Name = "Ядра" },
                new Frame.Domain.Entities.Attribute { Name = "Потоки" },
                new Frame.Domain.Entities.Attribute { Name = "Базовая частота (ГГц)" },
                new Frame.Domain.Entities.Attribute { Name = "Turbo частота (ГГц)" },
                new Frame.Domain.Entities.Attribute { Name = "Кэш L3 (МБ)" },
                new Frame.Domain.Entities.Attribute { Name = "TDP (Вт)" },
                new Frame.Domain.Entities.Attribute { Name = "Встроенная графика" },
                new Frame.Domain.Entities.Attribute { Name = "Поддержка ECC" },

                // ── Видеокарты ──
                new Frame.Domain.Entities.Attribute { Name = "Чип" },
                new Frame.Domain.Entities.Attribute { Name = "Тип памяти" },
                new Frame.Domain.Entities.Attribute { Name = "Шина памяти (бит)" },
                new Frame.Domain.Entities.Attribute { Name = "Частота GPU (МГц)" },
                new Frame.Domain.Entities.Attribute { Name = "Boost частота (МГц)" },
                new Frame.Domain.Entities.Attribute { Name = "Количество разъёмов питания" },
                new Frame.Domain.Entities.Attribute { Name = "Рекомендуемый БП (Вт)" },

                // ── Материнские платы ──
                new Frame.Domain.Entities.Attribute { Name = "Форм-фактор" },
                new Frame.Domain.Entities.Attribute { Name = "Чипсет" },
                new Frame.Domain.Entities.Attribute { Name = "Слоты RAM" },
                new Frame.Domain.Entities.Attribute { Name = "Макс. RAM (ГБ)" },
                new Frame.Domain.Entities.Attribute { Name = "Слоты PCIe x16" },
                new Frame.Domain.Entities.Attribute { Name = "Порты M.2" },
                new Frame.Domain.Entities.Attribute { Name = "Порты SATA" },
                new Frame.Domain.Entities.Attribute { Name = "Wi-Fi" },
                new Frame.Domain.Entities.Attribute { Name = "Bluetooth" },

                // ── Оперативная память ──
                new Frame.Domain.Entities.Attribute { Name = "Объём модуля (ГБ)" },
                new Frame.Domain.Entities.Attribute { Name = "Количество модулей" },
                new Frame.Domain.Entities.Attribute { Name = "Частота (МГц)" },
                new Frame.Domain.Entities.Attribute { Name = "Тайминги" },
                new Frame.Domain.Entities.Attribute { Name = "Напряжение (В)" },
                new Frame.Domain.Entities.Attribute { Name = "RGB подсветка" },

                // ── Дисковые накопители ──
                new Frame.Domain.Entities.Attribute { Name = "Тип накопителя" },
                new Frame.Domain.Entities.Attribute { Name = "Объём (ГБ)" },
                new Frame.Domain.Entities.Attribute { Name = "Интерфейс" },
                new Frame.Domain.Entities.Attribute { Name = "Скорость чтения (МБ/с)" },
                new Frame.Domain.Entities.Attribute { Name = "Скорость записи (МБ/с)" },
                new Frame.Domain.Entities.Attribute { Name = "NAND тип" },
                new Frame.Domain.Entities.Attribute { Name = "Скорость (об/мин)" },

                // ── Корпуса ──
                new Frame.Domain.Entities.Attribute { Name = "Тип корпуса" },
                new Frame.Domain.Entities.Attribute { Name = "Поддерживаемые форм-факторы" },
                new Frame.Domain.Entities.Attribute { Name = "Макс. длина GPU (мм)" },
                new Frame.Domain.Entities.Attribute { Name = "Макс. высота CPU кулера (мм)" },
                new Frame.Domain.Entities.Attribute { Name = "Отсеки 2.5\" / 3.5\"" },
                new Frame.Domain.Entities.Attribute { Name = "Место под 360мм радиатор" },
                new Frame.Domain.Entities.Attribute { Name = "Цвет" },
                new Frame.Domain.Entities.Attribute { Name = "Боковая панель" },

                // ── Системы охлаждения ──
                new Frame.Domain.Entities.Attribute { Name = "Тип охлаждения" },
                new Frame.Domain.Entities.Attribute { Name = "Размер радиатора (мм)" },
                new Frame.Domain.Entities.Attribute { Name = "Макс. TDP (Вт)" },
                new Frame.Domain.Entities.Attribute { Name = "Совместимые сокеты" },
                new Frame.Domain.Entities.Attribute { Name = "Уровень шума (дБ)" },
                new Frame.Domain.Entities.Attribute { Name = "Скорость вентилятора (об/мин)" },

                // ── Блоки питания ──
                new Frame.Domain.Entities.Attribute { Name = "Мощность (Вт)" },
                new Frame.Domain.Entities.Attribute { Name = "Сертификат 80 PLUS" },
                new Frame.Domain.Entities.Attribute { Name = "Модульность" },
                new Frame.Domain.Entities.Attribute { Name = "Размер вентилятора БП (мм)" },
                new Frame.Domain.Entities.Attribute { Name = "Защита" },

                // ── Ноутбуки ──
                new Frame.Domain.Entities.Attribute { Name = "Диагональ экрана (дюйм)" },
                new Frame.Domain.Entities.Attribute { Name = "Разрешение экрана" },
                new Frame.Domain.Entities.Attribute { Name = "Частота обновления (Гц)" },
                new Frame.Domain.Entities.Attribute { Name = "Тип матрицы" },
                new Frame.Domain.Entities.Attribute { Name = "Время работы от батареи (ч)" },
                new Frame.Domain.Entities.Attribute { Name = "Вес (кг)" },
                new Frame.Domain.Entities.Attribute { Name = "USB-C / Thunderbolt" }
            );
        }

        context.SaveChanges();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}


app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
