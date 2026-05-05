namespace SiteDePrediction;

// Contient les règles principales des prédictions.
public class PredictionService
{
    private readonly PredictionDataStore dataStore;

    public PredictionService(PredictionDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    // Retourne toutes les prédictions.
    public IReadOnlyList<Prediction> GetAll()
    {
        return dataStore.Load().Predictions;
    }

    // Cherche une prédiction avec son identifiant.
    public Prediction? GetById(int id)
    {
        return dataStore.Load().Predictions.FirstOrDefault(prediction => prediction.Id == id);
    }

    // Crée une prédiction après avoir vérifié les règles de base.
    public Prediction Create(Prediction prediction)
    {
        PredictionDatabase database = dataStore.Load();

        ValidatePrediction(prediction);

        // Evite d'avoir deux prédictions avec le même identifiant.
        if (prediction.Id != 0 && database.Predictions.Any(item => item.Id == prediction.Id))
        {
            throw new InvalidOperationException("Une prédiction avec cet identifiant existe déjà.");
        }

        prediction.Id = prediction.Id == 0
            ? GetNextPredictionId(database)
            : prediction.Id;
        prediction.Status = PredictionStatus.Open;
        prediction.CreatedAt = prediction.CreatedAt == default ? DateTime.UtcNow : prediction.CreatedAt;
        prediction.ClosedAt = null;
        prediction.FinalOutcome = null;

        database.Predictions.Add(prediction);
        dataStore.Save(database);

        return prediction;
    }

    // Ajoute un vote si l'utilisateur a le droit de voter.
    public Vote Vote(int predictionId, int userId, bool choice)
    {
        PredictionDatabase database = dataStore.Load();
        Prediction prediction = FindPredictionOrThrow(database, predictionId);

        // Une prédiction clôturée ne doit plus recevoir de vote.
        if (prediction.Status == PredictionStatus.Closed)
        {
            throw new InvalidOperationException("Impossible de voter sur une prédiction clôturée.");
        }

        // Le vote est refusé après la date limite.
        if (DateTime.UtcNow > prediction.ClosingDate)
        {
            throw new InvalidOperationException("Impossible de voter après la date limite.");
        }

        // On vérifie que l'utilisateur existe dans les données.
        if (!database.Users.Any(user => user.Id == userId))
        {
            throw new InvalidOperationException("Utilisateur introuvable.");
        }

        // Un utilisateur ne peut voter qu'une seule fois.
        if (prediction.Votes.Any(vote => vote.UserId == userId))
        {
            throw new InvalidOperationException("Un utilisateur ne peut voter qu'une seule fois par prédiction.");
        }

        var vote = new Vote
        {
            Id = GetNextVoteId(database),
            PredictionId = predictionId,
            UserId = userId,
            IsYes = choice,
            CreatedAt = DateTime.UtcNow
        };

        prediction.Votes.Add(vote);
        dataStore.Save(database);

        return vote;
    }

    // Clôture une prédiction et enregistre le résultat final.
    public Prediction ClosePrediction(int predictionId, bool finalOutcome)
    {
        PredictionDatabase database = dataStore.Load();
        Prediction prediction = FindPredictionOrThrow(database, predictionId);

        // On évite de clôturer deux fois la même prédiction.
        if (prediction.Status == PredictionStatus.Closed)
        {
            throw new InvalidOperationException("Cette prédiction est déjà clôturée.");
        }

        prediction.Status = PredictionStatus.Closed;
        prediction.FinalOutcome = finalOutcome;
        prediction.ClosedAt = DateTime.UtcNow;

        dataStore.Save(database);
        return prediction;
    }

    private static void ValidatePrediction(Prediction prediction)
    {
        // Une prédiction doit toujours avoir une question lisible.
        if (string.IsNullOrWhiteSpace(prediction.Question))
        {
            throw new ArgumentException("La question ne peut pas être vide.");
        }

        // La date de clôture doit laisser le temps de voter.
        if (prediction.ClosingDate <= DateTime.UtcNow)
        {
            throw new ArgumentException("La date de clôture doit être future.");
        }
    }

    private static Prediction FindPredictionOrThrow(PredictionDatabase database, int predictionId)
    {
        return database.Predictions.FirstOrDefault(prediction => prediction.Id == predictionId)
            ?? throw new InvalidOperationException("Prédiction introuvable.");
    }

    private static int GetNextPredictionId(PredictionDatabase database)
    {
        return database.Predictions.Count == 0 ? 1 : database.Predictions.Max(prediction => prediction.Id) + 1;
    }

    private static int GetNextVoteId(PredictionDatabase database)
    {
        IEnumerable<Vote> votes = database.Predictions.SelectMany(prediction => prediction.Votes);
        return votes.Any() ? votes.Max(vote => vote.Id) + 1 : 1;
    }
}
