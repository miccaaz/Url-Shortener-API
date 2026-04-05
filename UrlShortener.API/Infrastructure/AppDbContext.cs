using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Entities;
using UrlShortener.API.Services;

namespace UrlShortener.API.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShortenerService.NumberOfCharInShortLink);
            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}
