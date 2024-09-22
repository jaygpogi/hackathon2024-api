Fire this block to seed dummy data
```
IndexDocumentsBatch<Answer> indexDocumentsbatch = IndexDocumentsBatch.Create(
    IndexDocumentsAction.MergeOrUpload(
        new Answer()
        {
            Id = "1",
            Tags = new string[] { "Assignments Exceeding Budget", "Timesheets Exceeding Budget" },
            Problem = "Budget enforcement not working for reprocessed timesheets.",
            Investigation = "This is by design. Reprocessed timesheets are by default excluded from budget enforcement. There is a config setting though that overrides this: BTE.TimeAndExpense.EnableBudgetEnforcementForReprocessedBTE",
            Recommendation = "Explain to client that this is intended behavior. And refer to config team if client wants to enable the config setting as this might need a Service Request.",
            Related = "CWS-600889"
        }),
    IndexDocumentsAction.MergeOrUpload(
        new Answer()
        {
            Id = "2",
            Tags = new string[] { "Estimated Resource Plan", "Competitive Bid" },
            Problem = "Estimated Resource Plan budget not reflecting on Supplier Bid Evaluation screen.",
            Investigation = "Estimated Resource Plan is documented as not supported for competitive bids.",
            Recommendation = "Explain to client that this is intended behavior. And will need to undergo the enhancement route if they need this feature.",
            Related = "CWS-602133"
        }),
    IndexDocumentsAction.MergeOrUpload(
        new Answer()
        {
            Id = "3",
            Tags = new string[] { "Report JSON API", "Reporting", "Timeout" },
            Problem = "API report (RaaS) timing out.",
            Investigation = "As documented, API reports in general need to be running within 2 minutes and 100k rows.",
            Recommendation = "The report should be ran in report builder UI as JSON output first and made sure to succeed. If not, the report will need to be split into categories or filtered further to decrease reads and data retrieved.",
            Related = "CWS-601309"
        })
    );
await searchClient.IndexDocumentsAsync(indexDocumentsbatch);
```