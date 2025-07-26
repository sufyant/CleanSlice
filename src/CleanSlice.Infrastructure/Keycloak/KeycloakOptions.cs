namespace CleanSlice.Infrastructure.Keycloak;

public sealed class KeycloakOptions
{
    public const string SectionName = "Keycloak";
    
    public string BaseUrl { get; set; } = string.Empty;
    public string AdminUrl { get; set; } = string.Empty;
    public string TokenUrl { get; set; } = string.Empty;
    public string AdminClientId { get; set; } = string.Empty;
    public string AdminClientSecret { get; set; } = string.Empty;
    public string AuthClientId { get; set; } = string.Empty;
    public string AuthClientSecret { get; set; } = string.Empty;
    public string Realm { get; set; } = "clean-slice";
}
