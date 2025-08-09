using System.Text.Json.Serialization;

namespace BookRadar.Common.Response
{
    public class OpenLibraryResponse
    {
        [JsonPropertyName("numFound")]
        public int NumFound { get; set; }

        [JsonPropertyName("docs")]
        public List<OpenLibraryDoc> Docs { get; set; } = [];
    }

    public class OpenLibraryDoc
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("publisher")]
        public List<string>? Publisher { get; set; }
    }
}