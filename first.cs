using System.Globalization;
using System.Net;

namespace SiteDePrediction;

public class HtmlPageGenerator
{
    private readonly PredictionService predictionService;

    public HtmlPageGenerator(PredictionService predictionService)
    {
        this.predictionService = predictionService;
    }

    public void Generate()
    {
        IReadOnlyList<Prediction> predictions = predictionService.GetAll();
        File.WriteAllText("index.html", BuildIndexPage(predictions));
        File.WriteAllText("PageDeResultat.html", BuildResultsPage(predictions));
    }

    private static string BuildIndexPage(IReadOnlyList<Prediction> predictions)
    {
        string cards = string.Join(Environment.NewLine, predictions.Select(BuildPredictionCard));
        string filters = string.Join(Environment.NewLine, Enum.GetNames<Category>().Select(category =>
            $"            <button class=\"filter-btn\">{Encode(category)}</button>"));

        return $@"<!DOCTYPE html>
<html lang=""fr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Prédire | Sondages prédictifs</title>
    <link rel=""stylesheet"" href=""style.css"">
</head>
<body>
    <header class=""header"">
        <div class=""header-container"">
            <a href=""index.html"" class=""logo"">
                <img class=""logo-icon"" src=""wwwroot/images/parismarket-logo.png"" alt=""ParisMarket"">
                <span class=""logo-text"">ParisMarket</span>
            </a>
            <nav class=""nav"">
                <a href=""index.html"" class=""nav-link active"">Accueil</a>
                <a href=""PageCréer.html"" class=""nav-link"">Créer</a>
                <a href=""PageDeResultat.html"" class=""nav-link"">Résultats</a>
                <button id=""theme-toggle"" class=""theme-toggle"" title=""Changer de mode"">☀️</button>
            </nav>
        </div>
    </header>

    <section class=""hero"">
        <div class=""container"">
            <h1>Que va-t-il se passer ensuite ?</h1>
            <p>Votez sur les prédictions concernant les événements futurs dans la technologie, le sport, la culture et plus encore.</p>
            <div class=""stats-grid"">
                <div class=""stat-card"">
                    <span class=""stat-number"">{predictions.Count}</span>
                    <span class=""stat-label"">Prédictions actives</span>
                </div>
                <div class=""stat-card"">
                    <span class=""stat-number"">{predictions.Sum(prediction => prediction.TotalVotes)}</span>
                    <span class=""stat-label"">Nombre total de votes</span>
                </div>
            </div>
        </div>
    </section>

    <div class=""container"">
        <div class=""filters"">
            <button class=""filter-btn active"">Tous</button>
{filters}
        </div>
    </div>

    <main class=""container"">
        <div class=""poll-grid"">
{cards}
        </div>
    </main>

    <footer class=""main-footer"">
        © 2026 Prédire. La prédiction est entre vos mains. Ynov campus. Maxence, Karl, Eddie.
    </footer>
    <script src=""theme.js""></script>
</body>
</html>";
    }

    private static string BuildResultsPage(IReadOnlyList<Prediction> predictions)
    {
        string cards = string.Join(Environment.NewLine, predictions.Select(BuildResultCard));

        return $@"<!DOCTYPE html>
<html lang=""fr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <link rel=""stylesheet"" href=""style.css"">
    <title>Résultats des Paris | ParisMarket</title>
</head>
<body>
    <header class=""header"">
        <div class=""header-container"">
            <a href=""index.html"" class=""logo""><img class=""logo-icon"" src=""wwwroot/images/parismarket-logo.png"" alt=""ParisMarket""><span class=""logo-text"">ParisMarket</span></a>
            <nav class=""nav"">
                <a href=""index.html"" class=""nav-link"">Accueil</a>
                <a href=""PageCréer.html"" class=""nav-link"">Créer</a>
                <a href=""PageDeResultat.html"" class=""nav-link active"">Résultats</a>
                <button id=""theme-toggle"" class=""theme-toggle"" title=""Changer de mode"">☀️</button>
            </nav>
        </div>
    </header>
    <main class=""container"" style=""padding-top:5rem;"">
        <div class=""hero"">
            <h1>Tableau des scores</h1>
            <p>Consultez les tendances calculées à partir des votes enregistrés.</p>
        </div>
        <div class=""poll-grid"">
{cards}
        </div>
    </main>
    <footer class=""main-footer"">
        © 2026 ParisMarket. Tous droits réservés.
    </footer>
    <script src=""theme.js""></script>
</body>
</html>";
    }

    private static string BuildPredictionCard(Prediction prediction)
    {
        string yesWidth = CssPercentage(prediction.YesPercentage);
        string noWidth = CssPercentage(prediction.NoPercentage);

        return $@"            <div class=""poll-card"">
                <div class=""poll-category"">{Encode(prediction.Category.ToString().ToUpperInvariant())}</div>
                <h2 class=""poll-question"">{Encode(prediction.Question)}</h2>

                <div class=""poll-actions"">
                    <button class=""vote-btn yes"" disabled title=""À connecter à PredictionService.Vote"">OUI</button>
                    <button class=""vote-btn no"" disabled title=""À connecter à PredictionService.Vote"">NON</button>
                </div>

                <div class=""progress-section"">
                    <div class=""progress-labels"">
                        <span class=""oui-label"">Oui, {DisplayPercentage(prediction.YesPercentage)}</span>
                        <span class=""non-label"">Non, {DisplayPercentage(prediction.NoPercentage)}</span>
                    </div>
                    <div class=""progress-bar-wrap"">
                        <div class=""progress-fill-yes"" style=""width: {yesWidth}""></div>
                        <div class=""progress-fill-no"" style=""width: {noWidth}""></div>
                    </div>
                </div>

                <div class=""poll-footer"">
                    <span>👤 {prediction.TotalVotes} votes</span>
                    <span>📅 Clôture : {prediction.ClosingDate:dd/MM/yyyy}</span>
                </div>
            </div>";
    }

    private static string BuildResultCard(Prediction prediction)
    {
        string status = prediction.Status == PredictionStatus.Closed ? "Clôturée" : "Ouverte";
        string finalOutcome = prediction.FinalOutcome is null
            ? "Résultat final : non défini"
            : $"Résultat final : {(prediction.FinalOutcome.Value ? "OUI" : "NON")}";

        return $@"            <div class=""poll-card"">
                <div class=""poll-category"">{Encode(prediction.Category.ToString().ToUpperInvariant())} - {status}</div>
                <h2 class=""poll-question"">{Encode(prediction.Question)}</h2>
                <div class=""progress-section"">
                    <div class=""progress-labels"">
                        <span class=""oui-label"">OUI: {DisplayPercentage(prediction.YesPercentage)}</span>
                        <span class=""non-label"">NON: {DisplayPercentage(prediction.NoPercentage)}</span>
                    </div>
                    <div class=""progress-bar-wrap"">
                        <div class=""progress-fill-yes"" style=""width:{CssPercentage(prediction.YesPercentage)}""></div>
                    </div>
                </div>
                <div class=""poll-footer"">
                    <span>👤 {prediction.TotalVotes} votes</span>
                    <span>📅 Clôture : {prediction.ClosingDate:dd/MM/yyyy}</span>
                </div>
                <p>{finalOutcome}</p>
            </div>";
    }

    private static string CssPercentage(double value)
    {
        return value.ToString("0.0", CultureInfo.InvariantCulture) + "%";
    }

    private static string DisplayPercentage(double value)
    {
        return value.ToString("0.0", CultureInfo.GetCultureInfo("fr-FR")) + "%";
    }

    private static string Encode(string value)
    {
        return WebUtility.HtmlEncode(value);
    }
}
