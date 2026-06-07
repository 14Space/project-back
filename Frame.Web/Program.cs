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
builder.Services.AddScoped<IBannerService, BannerService>();

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
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        
        var adminEmail = "14t.space@gmail.com";
        var adminHash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Frame-admin")));
        var adminUser = context.Users.FirstOrDefault(u => u.Email == adminEmail);
        if (adminUser == null)
        {
            context.Users.Add(new Frame.Domain.Entities.User { Name = "Admin", Email = adminEmail, Role = "Admin", PasswordHash = adminHash });
        }
        else
        {
            adminUser.PasswordHash = adminHash;
        }

        var managerEmail = "m12.claude.green@gmail.com";
        var managerHash = Convert.ToBase64String(sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Frame-manager")));
        var managerUser = context.Users.FirstOrDefault(u => u.Email == managerEmail);
        if (managerUser == null)
        {
            context.Users.Add(new Frame.Domain.Entities.User { Name = "Manager", Email = managerEmail, Role = "Manager", PasswordHash = managerHash });
        }
        else
        {
            managerUser.PasswordHash = managerHash;
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
            
            int order = 0;
            foreach (var categoryProp in doc.RootElement.EnumerateObject())
            {
                var categoryName = categoryProp.Name;
                var dbCat = dbCategories.FirstOrDefault(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));
                if (dbCat == null)
                {
                    dbCat = new Frame.Domain.Entities.Category { Name = categoryName };
                    context.Categories.Add(dbCat);
                    context.SaveChanges();
                    dbCategories.Add(dbCat);
                }
                
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
                    
                    var titleLower = title.ToLower();
                    bool isBrandAttribute = titleLower.Contains("бренд") || 
                                            titleLower.Contains("производител") || 
                                            titleLower.Contains("brand") || 
                                            titleLower.Contains("manufactur");
                    if (isBrandAttribute)
                    {
                        foreach (var opt in optionsList)
                        {
                            var brandName = opt.Split(" / ")[0].Trim();
                            bool brandExists = context.Brands.Local.Any(b => b.Name.ToLower() == brandName.ToLower()) || 
                                               context.Brands.Any(b => b.Name.ToLower() == brandName.ToLower());
                            if (!brandExists)
                            {
                                context.Brands.Add(new Frame.Domain.Entities.Brand { Name = brandName });
                            }
                        }
                    }
                    
                    var orderValue = filterObj.TryGetProperty("order", out var orderProp) ? orderProp.GetInt32() : order++;
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
                            var globalOptionsList = new List<string>();
                            if (!string.IsNullOrEmpty(globalAttr.Options))
                            {
                                try 
                                {
                                    globalOptionsList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(globalAttr.Options) ?? new List<string>();
                                }
                                catch { }
                            }
                            var mergedGlobalOptions = globalOptionsList.Union(optionsList).Distinct().ToList();
                            globalAttr.Options = System.Text.Json.JsonSerializer.Serialize(mergedGlobalOptions);
                            globalAttr.Order = orderValue;
                        }
                        else
                        {
                            context.Attributes.Add(new Frame.Domain.Entities.Attribute
                            {
                                Name = title,
                                CategoryId = dbCat.Id,
                                Options = serializedOptions,
                                Order = orderValue
                            });
                        }
                    }
                    else
                    {
                        // Merge options: keep existing options (which might include user-added ones) and add new ones from extracted_options.json
                        var existingOptionsList = new List<string>();
                        if (!string.IsNullOrEmpty(existingAttr.Options))
                        {
                            try 
                            {
                                existingOptionsList = System.Text.Json.JsonSerializer.Deserialize<List<string>>(existingAttr.Options) ?? new List<string>();
                            }
                            catch { }
                        }
                        var mergedOptions = existingOptionsList.Union(optionsList).Distinct().ToList();
                        existingAttr.Options = System.Text.Json.JsonSerializer.Serialize(mergedOptions);
                        existingAttr.Order = orderValue;
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
