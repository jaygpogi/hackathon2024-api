using HackathonAPI;
using Azure;
using Azure.AI.TextAnalytics;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;

// NUGET: Azure.Search.Documents, Azure.AI.TextAnalytics

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.WithOrigins("https://jira.beeline.com/");
}));
var app = builder.Build();

app.MapPost("/", async (JiraData jiraData) =>
{
    /// extract keys ///
    AzureKeyCredential languageCredential = new AzureKeyCredential(Environment.GetEnvironmentVariable("LANGUAGE_KEY"));
    Uri languageEndpoint = new Uri("https://jayglanguage.cognitiveservices.azure.com/");
    Response<KeyPhraseCollection> keyPhrases = new TextAnalyticsClient(languageEndpoint, languageCredential).ExtractKeyPhrases($"{jiraData.Title} {jiraData.Description}");
    string searchKeys = string.Join(",", keyPhrases.Value);

    // init search
    Uri serviceEndpoint = new Uri($"https://jaygaisearch.search.windows.net/");
    AzureKeyCredential credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("SEARCH_KEY"));
    SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);
    SearchClient searchClient = new SearchClient(serviceEndpoint, "jaygsearchindex", credential);

    // search
    SearchOptions options = new SearchOptions()
    {
        SearchFields = { "Tags" },
        QueryType = SearchQueryType.Full
    };

    options.Select.Add("Tags");
    options.Select.Add("Problem");
    options.Select.Add("Investigation");
    options.Select.Add("Recommendation");
    options.Select.Add("Related");

    SearchResults<Answer> response = searchClient.Search<Answer>(searchKeys, options);
    return response.GetResults().OrderByDescending(x => x.Score.Value).FirstOrDefault();
}).DisableAntiforgery();

app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);

app.Run();