using SiteDePrediction;

if (args.Contains("--test"))
{
    SimpleTests.Run();
    return;
}

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(new PredictionDataStore(Path.Combine(builder.Environment.ContentRootPath, "data.json")));
builder.Services.AddSingleton<PredictionService>();
builder.Services.AddSingleton<UserService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
