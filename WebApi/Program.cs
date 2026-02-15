using Infrastructure.AutoMapper;
using Infrastructure.Data;
using Infrastructure.DI;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// CORS — разрешаем фронтенду доступ
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

// Получаем строку подключения: локально из appsettings.Development.json, на Render из переменной окружения
var connection = builder.Configuration.GetConnectionString("DefaultConnection")
                 ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                 ?? throw new InvalidOperationException("No database connection string configured.");

// Сервисы
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(InfrastructureProfile));
builder.Services.AddInfrastructure();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connection));
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Автоматически применяем миграции при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

// Swagger только в Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseRouting();
app.UseStaticFiles();
app.UseCors("AllowFrontend");
app.UseAuthorization();

// Контроллеры
app.MapControllers();

// Render использует PORT, локально 5000
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

app.Run();