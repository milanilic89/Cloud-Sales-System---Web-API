global using CloudSalesAPI.Models;
using CloudSalesAPI.Data;
using CloudSalesAPI.Middleware;
using CloudSalesAPI.Provider;
using CloudSalesAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<LoggerMiddleware>();
builder.Services.AddScoped<IAdministrationService, AdministrationService>();
builder.Services.AddScoped<IProvisioningService, ProvisioningService>();
builder.Services.AddScoped<ICloudComputingProvider, MockedProvider>();
builder.Services.AddScoped<IDataContext, DataContext>();
builder.Services.AddDbContext<DataContext>(optionsAction =>
{
    optionsAction.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSetting"));
} );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<LoggerMiddleware>();

app.MapControllers();

app.Run();
