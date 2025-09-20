using CShop.Application.Interfaces;
using CShop.Application.Mapping;
using CShop.Infrastructure.Data;
using CShop.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
builder.Services.AddAutoMapper(typeof(RoleProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
