using Microsoft.EntityFrameworkCore;
using WorkoutApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Lägg till CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Din React-apps adress
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// Tillåt att enum konverteras till strängar i JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))); // Anslutningssträng, kan styras dynamiskt

var app = builder.Build();

// Använd CORS
app.UseCors("AllowReactApp");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
