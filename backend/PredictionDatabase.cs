namespace SiteDePrediction;

// Regroupe les données enregistrées dans data.json.
public class PredictionDatabase
{
    public List<Prediction> Predictions { get; set; } = new();
    public List<User> Users { get; set; } = new();
}
