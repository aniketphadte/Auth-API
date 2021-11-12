using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth.Core.Models
{
    public interface ILoginDetails
    {
        public string SecurityToken { get; set; }
    }
    public class LoginDetails : ILoginDetails
    {
        [JsonPropertyName("tokenDetails")]
        public TokenDetails TokenDetails { get; set; }
        [JsonPropertyName("securityToken")]
        public string SecurityToken { get; set; }
    }

    public class TokenDetails
    {
        [JsonPropertyName("userId")]
        public string UserId { get; set; }
        [JsonPropertyName("idToken")]
        public string IdToken { get; set; }
        [JsonPropertyName("issueTimestamp")]
        public long IssueTimestamp { get; set; }
        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
