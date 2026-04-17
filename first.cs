using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace SiteDePrediction
{
    public enum Category { Technologie, Sport, Culture, Temperature, Universitaire }
    public enum PredictionStatus { Open, Resolved }

    public class Bet
    {
        public bool IsYes { get; set; }
        public decimal Amount { get; set; }
        public double OddsAtTime { get; set; }
    }

    public class Prediction
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public Category Category { get; set; }
        public int YesVotes { get; set; }
        public int NoVotes { get; set; }
        public PredictionStatus Status { get; set; } = PredictionStatus.Open;
        public bool? Result { get; set; }

        public int Total => YesVotes + NoVotes;
        public double YesPct => Total == 0 ? 50 : Math.Round((double)YesVotes / Total * 100, 1);
        public double NoPct => Total == 0 ? 50 : Math.Round((double)NoVotes / Total * 100, 1);
        public double OddsYes => YesPct == 0 ? 10 : Math.Round(100 / YesPct, 2);
        public double OddsNo => NoPct == 0 ? 10 : Math.Round(100 / NoPct, 2);

        public Prediction() { } // Constructeur vide pour JSON
        public Prediction(int id, string question, Category category) 
        { 
            Id = id; 
            Question = question; 
            Category = category; 
        }
    }

    public class User
    {
        public string Name { get; set; }
        public decimal Balance { get; set; } = 1000;
        public List<Bet> Bets { get; set; } = new List<Bet>();

        public void Bet(Prediction p, bool isYes, decimal amount)
        {
            if (p.Status != PredictionStatus.Open || Balance < amount) return;
            Balance -= amount;
            Bets.Add(new Bet { IsYes = isYes, Amount = amount, OddsAtTime = isYes ? p.OddsYes : p.OddsNo });
            if (isYes) p.YesVotes++; else p.NoVotes++;
        }
    }

    public class PredictionEngine
    {
        public List<Prediction> Predictions = new List<Prediction>();

        public void Load()
        {
            if (File.Exists("data.json")) Predictions = JsonSerializer.Deserialize<List<Prediction>>(File.ReadAllText("data.json"));
            else InitDefaults();
        }

        private void InitDefaults()
        {
            Predictions.Add(new Prediction(1, "Monsieur Tétar sera-t-il à l'heure ?", Category.Universitaire) { YesVotes = 10, NoVotes = 500 });
            Predictions.Add(new Prediction(2, "La France gagnera le prochain match ?", Category.Sport) { YesVotes = 300, NoVotes = 200 });
        }

        public void Save() => File.WriteAllText("data.json", JsonSerializer.Serialize(Predictions));

        public void GenerateHtml()
        {
            string cards = string.Join("", Predictions.Select(p => $@"
            <div class='poll-card'>
                <div class='poll-category'>{p.Category} {(p.Status == PredictionStatus.Resolved ? "✅" : "⏳")}</div>
                <h2 class='poll-question'>{p.Question}</h2>
                <div class='progress-section'>
                    <div class='progress-labels'>
                        <span>OUI: {p.YesPct}% (Cote: {p.OddsYes})</span>
                        <span>NON: {p.NoPct}% (Cote: {p.OddsNo})</span>
                    </div>
                    <div class='progress-bar-wrap'>
                        <div class='progress-fill-yes' style='width:{p.YesPct}%'></div>
                    </div>
                </div>
            </div>"));

            string template = $@"<!DOCTYPE html><html lang='fr'><head><meta charset='UTF-8'><link rel='stylesheet' href='style.css'><title>Paries | Blue Team</title></head>
            <body><header class='header'><div class='header-container'><span class='logo'>🔥 Blue Team : Paries</span><nav class='nav'><a href='index.html' class='nav-link'>Accueil</a><a href='#' class='nav-link active'>Résultats</a></nav></div></header>
            <main class='container' style='padding-top:5rem;'><div class='hero'><h1>💰 Système de Paris</h1><p>Gérez vos jetons sur les événements futurs.</p></div>
            <div class='poll-grid'>{cards}</div></main></body></html>";
            File.WriteAllText("PageDeResultat.html", template);
        }
    }

    class Program
    {
        static void Main()
        {
            var engine = new PredictionEngine();
            engine.Load();
            
            // Simulation d'un pari de Karl
            var karl = new User { Name = "Karl" };
            engine.Predictions.Find(p => p.Id == 1)?.Id.ToString(); // Test existence
            karl.Bet(engine.Predictions[0], true, 100); 

            engine.Save();
            engine.GenerateHtml();
            Console.WriteLine("OK : Système de paris mis à jour et PageDeResultat.html générée.");
        }
    }
}
