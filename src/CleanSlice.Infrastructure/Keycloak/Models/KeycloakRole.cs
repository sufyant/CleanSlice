using Newtonsoft.Json;

namespace CleanSlice.Infrastructure.Keycloak.Models;

public sealed class KeycloakRole
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;
    
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
    
    [JsonProperty("composite")]
    public bool Composite { get; set; }
    
    [JsonProperty("clientRole")]
    public bool ClientRole { get; set; }
    
    [JsonProperty("containerId")]
    public string ContainerId { get; set; } = string.Empty;
    
    [JsonProperty("attributes")]
    public Dictionary<string, List<string>> Attributes { get; set; } = new();
}
