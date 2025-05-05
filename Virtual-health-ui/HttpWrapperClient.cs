using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;

public class InternalHttpClient
{
    public HttpClient Client { get; set; }

    public InternalHttpClient(HttpClient client)
    {
        Client = client;
    }
}

public class MedplumWrapperApiHttpClient
{
    public HttpClient Client { get; set; }

    public MedplumWrapperApiHttpClient(HttpClient client)
    {
        Client = client;
    }
}
