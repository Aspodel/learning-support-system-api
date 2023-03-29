using LearningSupportSystemAPI.Configs;
using LearningSupportSystemAPI.Core.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration; // allows both to access and to set up the config

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string azureAppConfigConnectionString = configuration.GetConnectionString("AppConfig")!;
configuration.AddAzureAppConfiguration(azureAppConfigConnectionString);

builder.Services.Configure<EduConfig>(configuration.GetSection("EduConfig"));
builder.Services.AddDbContextPool<ApplicationDbContext>(options => options.UseSqlServer(configuration["EduConfig:ConnectionString"]));


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
