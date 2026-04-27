using System.Collections.Generic;
using System.Linq;
using TextAnalyzer.Models;

namespace TextAnalyzer.Services;

public class AnalyzerEngine
{
    private readonly TextTokenizer _tokenizer;

    public AnalyzerEngine(TextTokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public Dictionary<string, WordStats> Analyze(List<Paragraph> paragraphs)
    {
        var index = new Dictionary<string, WordStats>();

        foreach (var p in paragraphs)
        {
            var tokens = _tokenizer.Tokenize(p.OriginalText);
            foreach (var token in tokens)
            {
                if (!index.ContainsKey(token.Word))
                {
                    index[token.Word] = new WordStats { NormalizedWord = token.Word };
                }

                index[token.Word].Locations.Add(new WordLocation
                {
                    ParagraphId = p.Id,
                    Position = token.Position
                });
            }
        }

        return index;
    }

    // Retorna solo las palabras que aparecen en más de un párrafo distinto
    public Dictionary<string, WordStats> GetSharedWords(Dictionary<string, WordStats> index)
    {
        return index.Where(kvp => kvp.Value.Locations.Select(l => l.ParagraphId).Distinct().Count() > 1)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}
