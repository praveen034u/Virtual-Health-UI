using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

public class LocalApiHttpClient
{
    public HttpClient Client { get; }

    public LocalApiHttpClient(HttpClient client)
    {
        Client = client;
    }
}

public class ExternalApiHttpClient
{
    public HttpClient Client { get; }

    public ExternalApiHttpClient(HttpClient client)
    {
        Client = client;
    }
}
