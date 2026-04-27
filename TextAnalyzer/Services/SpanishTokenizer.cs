using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace TextAnalyzer.Services;

public class SpanishTokenizer
{
    private readonly HashSet<string> _ignoreWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "a", "ante", "bajo", "cabe", "con", "contra", "de", "desde", "durante", 
        "en", "entre", "hacia", "hasta", "mediante", "para", "por", "según", "segun", 
        "sin", "so", "sobre", "tras", "versus", "vía", "via",
        "el", "la", "los", "las", "un", "una", "unos", "unas", "lo", "al", "del",
        "que", "y", "e", "o", "u", "ni", "pero", "aunque", "mas", "sino",
        "mi", "mis", "tu", "tus", "su", "sus", "nuestro", "nuestra", "sean", "son",
        "cuantas", "cual", "cuales", "como", "cuando", "donde", "quien", "quienes",
        "me", "te", "se", "nos", "os", "le", "les", "este", "esta", "estos", "estas", "cualquier", "mismo"
    };

    public HashSet<string> Tokenize(string text)
    {
        var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        string cleanText = RemoveDiacritics(text);
        cleanText = Regex.Replace(cleanText, @"[^\w\s]", "");
        string[] words = cleanText.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var word in words)
        {
            if (!_ignoreWords.Contains(word))
            {
                string normalized = SynonymDictionary.Normalize(word.ToLower());
                result.Add(normalized); // HashSet ignora los duplicados matemáticamente
            }
        }

        return result;
    }

    // Remueve acentos y diacríticos ("canción" -> "cancion")
    private string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
