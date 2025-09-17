using System.Net.Http;
using System.Net.Http.Json;

namespace LybraryManagement.Services;

public static class ApiClient
{
    public static async Task<T?> GetAsync<T>(IHttpClientFactory factory, string url, CancellationToken ct = default)
        => await factory.CreateClient("Api").GetFromJsonAsync<T>(url, ct);

    public static async Task<HttpResponseMessage> PostAsync<T>(IHttpClientFactory factory, string url, T payload, CancellationToken ct = default)
        => await factory.CreateClient("Api").PostAsJsonAsync(url, payload, ct);

    public static async Task<HttpResponseMessage> PutAsync<T>(IHttpClientFactory factory, string url, T payload, CancellationToken ct = default)
        => await factory.CreateClient("Api").PutAsJsonAsync(url, payload, ct);

    public static async Task<HttpResponseMessage> DeleteAsync(IHttpClientFactory factory, string url, CancellationToken ct = default)
        => await factory.CreateClient("Api").DeleteAsync(url, ct);
}
