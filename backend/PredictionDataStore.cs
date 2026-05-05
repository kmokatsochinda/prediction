using System.Text.Json;
using System.Text.Json.Serialization;

namespace SiteDePrediction;

// Lit et sauvegarde les données dans le fichier JSON.
public class PredictionDataStore
{
    private readonly string filePath;
    private readonly JsonSerializerOptions jsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public PredictionDataStore(string filePath = "data.json")
    {
        this.filePath = Path.GetFullPath(filePath, Directory.GetCurrentDirectory());
        jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    // Charge les données. Si le fichier n'existe pas, on utilise des données de départ.
    public PredictionDatabase Load()
    {
        if (!File.Exists(filePath))
        {
            return CreateDefaultDatabase();
        }

        string json = File.ReadAllText(filePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return CreateDefaultDatabase();
        }

        PredictionDatabase? database = JsonSerializer.Deserialize<PredictionDatabase>(json, jsonOptions);
        return database ?? CreateDefaultDatabase();
    }

    // Sauvegarde toutes les données dans le fichier JSON.
    public void Save(PredictionDatabase database)
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, JsonSerializer.Serialize(database, jsonOptions));
    }

    private static PredictionDatabase CreateDefaultDatabase()
    {
        var users = new List<User>
        {
            new() { Id = 1, Username = "alice" },
            new() { Id = 2, Username = "bob" },
            new() { Id = 3, Username = "charlie" }
        };

        return new PredictionDatabase
        {
            Users = users,
            Predictions = new List<Prediction>
            {
                new()
                {
                    Id = 1,
                    Question = "La France gagnera-t-elle la Coupe du Monde 2026 ?",
                    Category = Category.Sport,
                    ClosingDate = new DateTime(2026, 12, 18),
                    CreatedAt = new DateTime(2026, 04, 30),
                    Votes = new List<Vote>
                    {
                        new() { Id = 1, PredictionId = 1, UserId = 1, IsYes = true, CreatedAt = new DateTime(2026, 04, 30) },
                        new() { Id = 2, PredictionId = 1, UserId = 2, IsYes = true, CreatedAt = new DateTime(2026, 04, 30) },
                        new() { Id = 3, PredictionId = 1, UserId = 3, IsYes = false, CreatedAt = new DateTime(2026, 04, 30) }
                    }
                },
                new()
                {
                    Id = 2,
                    Question = "Le Bitcoin dépassera-t-il les 100k$ avant 2027 ?",
                    Category = Category.Finance,
                    ClosingDate = new DateTime(2027, 01, 01),
                    CreatedAt = new DateTime(2026, 04, 30)
                },
                new()
                {
                    Id = 3,
                    Question = "L'IA remplacera-t-elle les développeurs juniors cette année ?",
                    Category = Category.Technologie,
                    ClosingDate = new DateTime(2026, 12, 31),
                    CreatedAt = new DateTime(2026, 04, 30)
                }
            }
        };
    }
}
