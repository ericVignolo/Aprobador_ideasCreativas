using System;
using System.Collections.Generic;

namespace TextAnalyzer.Services;

public static class SynonymDictionary
{
    private static readonly Dictionary<string, string> _synonyms = new(StringComparer.OrdinalIgnoreCase)
    {
        { "tema", "cancion" },
        { "temas", "cancion" },
        { "canciones", "cancion" },
        { "pista", "cancion" },
        
        { "lista", "listar" },
        { "listas", "listar" },
        
        { "mostrar", "ver" },
        { "visualizar", "ver" },
        { "verla", "ver" },

        { "reproducir", "escuchar" },
        { "reproduccion", "escuchar" },
        { "reproducidas", "escuchar" },
        { "escuchadas", "escuchar" },
        { "escuche", "escuchar" },
        { "escucho", "escuchar" },
        
        { "ultima", "ultimo" },
        { "última", "ultimo" },
        { "último", "ultimo" }
    };

    public static string Normalize(string word)
    {
        return _synonyms.TryGetValue(word, out var normalizedWord) ? normalizedWord : word;
    }
}
