using Frame.BusinessLogic;
using Frame.BusinessLogic.Interfaces;
using Frame.BusinessLogic.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register AppDbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
 felciuc
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Frame.BusinessLogic")));

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
 main

// Register Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();

// Register AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Accessible at /swagger
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
