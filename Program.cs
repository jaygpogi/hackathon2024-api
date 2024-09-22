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

    //// seed only
    //IndexDocumentsBatch<Answer> indexDocumentsbatch = IndexDocumentsBatch.Create(
    //    IndexDocumentsAction.MergeOrUpload(
    //        new Answer()
    //        {
    //            Id = "1",
    //            Tags = new string[] { "Assignments Exceeding Budget", "Timesheets Exceeding Budget" },
    //            Problem = "Budget enforcement not working for reprocessed timesheets.",
    //            Investigation = "This is by design. Reprocessed timesheets are by default excluded from budget enforcement. There is a config setting though that overrides this: BTE.TimeAndExpense.EnableBudgetEnforcementForReprocessedBTE",
    //            Recommendation = "Explain to client that this is intended behavior. And refer to config team if client wants to enable the config setting as this might need a Service Request.",
    //            Related = "CWS-600889"
    //        }),
    //    IndexDocumentsAction.MergeOrUpload(
    //        new Answer()
    //        {
    //            Id = "2",
    //            Tags = new string[] { "Estimated Resource Plan", "Competitive Bid" },
    //            Problem = "Estimated Resource Plan budget not reflecting on Supplier Bid Evaluation screen.",
    //            Investigation = "Estimated Resource Plan is documented as not supported for competitive bids.",
    //            Recommendation = "Explain to client that this is intended behavior. And will need to undergo the enhancement route if they need this feature.",
    //            Related = "CWS-602133"
    //        }),
    //    IndexDocumentsAction.MergeOrUpload(
    //        new Answer()
    //        {
    //            Id = "3",
    //            Tags = new string[] { "Report JSON API", "Reporting", "Timeout" },
    //            Problem = "API report (RaaS) timing out.",
    //            Investigation = "As documented, API reports in general need to be running within 2 minutes and 100k rows.",
    //            Recommendation = "The report should be ran in report builder UI as JSON output first and made sure to succeed. If not, the report will need to be split into categories or filtered further to decrease reads and data retrieved.",
    //            Related = "CWS-601309"
    //        })
    //    );
    //await searchClient.IndexDocumentsAsync(indexDocumentsbatch);

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
    var result = response.GetResults().OrderByDescending(x => x.Score.Value).FirstOrDefault();
    if (result != null)
    {
        return result;
    }
    return null;
}).DisableAntiforgery();

app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
);

app.Run();