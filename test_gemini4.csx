using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

var _apiKey = "AQ.Ab8RN6L6DACXLfC_xubhsecZWnCNjR8WmpbjKWpdmDhk7pC-gA";
var _model = "gemini-3.5-flash";
var _http = new HttpClient();
_http.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

var contents = new[]
{
    new { role = "user", parts = new[] { new { text = "Hello" } } }
};

var requestBody = new
{
    contents,
    generationConfig = new
    {
        temperature = 0.7,
        maxOutputTokens = 1024,
        topP = 0.9
    }
};

var url  = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent";
var json = JsonSerializer.Serialize(requestBody);
var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
var response = await _http.PostAsync(url, httpContent);
var body = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Status: {response.StatusCode}");
Console.WriteLine(body);
