namespace SiteDePrediction;

public static class SimpleTests
{
    public static void Run()
    {
        string testFile = Path.Combine(Path.GetTempPath(), $"prediction-tests-{Guid.NewGuid()}.json");

        try
        {
            var dataStore = new PredictionDataStore(testFile);
            var service = new PredictionService(dataStore);

            Prediction prediction = service.Create(new Prediction
            {
                Question = "Le test passera-t-il ?",
                Category = Category.Technologie,
                ClosingDate = DateTime.UtcNow.AddDays(7)
            });

            AssertThrows(() => service.Create(new Prediction
            {
                Id = prediction.Id,
                Question = "Identifiant déjà utilisé ?",
                Category = Category.Technologie,
                ClosingDate = DateTime.UtcNow.AddDays(7)
            }), "Une prédiction ne doit pas pouvoir être créée avec un Id déjà existant.");

            Assert(prediction.TotalVotes == 0, "Une nouvelle prédiction doit avoir 0 vote.");
            Assert(prediction.YesPercentage == 0, "Avec 0 vote, le pourcentage Oui doit être 0.");
            Assert(prediction.NoPercentage == 0, "Avec 0 vote, le pourcentage Non doit être 0.");

            service.Vote(prediction.Id, userId: 1, choice: true);
            Prediction votedPrediction = service.GetById(prediction.Id)!;
            Assert(votedPrediction.YesVotes == 1, "Le vote Oui doit être compté.");
            Assert(votedPrediction.YesPercentage == 100, "Un vote Oui sur un total de 1 doit donner 100%.");

            AssertThrows(() => service.Vote(prediction.Id, userId: 1, choice: false), "Un utilisateur ne doit pas voter deux fois.");

            Prediction expiredPrediction = service.Create(new Prediction
            {
                Question = "Vote tardif ?",
                Category = Category.Sport,
                ClosingDate = DateTime.UtcNow.AddDays(1)
            });
            PredictionDatabase database = dataStore.Load();
            database.Predictions.First(item => item.Id == expiredPrediction.Id).ClosingDate = DateTime.UtcNow.AddDays(-1);
            dataStore.Save(database);
            AssertThrows(() => service.Vote(expiredPrediction.Id, userId: 1, choice: true), "Un vote après la date limite doit être refusé.");

            service.ClosePrediction(prediction.Id, finalOutcome: true);
            AssertThrows(() => service.Vote(prediction.Id, userId: 2, choice: false), "Une prédiction clôturée ne doit plus accepter de vote.");
            AssertThrows(() => service.ClosePrediction(prediction.Id, finalOutcome: true), "Une prédiction déjà clôturée ne doit pas être clôturée une deuxième fois.");

            AssertThrows(() => service.Create(new Prediction
            {
                Question = "",
                Category = Category.Sport,
                ClosingDate = DateTime.UtcNow.AddDays(1)
            }), "Une question vide doit être refusée.");

            AssertThrows(() => service.Create(new Prediction
            {
                Question = "Date passée ?",
                Category = Category.Sport,
                ClosingDate = DateTime.UtcNow.AddDays(-1)
            }), "Une date passée doit être refusée.");

            Console.WriteLine("SUCCÈS : tous les tests simples sont passés.");
        }
        finally
        {
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
        }
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception("Test échoué : " + message);
        }
    }

    private static void AssertThrows(Action action, string message)
    {
        try
        {
            action();
        }
        catch
        {
            return;
        }

        throw new Exception("Test échoué : " + message);
    }
}
