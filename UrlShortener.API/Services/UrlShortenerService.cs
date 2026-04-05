using Microsoft.EntityFrameworkCore;
using UrlShortener.API.Infrastructure;

namespace UrlShortener.API.Services;

public class UrlShortenerService
{
    public const int NumberOfCharInShortLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();
    private readonly AppDbContext _dbContext;

    public UrlShortenerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharInShortLink];

        while (true)
        {
            for (var i = 0; i < NumberOfCharInShortLink; i++)
            {
                var randomindex = _random.Next(Alphabet.Length - 1);

                codeChars[i] = Alphabet[randomindex];
            }

            var code = new string(codeChars);

            if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
            {
                return code;
            }
        }
    }
}
