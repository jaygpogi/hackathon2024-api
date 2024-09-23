using Azure.Search.Documents.Indexes;

namespace HackathonAPI
{
    public class Answer
    {
        [SimpleField(IsKey = true)]
        public string Id { get; set; }
        [SearchableField]
        public string[] Tags { get; set; }
        [SearchableField]
        public string Problem { get; set; }
        [SimpleField]
        public string Investigation { get; set; }
        [SimpleField]
        public string Recommendation { get; set; }
        [SimpleField]
        public string Related { get; set; }
    }
}
