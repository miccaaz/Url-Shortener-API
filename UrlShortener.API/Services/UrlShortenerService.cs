namespace UrlShortener.API.Services;

public class UrlShortenerService
{
    private const int NumberOfCharInShortLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();

    public string GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharInShortLink];

        for (var i = 0; i < NumberOfCharInShortLink; i++)
        {
            var randomindex = _random.Next(Alphabet.Length - 1);

            codeChars[i] = Alphabet[randomindex];
        }

        var code = new string(codeChars);

        return code;
    }
}
