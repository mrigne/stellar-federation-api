using System.Text.Json.Serialization;

namespace StellarFederationApi.Models
{
    public class FederationResponse
    {
        [JsonPropertyName("stellar_address")]
        public string StellarAddress { get; set; }
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }
        [JsonPropertyName("memo_type")]
        public MemoType MemoType { get; set; }
        [JsonPropertyName("memo")]
        public string Memo { get; set; }
    }
}