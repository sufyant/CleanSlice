using Newtonsoft.Json;

namespace CleanSlice.Infrastructure.Keycloak.Models;

public sealed class KeycloakUser
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonProperty("username")]
    public string Username { get; set; } = string.Empty;
    
    [JsonProperty("email")]
    public string Email { get; set; } = string.Empty;
    
    [JsonProperty("firstName")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonProperty("lastName")]
    public string LastName { get; set; } = string.Empty;
    
    [JsonProperty("enabled")]
    public bool Enabled { get; set; } = true;
    
    [JsonProperty("emailVerified")]
    public bool EmailVerified { get; set; }
    
    [JsonProperty("createdTimestamp")]
    public long CreatedTimestamp { get; set; }
    
    [JsonProperty("attributes")]
    public Dictionary<string, List<string>> Attributes { get; set; } = new();
}
