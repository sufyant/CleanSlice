using Newtonsoft.Json;

namespace CleanSlice.Infrastructure.Keycloak.Models;

public sealed class KeycloakTokenResponse
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    
    [JsonProperty("refresh_expires_in")]
    public int RefreshExpiresIn { get; set; }
    
    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;
    
    [JsonProperty("not-before-policy")]
    public int NotBeforePolicy { get; set; }
    
    [JsonProperty("scope")]
    public string Scope { get; set; } = string.Empty;
}
