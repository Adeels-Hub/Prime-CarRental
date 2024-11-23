using PrimeGroup.CarRentalService.Core.Interfaces;
using PrimeGroup.CarRentalService.Core.Services;
using PrimeGroup.CarRentalService.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Adds support for controllers
builder.Services.AddEndpointsApiExplorer(); // Enables API endpoint metadata
builder.Services.AddSwaggerGen(); // Adds Swagger for API documentation

// Dependency Injection (DI)
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add the custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
