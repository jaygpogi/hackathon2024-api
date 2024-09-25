## Guardians Inquiry Documentation
<img width="934" alt="image" src="https://github.com/user-attachments/assets/180a8790-c525-40c8-ae1e-ff360ac5c4f1">

## Flow

<img width="575" alt="image" src="https://github.com/user-attachments/assets/8e34dc12-313f-4359-9a48-eefb4c0b0a0a">

## Azure AI Language Extraction
<img width="535" alt="image" src="https://github.com/user-attachments/assets/6572f2cf-3c61-4468-96a1-79380c3a742c">

## Azure AI Search Indexes
<img width="813" alt="image" src="https://github.com/user-attachments/assets/cd38a6fb-38b0-43e5-8a71-d2e5314d6f6f">

<img width="484" alt="image" src="https://github.com/user-attachments/assets/903ea8b9-b18b-40f3-8503-c69a055c4780">

## API 
Endpoint: https://hackathonapi20240922091054.azurewebsites.net/

Sample body:
```
{
    "title": "India Budget Management - Assignments Exceeding Budget",
    "description": "The client reported that there is a shortfall of budget for 10 resources whose assignment expired on 4th June 2024. The timesheets submitted for the period of 1st to 4th June should have been rejected by the Beeline system due to budget insufficiency however these timesheets were submitted and approved as well making the Remaining Budget with Non Invoiced Cost go in negative. Attached are the details of the 10 resources."
}
```


# Samples:
- Without results: https://jira.beeline.com/browse/TECH-20752
- Behavioral issue: https://jira.beeline.com/browse/TECH-20749
- Feature not supported: https://jira.beeline.com/browse/TECH-20750
- Specific performance issue: https://jira.beeline.com/browse/TECH-20751

# Possible Future Development
- When a ticket gets resolved, Guardian devs fill up the Status Summary Jira field. This field indicates the problem, investigation, and recommendation. A new endpoint can be developed that can push new records on the search index based on this field if nothing is retrieved for a ticket.
- More Accuracy
    - Use Azure AI Language Text Classification for categorization. (Assignment, BTE, Reports, etc)
    - Declare a "bias" list of phrases to eliminate any non-related texts.
    - Right now this uses BM25 search algorithm which depends on term frequency. This should use semantic ranking logic that also understands contexts (but needs higher tier account).
  
