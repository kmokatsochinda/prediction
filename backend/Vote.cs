namespace SiteDePrediction;

// Représente le vote d'un utilisateur sur une prédiction.
public class Vote
{
    public int Id { get; set; }
    public int PredictionId { get; set; }
    public int UserId { get; set; }

    // true = Oui, false = Non.
    public bool IsYes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
