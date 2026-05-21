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
        var filePath = Path.Combine(AppContext.BaseDirectory, "extracted_options.json");
        if (!File.Exists(filePath))
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "extracted_options.json");
        }
        if (!File.Exists(filePath))
        {
            filePath = @"C:\project\project-back\Frame.Web\extracted_options.json";
        }

        if (File.Exists(filePath))
        {
            var jsonStr = File.ReadAllText(filePath);
            using var doc = System.Text.Json.JsonDocument.Parse(jsonStr);
            
            // First save categories to make sure they have IDs
            context.SaveChanges();
            var dbCategories = context.Categories.ToList();
            
            foreach (var categoryProp in doc.RootElement.EnumerateObject())
            {
                var categoryName = categoryProp.Name;
                var dbCat = dbCategories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                if (dbCat == null) continue;
                
                foreach (var filterObj in categoryProp.Value.EnumerateArray())
                {
                    var title = filterObj.GetProperty("title").GetString();
                    // Skip only internal price filter key; all other titles (including Подкатегории, Производители) are seeded
                    if (string.IsNullOrEmpty(title) || title == "filters.price") continue;
                    
                    var optionsElement = filterObj.GetProperty("options");
                    var optionsList = new List<string>();
                    foreach (var opt in optionsElement.EnumerateArray())
                    {
                        var val = opt.GetString();
                        if (!string.IsNullOrEmpty(val)) optionsList.Add(val);
                    }
                    
                    // Skip filters with no options
                    if (optionsList.Count == 0) continue;
                    
                    var serializedOptions = System.Text.Json.JsonSerializer.Serialize(optionsList);
                    
                    // Check if attribute already exists for this category
                    var existingAttr = context.Attributes.FirstOrDefault(a => a.CategoryId == dbCat.Id && a.Name == title);
                    if (existingAttr == null)
                    {
                        // Check if there is a global attribute with the same name that we can migrate
                        var globalAttr = context.Attributes.FirstOrDefault(a => a.CategoryId == null && a.Name == title);
                        if (globalAttr != null)
                        {
                            globalAttr.CategoryId = dbCat.Id;
                            globalAttr.Options = serializedOptions;
                        }
                        else
                        {
                            context.Attributes.Add(new Frame.Domain.Entities.Attribute
                            {
                                Name = title,
                                CategoryId = dbCat.Id,
                                Options = serializedOptions
                            });
                        }
                    }
                    else
                    {
                        // Always update options from the source of truth (extracted_options.json)
                        existingAttr.Options = serializedOptions;
                    }
                }
            }
            
            // Clean up any remaining attributes with null CategoryId that are not used by any products
            var unusedGlobalAttrs = context.Attributes
                .Where(a => a.CategoryId == null)
                .ToList();
            foreach (var attr in unusedGlobalAttrs)
            {
                bool isUsed = context.ProductAttributeValues.Any(pav => pav.AttributeId == attr.Id);
                if (!isUsed)
                {
                    context.Attributes.Remove(attr);
                }
            }
            
            context.SaveChanges();
        }

        // Seeding 5 Computers
        var pcCategory = context.Categories.FirstOrDefault(c => c.Name == "Компьютеры");
        if (pcCategory != null)
        {
            var brandAsus = context.Brands.FirstOrDefault(b => b.Name == "ASUS");
            var brandMsi = context.Brands.FirstOrDefault(b => b.Name == "MSI");
            var brandHp = context.Brands.FirstOrDefault(b => b.Name == "HP");
            var brandLenovo = context.Brands.FirstOrDefault(b => b.Name == "Lenovo");
            var brandApple = context.Brands.FirstOrDefault(b => b.Name == "Apple");

            var pcAttributes = context.Attributes.Where(a => a.CategoryId == pcCategory.Id).ToList();

            var attrSubcat = pcAttributes.FirstOrDefault(a => a.Name == "Подкатегории");
            var attrCpu = pcAttributes.FirstOrDefault(a => a.Name == "Процессор");
            var attrGpu = pcAttributes.FirstOrDefault(a => a.Name == "Видеокарта");
            var attrGpuRam = pcAttributes.FirstOrDefault(a => a.Name == "Объём памяти видеоадаптера");
            var attrRam = pcAttributes.FirstOrDefault(a => a.Name == "Объём оперативной памяти");
            var attrRamType = pcAttributes.FirstOrDefault(a => a.Name == "Тип оперативной памяти");
            var attrSsd = pcAttributes.FirstOrDefault(a => a.Name == "Объём SSD");
            var attrHdd = pcAttributes.FirstOrDefault(a => a.Name == "Обьём HDD");
            var attrOs = pcAttributes.FirstOrDefault(a => a.Name == "Предустановленная ОС");

            var attrValues = new List<Frame.Domain.Entities.ProductAttributeValue>();

            void AddValue(Frame.Domain.Entities.Product p, Frame.Domain.Entities.Attribute? attr, string val)
            {
                if (attr != null)
                {
                    attrValues.Add(new Frame.Domain.Entities.ProductAttributeValue
                    {
                        ProductId = p.Id,
                        AttributeId = attr.Id,
                        Value = val
                    });
                }
            }

            var seedProducts = new List<(string Name, decimal Price, string Description, string Subcat, int? BrandId, string Img, Action<Frame.Domain.Entities.Product> AddSpecs)>
            {
                ("ASUS ROG Strix GA35 Ultimate", 150000.00m, "Мощный игровой компьютер ASUS ROG Strix GA35 с процессором Intel Ultra 9 и видеокартой RTX 5090.", "Игровые", brandAsus?.Id, "https://images.unsplash.com/photo-1587831990711-23ca6441447b?auto=format&fit=crop&w=600&q=80", p => {
                    AddValue(p, attrSubcat, "Игровые");
                    AddValue(p, attrCpu, "Intel Core Ultra 9 2xx");
                    AddValue(p, attrGpu, "NVIDIA GeForce RTX 5090");
                    AddValue(p, attrGpuRam, "24 ГБ и более");
                    AddValue(p, attrRam, "64 ГБ");
                    AddValue(p, attrRamType, "DDR5");
                    AddValue(p, attrSsd, "2000-4000 ГБ");
                    AddValue(p, attrHdd, "2000 ГБ");
                    AddValue(p, attrOs, "Windows 11");
                }),
                ("MSI Infinite X2 Gamer", 75000.00m, "Сбалансированный игровой ПК MSI Infinite X2 на базе Ryzen 7 и видеокарты RTX 5070.", "Игровые", brandMsi?.Id, "https://images.unsplash.com/photo-1593642632823-8f785ba67e45?auto=format&fit=crop&w=600&q=80", p => {
                    AddValue(p, attrSubcat, "Игровые");
                    AddValue(p, attrCpu, "AMD Ryzen 7 9xxx");
                    AddValue(p, attrGpu, "NVIDIA GeForce RTX 5070");
                    AddValue(p, attrGpuRam, "12 ГБ");
                    AddValue(p, attrRam, "32 ГБ");
                    AddValue(p, attrRamType, "DDR5");
                    AddValue(p, attrSsd, "960-1024 ГБ");
                    AddValue(p, attrHdd, "1000 ГБ");
                    AddValue(p, attrOs, "Windows 11");
                }),
                ("HP ProTower 400 G9 Workstation", 45000.00m, "Надежная рабочая станция HP ProTower для офисных задач, программирования и работы с данными.", "Рабочие станции", brandHp?.Id, "https://images.unsplash.com/photo-1618424181497-157f25b6ddd5?auto=format&fit=crop&w=600&q=80", p => {
                    AddValue(p, attrSubcat, "Рабочие станции");
                    AddValue(p, attrCpu, "Intel Core Ultra 5 2xx");
                    AddValue(p, attrGpu, "NVIDIA GeForce RTX 5050");
                    AddValue(p, attrGpuRam, "8 ГБ");
                    AddValue(p, attrRam, "16 ГБ");
                    AddValue(p, attrRamType, "DDR4");
                    AddValue(p, attrSsd, "480-512 ГБ");
                    AddValue(p, attrHdd, "500 ГБ");
                    AddValue(p, attrOs, "Windows 11");
                }),
                ("Lenovo IdeaCentre Mini Gen 8", 35000.00m, "Сверхкомпактный мини-ПК Lenovo IdeaCentre Mini Gen 8 для экономии рабочего пространства.", "Мини-ПК", brandLenovo?.Id, "https://images.unsplash.com/photo-1600541519401-44712e41ef34?auto=format&fit=crop&w=600&q=80", p => {
                    AddValue(p, attrSubcat, "Мини-ПК");
                    AddValue(p, attrCpu, "AMD Ryzen 5 9xxx");
                    AddValue(p, attrGpu, "AMD Radeon RX 9060 XT");
                    AddValue(p, attrGpuRam, "8 ГБ");
                    AddValue(p, attrRam, "16 ГБ");
                    AddValue(p, attrRamType, "DDR5");
                    AddValue(p, attrSsd, "480-512 ГБ");
                    AddValue(p, attrHdd, "500 ГБ");
                    AddValue(p, attrOs, "Linux");
                }),
                ("Apple Mac Studio M4 Max", 220000.00m, "Компактный суперкомпьютер Apple Mac Studio M4 Max для профессиональной работы со звуком, видео и графикой.", "Моноблоки", brandApple?.Id, "https://images.unsplash.com/photo-1636408807362-a6195d3dd4de?auto=format&fit=crop&w=600&q=80", p => {
                    AddValue(p, attrSubcat, "Моноблоки");
                    AddValue(p, attrCpu, "Apple M4 Max");
                    AddValue(p, attrGpu, "NVIDIA RTX PRO");
                    AddValue(p, attrGpuRam, "24 ГБ и более");
                    AddValue(p, attrRam, "48 ГБ");
                    AddValue(p, attrRamType, "DDR5");
                    AddValue(p, attrSsd, "2000-4000 ГБ");
                    AddValue(p, attrOs, "Mac OS");
                })
            };

            foreach (var seed in seedProducts)
            {
                if (!context.Products.Any(p => p.Name == seed.Name && p.CategoryId == pcCategory.Id))
                {
                    var product = new Frame.Domain.Entities.Product
                    {
                        Name = seed.Name,
                        Price = seed.Price,
                        Description = seed.Description,
                        CategoryId = pcCategory.Id,
                        SubcategoryName = seed.Subcat,
                        BrandId = seed.BrandId,
                        Images = new List<Frame.Domain.Entities.ProductImage> { new Frame.Domain.Entities.ProductImage { Url = seed.Img } }
                    };
                    context.Products.Add(product);
                    context.SaveChanges(); // Save to generate product.Id

                    seed.AddSpecs(product);
                }
            }

            if (attrValues.Count > 0)
            {
                context.ProductAttributeValues.AddRange(attrValues);
                context.SaveChanges();
            }
        }
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
