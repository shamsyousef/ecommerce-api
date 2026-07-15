using Core.Interfaces;
using Infrastructure.Data;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.OpenApi.Validations;
using System.Linq.Expressions;
using Infrastructure.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null);
        });
});
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:4200");
    });
});
builder.Services.AddSingleton<IConnectionMultiplexer>(config => {
    var conString = builder.Configuration.GetConnectionString("Redis")
    ?? throw new Exception("Cannot get redis connection string ");
    var configuration = ConfigurationOptions.Parse(conString, true);
    return ConnectionMultiplexer.Connect(configuration);

   } );
builder.Services.AddSingleton<ICartService, CartService>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<StoreContext>();

    await context.Database.MigrateAsync();

    await StoreContextSeed.SeedAsync(context);
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("CorsPolicy");
app.MapControllers();

app.Run();
