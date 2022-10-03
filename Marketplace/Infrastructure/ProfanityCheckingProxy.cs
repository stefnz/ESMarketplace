using Microsoft.AspNetCore.WebUtilities;

namespace Marketplace.Infrastructure; 

public class ProfanityCheckingProxy: IDisposable {
    private readonly HttpClient httpClient;
    private bool isDisposed = false;

    public ProfanityCheckingProxy() : this(new HttpClient()) { }
        
    public ProfanityCheckingProxy(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task<bool> CheckForProfanity(string text)
    {
        if (isDisposed) {
            throw new ObjectDisposedException($"The {GetType().FullName} has already been disposed.");
        }

        var result = await httpClient.GetAsync(
            QueryHelpers.AddQueryString("https://www.purgomalum.com/service/containsprofanity", "text", text));
            
        var value = await result.Content.ReadAsStringAsync();
        return bool.Parse(value);
    }

    public void Dispose() {
        if (isDisposed) {
            return;
        }

        if (httpClient != null) {
            httpClient.Dispose();
        }
        isDisposed = true;
    }
}