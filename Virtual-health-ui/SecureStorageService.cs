using System.Dynamic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using SkillBridge.UI;

public class SecureStorageService
{
    private readonly IJSRuntime _jsRuntime;

    public SecureStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task SetItemAsync(string key, string value)
    {
        await _jsRuntime.InvokeVoidAsync("secureStorage.setItem", key, value);
    }

    public async Task<string> GetItemAsync(string key)
    {
        return await _jsRuntime.InvokeAsync<string>("secureStorage.getItem", key);
    }

    public async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("secureStorage.removeItem", key);
    }

    public async Task<string> GetUserIdFromAccessTokenAsync()
    {
        string accessToken = await _jsRuntime.InvokeAsync<string>("secureStorage.getItem", "access_token");
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(accessToken) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
        var userId = jsonToken?.Claims.First(claim => claim.Type == "sub").Value;
        return userId;
    }


    public async Task<string> GetUserIdAsync()
    {
        return await _jsRuntime.InvokeAsync<string>("secureStorage.getItem", "user_id");
    }

    public async Task<string> GetSecureUserIdAsync(string userProfileJson)
    {
        
        var userSecureProfile= JsonSerializer.Deserialize<UserSecureProfile>(userProfileJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return userSecureProfile.Name;

    }

    public async Task<UserProfile> GetUserProfileAsync(string userProfileJson)
    {

        var userProfile = JsonSerializer.Deserialize<UserProfile>(userProfileJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return userProfile;

    }

    public async Task<UserProfile> GetSecureUserProfileAsync(string userProfileJson)
    {

        var userSecureProfile = JsonSerializer.Deserialize<UserProfile>(userProfileJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return userSecureProfile;

    }
}
