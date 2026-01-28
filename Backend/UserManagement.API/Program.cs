using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.Interfaces;
using UserManagement.API.Middleware;
using UserManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Add services to the container

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

var frontendOrigin = builder.Configuration["Frontend:Origin"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy  
            .WithOrigins(frontendOrigin)
            .AllowAnyHeader()                      
            .AllowAnyMethod();                     
    });
});


builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();


app.UseCors("AllowFrontend");

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();


app.MapControllers();


app.Run();
