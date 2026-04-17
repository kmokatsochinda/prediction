using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace SiteDePrediction
{
    public enum Category { Technologie, Sport, Culture, Temperature, Universitaire, Finance }
    public enum PredictionStatus { Open, Resolved }

    public class Prediction
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Category Category { get; set; }
        public int YesVotes { get; set; }
        public int NoVotes { get; set; }
        public PredictionStatus Status { get; set; } = PredictionStatus.Open;
        public string ClosingDate { get; set; } = "31 Dec 2026";

        public int Total => YesVotes + NoVotes;
        public double YesPct => Total == 0 ? 50 : Math.Round((double)YesVotes / Total * 100, 1);
        public double NoPct => Total == 0 ? 50 : Math.Round((double)NoVotes / Total * 100, 1);
        public double OddsYes => YesPct == 0 ? 10 : Math.Round(100 / YesPct, 2);
        public double OddsNo => NoPct == 0 ? 10 : Math.Round(100 / NoPct, 2);

        public Prediction() { }
        public Prediction(int id, string question, Category category, string date) 
        { 
            Id = id; 
            Question = question; 
            Category = category;
            ClosingDate = date;
        }
    }

    public class PredictionEngine
    {
        public List<Prediction> Predictions = new List<Prediction>();

        public void Load()
        {
            if (File.Exists("data.json")) 
                Predictions = JsonSerializer.Deserialize<List<Prediction>>(File.ReadAllText("data.json"));
            else 
                InitDefaults();
        }

        private void InitDefaults()
        {
            Predictions.Add(new Prediction(1, "La France gagnera-t-elle la Coupe du Monde 2026 ?", Category.Sport, "18 Dec 2026") { YesVotes = 450, NoVotes = 200 });
            Predictions.Add(new Prediction(2, "Le Bitcoin dépassera-t-il les 100k$ avant 2027 ?", Category.Finance, "1 Jan 2027") { YesVotes = 800, NoVotes = 150 });
            Predictions.Add(new Prediction(3, "L'IA remplacera-t-elle les développeurs juniors cette année ?", Category.Technologie, "31 Dec 2026") { YesVotes = 120, NoVotes = 600 });
        }

        public void Save() => File.WriteAllText("data.json", JsonSerializer.Serialize(Predictions, new JsonSerializerOptions { WriteIndented = true }));

        public void GenerateHtml()
        {
            string cards = string.Join("", Predictions.Select(p => $@"
            <div class='poll-card'>
                <div class='poll-category'>{p.Category.ToString().ToUpper()} {(p.Status == PredictionStatus.Resolved ? "✅" : "⏳")}</div>
                <h2 class='poll-question'>{p.Question}</h2>
                <div class='progress-section'>
                    <div class='progress-labels'>
                        <span class='oui-label'>OUI: {p.YesPct}% (Cote: {p.OddsYes})</span>
                        <span class='non-label'>NON: {p.NoPct}% (Cote: {p.OddsNo})</span>
                    </div>
                    <div class='progress-bar-wrap'>
                        <div class='progress-fill-yes' style='width:{p.YesPct}%'></div>
                    </div>
                </div>
                <div class='poll-footer'>
                    <span>👤 {p.Total} votes</span>
                    <span>📅 Clôture : {p.ClosingDate}</span>
                </div>
            </div>"));

            string template = $@"<!DOCTYPE html>
<html lang='fr'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <link rel='stylesheet' href='style.css'>
    <title>Résultats des Paris | Blue Team</title>
</head>
<body>
    <header class='header'>
        <div class='header-container'>
            <a href='index.html' class='logo'>🔥 Blue Team : Paries</a>
            <nav class='nav'>
                <a href='index.html' class='nav-link'>Accueil</a>
                <a href='PageCréer.html' class='nav-link'>Créer</a>
                <a href='#' class='nav-link active'>Résultats</a>
                <button id='theme-toggle' class='theme-toggle' title='Changer de mode'>☀️</button>
            </nav>
        </div>
    </header>
    <main class='container' style='padding-top:5rem;'>
        <div class='hero'>
            <h1>💰 Tableau des Scores</h1>
            <p>Consultez les tendances en temps réel sur les événements majeurs.</p>
        </div>
        <div class='poll-grid'>{cards}</div>
    </main>
    <footer class='main-footer'>
        © 2026 Blue Team. Tous droits réservés.
    </footer>
    <script src='theme.js'></script>
</body>
</html>";
            File.WriteAllText("PageDeResultat.html", template);
        }
    }

    class Program
    {
        static void Main()
        {
            var engine = new PredictionEngine();
            engine.Load();
            engine.Save();
            engine.GenerateHtml();
            Console.WriteLine("SUCCÈS : Base de données synchronisée et interface de résultats générée.");
        }
    }
}
