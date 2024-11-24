using PrimeGroup.CarRentalService.Core.Interfaces;
using PrimeGroup.CarRentalService.Core.Services;
using PrimeGroup.CarRentalService.Api.Middleware;
using PrimeGroup.CarRentalService.Data;

var builder = WebApplication.CreateBuilder(args);

//ToDo: Configuration can be loaded as well if some settings need to be configured.
// Add services to the container
// ToDo: if number of service registerations increase then it is better to move them to separate startup class
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

// ToDo: if number of middleware registerations increase then it is better to move them to separate startup class
// Add the custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();
//ToDo: logging should also be added

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
