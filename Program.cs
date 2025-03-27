using Microsoft.EntityFrameworkCore;
using Minio;
using ProductManagementAPI.DataAccess;
using ProductManagementAPI.DataAccess.Repos;
using ProductManagementAPI.DataAccess.Repos.RepoInterface;
using ProductManagementAPI.Mapping;
using ProductManagementAPI.Services;
using ProductManagementAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IMinioClient>(_ =>
    new MinioClient()
        .WithEndpoint(builder.Configuration["Minio:Endpoint"])
        .WithCredentials(
            builder.Configuration["Minio:AccessKey"],
            builder.Configuration["Minio:SecretKey"])
        .WithSSL(false)
        .Build());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IFileStorageService, MinioStorageService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<ProductManagementDb>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().AddNewtonsoftJson();

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