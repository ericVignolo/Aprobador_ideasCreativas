namespace TextAnalyzer.Models;

public class WordStats
{
    public string NormalizedWord { get; set; } = string.Empty;
    public int TotalCount => Locations.Count;
    public List<WordLocation> Locations { get; set; } = new List<WordLocation>();
}
