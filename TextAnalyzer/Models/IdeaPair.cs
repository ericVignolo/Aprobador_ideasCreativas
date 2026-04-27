namespace TextAnalyzer.Models;

public class IdeaPair
{
    public int Id1 { get; set; }
    public int Id2 { get; set; }
    public string Idea1 { get; set; } = string.Empty;
    public string Idea2 { get; set; } = string.Empty;
    public string Team1Name { get; set; } = string.Empty;
    public string Team2Name { get; set; } = string.Empty;
    public double SimilarityScore { get; set; }
}
