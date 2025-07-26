using System.Text;
using CleanSlice.Application.Abstractions.Keycloak;
using CleanSlice.Infrastructure.Keycloak.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CleanSlice.Infrastructure.Keycloak;

internal sealed class KeycloakService(
    HttpClient httpClient,
    IOptions<KeycloakOptions> options,
    ILogger<KeycloakService> logger)
    : IKeycloakService
{
    private readonly KeycloakOptions _options = options.Value;
    private string? _adminToken;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public async Task<string?> GetUserIdByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            var url = $"{_options.AdminUrl}users?email={Uri.EscapeDataString(email)}";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to get user by email {Email}. Status: {StatusCode}", email, response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var users = JsonConvert.DeserializeObject<List<KeycloakUser>>(content);
            
            return users?.FirstOrDefault()?.Id;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting user by email {Email}", email);
            return null;
        }
    }

    public async Task<bool> CreateUserAsync(string email, string firstName, string lastName, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            var user = new
            {
                username = email,
                email,
                firstName,
                lastName,
                enabled = true,
                emailVerified = true,
                credentials = new[]
                {
                    new
                    {
                        type = "password",
                        value = password,
                        temporary = false
                    }
                }
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var url = $"{_options.AdminUrl}users";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.PostAsync(url, content, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully created user {Email}", email);
                return true;
            }
            
            logger.LogWarning("Failed to create user {Email}. Status: {StatusCode}", email, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating user {Email}", email);
            return false;
        }
    }

    public async Task<bool> UpdateUserAsync(string userId, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            var user = new
            {
                email,
                firstName,
                lastName
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var url = $"{_options.AdminUrl}users/{userId}";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.PutAsync(url, content, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully updated user {UserId}", userId);
                return true;
            }
            
            logger.LogWarning("Failed to update user {UserId}. Status: {StatusCode}", userId, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            var url = $"{_options.AdminUrl}users/{userId}";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.DeleteAsync(url, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully deleted user {UserId}", userId);
                return true;
            }
            
            logger.LogWarning("Failed to delete user {UserId}. Status: {StatusCode}", userId, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            // First get the role
            var roleUrl = $"{_options.AdminUrl}roles/{roleName}";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var roleResponse = await httpClient.GetAsync(roleUrl, cancellationToken);
            if (!roleResponse.IsSuccessStatusCode)
            {
                logger.LogWarning("Role {RoleName} not found", roleName);
                return false;
            }

            var roleContent = await roleResponse.Content.ReadAsStringAsync(cancellationToken);
            var role = JsonConvert.DeserializeObject<KeycloakRole>(roleContent);
            
            if (role == null)
            {
                logger.LogWarning("Failed to deserialize role {RoleName}", roleName);
                return false;
            }

            // Assign role to user
            var roles = new[] { role };
            var json = JsonConvert.SerializeObject(roles);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var assignUrl = $"{_options.AdminUrl}users/{userId}/role-mappings/realm";
            var assignResponse = await httpClient.PostAsync(assignUrl, content, cancellationToken);
            
            if (assignResponse.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully assigned role {RoleName} to user {UserId}", roleName, userId);
                return true;
            }
            
            logger.LogWarning("Failed to assign role {RoleName} to user {UserId}. Status: {StatusCode}", 
                roleName, userId, assignResponse.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error assigning role {RoleName} to user {UserId}", roleName, userId);
            return false;
        }
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            // First get the role
            var roleUrl = $"{_options.AdminUrl}roles/{roleName}";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var roleResponse = await httpClient.GetAsync(roleUrl, cancellationToken);
            if (!roleResponse.IsSuccessStatusCode)
            {
                logger.LogWarning("Role {RoleName} not found", roleName);
                return false;
            }

            var roleContent = await roleResponse.Content.ReadAsStringAsync(cancellationToken);
            var role = JsonConvert.DeserializeObject<KeycloakRole>(roleContent);
            
            if (role == null)
            {
                logger.LogWarning("Failed to deserialize role {RoleName}", roleName);
                return false;
            }

            // Remove role from user
            var roles = new[] { role };
            var json = JsonConvert.SerializeObject(roles);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var removeUrl = $"{_options.AdminUrl}users/{userId}/role-mappings/realm";
            var request = new HttpRequestMessage(HttpMethod.Delete, removeUrl)
            {
                Content = content
            };
            
            var removeResponse = await httpClient.SendAsync(request, cancellationToken);
            
            if (removeResponse.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully removed role {RoleName} from user {UserId}", roleName, userId);
                return true;
            }
            
            logger.LogWarning("Failed to remove role {RoleName} from user {UserId}. Status: {StatusCode}", 
                roleName, userId, removeResponse.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error removing role {RoleName} from user {UserId}", roleName, userId);
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);
            
            var url = $"{_options.AdminUrl}users/{userId}/role-mappings/realm";
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to get roles for user {UserId}. Status: {StatusCode}", userId, response.StatusCode);
                return [];
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var roles = JsonConvert.DeserializeObject<List<KeycloakRole>>(content);
            
            return roles?.Select(r => r.Name) ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting roles for user {UserId}", userId);
            return [];
        }
    }

    public async Task<bool> CreateRoleAsync(string roleName, string description, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);

            var role = new
            {
                name = roleName,
                description
            };

            var json = JsonConvert.SerializeObject(role);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_options.AdminUrl}roles";
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.PostAsync(url, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully created role {RoleName}", roleName);
                return true;
            }

            logger.LogWarning("Failed to create role {RoleName}. Status: {StatusCode}", roleName, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating role {RoleName}", roleName);
            return false;
        }
    }

    public async Task<bool> DeleteRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);

            var url = $"{_options.AdminUrl}roles/{roleName}";
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.DeleteAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully deleted role {RoleName}", roleName);
                return true;
            }

            logger.LogWarning("Failed to delete role {RoleName}. Status: {StatusCode}", roleName, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting role {RoleName}", roleName);
            return false;
        }
    }

    public async Task<IEnumerable<string>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);

            var url = $"{_options.AdminUrl}roles";
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to get all roles. Status: {StatusCode}", response.StatusCode);
                return [];
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var roles = JsonConvert.DeserializeObject<List<KeycloakRole>>(content);

            return roles?.Select(r => r.Name) ?? [];
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting all roles");
            return [];
        }
    }

    public async Task<bool> SetUserPasswordAsync(string userId, string password, bool temporary = false, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);

            var credential = new
            {
                type = "password",
                value = password,
                temporary
            };

            var json = JsonConvert.SerializeObject(credential);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_options.AdminUrl}users/{userId}/reset-password";
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.PutAsync(url, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully set password for user {UserId}", userId);
                return true;
            }

            logger.LogWarning("Failed to set password for user {UserId}. Status: {StatusCode}", userId, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error setting password for user {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> EnableUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await SetUserEnabledStatusAsync(userId, true, cancellationToken);
    }

    public async Task<bool> DisableUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await SetUserEnabledStatusAsync(userId, false, cancellationToken);
    }

    private async Task<bool> SetUserEnabledStatusAsync(string userId, bool enabled, CancellationToken cancellationToken)
    {
        try
        {
            await EnsureAdminTokenAsync(cancellationToken);

            var user = new
            {
                enabled
            };

            var json = JsonConvert.SerializeObject(user);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_options.AdminUrl}users/{userId}";
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _adminToken);

            var response = await httpClient.PutAsync(url, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully {Action} user {UserId}", enabled ? "enabled" : "disabled", userId);
                return true;
            }

            logger.LogWarning("Failed to {Action} user {UserId}. Status: {StatusCode}",
                enabled ? "enable" : "disable", userId, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error {Action} user {UserId}", enabled ? "enabling" : "disabling", userId);
            return false;
        }
    }

    private async Task EnsureAdminTokenAsync(CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(_adminToken) && DateTime.UtcNow < _tokenExpiry)
        {
            return;
        }

        var tokenRequest = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", _options.AdminClientId),
            new("client_secret", _options.AdminClientSecret)
        };

        var content = new FormUrlEncodedContent(tokenRequest);
        var response = await httpClient.PostAsync(_options.TokenUrl, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Failed to get admin token. Status: {response.StatusCode}");
        }

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        var tokenResponse = JsonConvert.DeserializeObject<KeycloakTokenResponse>(responseContent);

        if (tokenResponse == null)
        {
            throw new InvalidOperationException("Failed to deserialize token response");
        }

        _adminToken = tokenResponse.AccessToken;
        _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60); // Refresh 1 minute before expiry
    }
}
