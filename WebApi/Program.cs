using Infrastructure.AutoMapper;
using Infrastructure.Data;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// CORS — разрешаем все источники для фронтенда
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(InfrastructureProfile));
builder.Services.AddInfrastructure();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connection));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseStaticFiles();
app.UseCors("AllowFrontend");

app.MapControllers();

// Render использует переменную PORT
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
app.Urls.Add($"http://*:{port}");

app.Run();