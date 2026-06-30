using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Google.GenAI;
using Google.GenAI.Types;
using System.Collections.Generic;

public class LoggingHandler : DelegatingHandler {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        Console.WriteLine("URL: " + request.RequestUri);
        Console.WriteLine("HEADERS:");
        foreach (var header in request.Headers) {
            Console.WriteLine(header.Key + ": " + string.Join(",", header.Value));
        }
        return new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StringContent("{}") };
    }
}

class Program {
    static async Task Main() {
        var opts = new ClientOptions {
            HttpClientFactory = () => new HttpClient(new LoggingHandler())
        };
        var client = new Client(apiKey: "AQ.test", clientOptions: opts);
        
        try {
            await client.Models.GenerateContentAsync("gemini-2.5-flash", "Hello");
        } catch { }
    }
}
