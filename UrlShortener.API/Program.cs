using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Infrastructure;
using UrlShortener.API.Models;
using UrlShortener.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<UrlShortenerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "Url Shortener"));
}

app.MapPost("api/shorten", (UrlShortenerService urlShortenerService, AppDbContext dbContext, ShortenedUrlRequest request) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("A URL especificada é inválida.");
    }


});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
