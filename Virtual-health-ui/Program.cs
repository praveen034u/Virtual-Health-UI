using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Text.Json;
using VirtualHealth.UI;
using VirtualHealth.UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(configuration);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<SecureStorageService>();
builder.Services.AddScoped<MedPlumAPIService>();

// Register multiple HttpClient instances manually
builder.Services.AddScoped<MedplumWrapperApiHttpClient>(sp => new MedplumWrapperApiHttpClient(
    new HttpClient
    {
        BaseAddress = new Uri("https://virtual-health-api-service-368018650904.us-central1.run.app")
    }
));

// Default HttpClient for the application
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// ✅ Fix: Avoid unsupported reflection for nullability metadata in WASM
builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.TypeInfoResolver = null;
});

builder.Services.AddOidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
});

await builder.Build().RunAsync();