//program.cs cote client

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LybraryManagement;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Default HttpClient for static files and loading appsettings.json
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Load client appsettings.json to get API base URL
var http = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
using var response = await http.GetAsync("appsettings.json");
if (response.IsSuccessStatusCode)
{
    var stream = await response.Content.ReadAsStreamAsync();
    builder.Configuration.AddJsonStream(stream);
}

var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:7000/";
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri(apiBase));

// MudBlazor services
builder.Services.AddMudServices();

await builder.Build().RunAsync();
