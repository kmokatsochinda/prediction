using System.Text.Json.Serialization;

namespace SiteDePrediction;

public enum Category
{
    Technologie,
    Sport,
    Culture,
    Temperature,
    Science,
    Politique,
    Jeux,
    Universitaire,
    Finance
}

public enum PredictionStatus
{
    Open,
    Closed
}

// Représente une question sur laquelle les utilisateurs peuvent voter Oui ou Non.
public class Prediction
{
    // Identifiant utilisé pour retrouver la prédiction.
    public int Id { get; set; }

    // Question affichée aux utilisateurs.
    public string Question { get; set; } = string.Empty;

    public Category Category { get; set; }
    public PredictionStatus Status { get; set; } = PredictionStatus.Open;

    // Date après laquelle on ne peut plus voter.
    public DateTime ClosingDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    // Résultat final une fois la prédiction clôturée.
    public bool? FinalOutcome { get; set; }

    public List<Vote> Votes { get; set; } = new();

    // Ces valeurs sont calculées à partir de la liste des votes.
    [JsonIgnore]
    public int TotalVotes => Votes.Count;

    [JsonIgnore]
    public int YesVotes => Votes.Count(vote => vote.IsYes);

    [JsonIgnore]
    public int NoVotes => Votes.Count(vote => !vote.IsYes);

    [JsonIgnore]
    public double YesPercentage => TotalVotes == 0 ? 0 : Math.Round((double)YesVotes / TotalVotes * 100, 1);

    [JsonIgnore]
    public double NoPercentage => TotalVotes == 0 ? 0 : Math.Round((double)NoVotes / TotalVotes * 100, 1);
}
