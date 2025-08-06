using Notes.Application;
using Notes.Persistence;
using System.Reflection;
using Notes.Application.Interfaces;
using Notes.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Получаем IConfiguration
var configuration = builder.Configuration;

// Регистрируем сервисы
builder.Services.AddAutoMapper(
    Assembly.GetExecutingAssembly(),
    typeof(INotesDbContext).Assembly);

builder.Services.AddApplication();
builder.Services.AddPersistence(configuration);
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});


var app = builder.Build();

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<NotesDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        // TODO: логирование ошибки при инициализации базы данных
        Console.WriteLine($"Ошибка инициализации БД: {ex.Message}");
    }
}

// Конфигурация middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();













//using Notes.Persistence;

//namespace Notes.WebAPI;

//public class Program
//{
//    public static void Main(string[] args)
//    {
//        var host = CreateHostBuilder(args).Build();

//        using (var scope = host.Services.CreateScope())
//        {
//            var serviceProvider = scope.ServiceProvider;

//            try
//            {
//                var context = serviceProvider.GetRequiredService<NotesDbContext>();
//                DbInitializer.Initialize(context);
//            }
//            catch (Exception ex)
//            {
//               //в следующие разы вернемся к этому коду что делать с ошибками

//            }
//        }
//    }

//    public static IHostBuilder CreateHostBuilder(string[] args) =>
//        Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(webBuilder =>
//            {
//                webBuilder.UseStartup<Startup>();
//            });
//}
