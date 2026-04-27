using System.Text.RegularExpressions;

namespace TextAnalyzer.Services;

public class TextTokenizer
{
    // Lista de palabras a ignorar (stop-words) comunes para el ejemplo y más
    private readonly HashSet<string> _ignoreWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "die", "das", "der", "dem", "den", "des", 
        "ein", "eine", "einer", "eines", "einem", "einen",
        "und", "oder", "in", "aus", "zu", "von", "mit", "für",
        "denen", "damit", "seine", "ihre", "dass", "als", "an", "auf"
    };

    public List<(string Word, int Position)> Tokenize(string text)
    {
        var result = new List<(string Word, int Position)>();
        
        // Quitar la puntuación y separar por espacios
        string cleanText = Regex.Replace(text, @"[^\w\s]", "");
        string[] words = cleanText.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        int pos = 1; // La primera palabra del párrafo es la posición 1
        foreach (var word in words)
        {
            if (!_ignoreWords.Contains(word))
            {
                result.Add((word.ToLower(), pos));
            }
            pos++;
        }

        return result;
    }
}
