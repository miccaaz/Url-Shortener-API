using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Entities;
using UrlShortener.API.Extensions;
using UrlShortener.API.Infrastructure;
using UrlShortener.API.Models;
using UrlShortener.API.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UrlShortenerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "Url Shortener"));

    app.ApllyMigrations();
}

app.MapPost("api/shorten", async (
    ShortenedUrlRequest request, 
    UrlShortenerService urlShortenerService, 
    AppDbContext dbContext, 
    HttpContext httpContext
    ) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("A URL especificada é inválida.");
    }

    var code = await urlShortenerService.GenerateUniqueCode();

    var shortenedUrl = new ShortenedUrl
    {
        Id = Guid.NewGuid(),
        LongUrl = request.Url,
        Code = code,
        ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
        CreatedAt = DateTime.UtcNow,
    };

    dbContext.ShortenedUrls.Add(shortenedUrl);
    await dbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});

app.MapGet("api/{code}", async (string code, AppDbContext dbContext) =>
{
    var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);

    if (shortenedUrl is null)
    {
        return Results.NotFound();
    }

    return Results.Redirect(shortenedUrl.LongUrl);
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
