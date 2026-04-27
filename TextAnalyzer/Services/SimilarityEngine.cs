using System;
using System.Collections.Generic;
using System.Linq;
using TextAnalyzer.Models;

namespace TextAnalyzer.Services;

public class SimilarityEngine
{
    private readonly SpanishTokenizer _tokenizer;

    public SimilarityEngine(SpanishTokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }

    public List<IdeaPair> FindSimilarIdeas(IEnumerable<(int Id, string Text, string TeamName)> ideas, double threshold = 0.35)
    {
        var results = new List<IdeaPair>();
        
        var tokenizedIdeas = ideas.Select(idea => new 
        { 
            Id = idea.Id,
            Original = idea.Text, 
            TeamName = idea.TeamName,
            Tokens = _tokenizer.Tokenize(idea.Text) 
        }).ToList();

        // Comparar todas contra todas (N x N / 2 iteraciones)
        for (int i = 0; i < tokenizedIdeas.Count; i++)
        {
            for (int j = i + 1; j < tokenizedIdeas.Count; j++)
            {
                var idea1 = tokenizedIdeas[i];
                var idea2 = tokenizedIdeas[j];

                double similarity = CalculateJaccard(idea1.Tokens, idea2.Tokens);

                if (similarity >= threshold)
                {
                    results.Add(new IdeaPair
                    {
                        Id1 = idea1.Id,
                        Id2 = idea2.Id,
                        Idea1 = idea1.Original,
                        Idea2 = idea2.Original,
                        Team1Name = idea1.TeamName,
                        Team2Name = idea2.TeamName,
                        SimilarityScore = similarity
                    });
                }
            }
        }

        return results.OrderByDescending(r => r.SimilarityScore).ToList();
    }

    // Cálculo matemático Similitud de Jaccard
    private double CalculateJaccard(HashSet<string> set1, HashSet<string> set2)
    {
        if (set1.Count == 0 && set2.Count == 0) return 1.0;
        if (set1.Count == 0 || set2.Count == 0) return 0.0;

        int intersectionCounter = set1.Intersect(set2, StringComparer.OrdinalIgnoreCase).Count();
        int unionCounter = set1.Union(set2, StringComparer.OrdinalIgnoreCase).Count();

        return (double)intersectionCounter / unionCounter;
    }
}
