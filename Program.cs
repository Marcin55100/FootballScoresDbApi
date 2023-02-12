using FootballScoresDbApi;
using FootballScoresDbApi.Logger;
using FootballScoresDbApi.Models;
using FootballScoresDbApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Logger
LoggerCreator.CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDbContext<AuthenticationContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Authentication
services.AddAuthentication();
services.ConfigureIdentity();
services.ConfigureJWT(builder.Configuration);
services.AddScoped<IAuthManager, AuthManager>();
services.AddSwaggerDoc();


var app = builder.Build();

app.UseCors(options =>
options.WithOrigins("http://localhost:4200")
.AllowAnyMethod()
.AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
