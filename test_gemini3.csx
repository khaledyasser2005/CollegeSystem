using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

var _apiKey = "AQ.Ab8RN6L6DACXLfC_xubhsecZWnCNjR8WmpbjKWpdmDhk7pC-gA";
var _model = "gemini-3.5-flash";
var _http = new HttpClient();

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
    },
    safetySettings = new[]
    {
        new { category = "HARM_CATEGORY_HARASSMENT",        threshold = "BLOCK_MEDIUM_AND_ABOVE" },
        new { category = "HARM_CATEGORY_HATE_SPEECH",       threshold = "BLOCK_MEDIUM_AND_ABOVE" },
        new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_MEDIUM_AND_ABOVE" },
        new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_MEDIUM_AND_ABOVE" }
    }
};

var url  = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";
var json = JsonSerializer.Serialize(requestBody);
var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
var response = await _http.PostAsync(url, httpContent);
var body = await response.Content.ReadAsStringAsync();
Console.WriteLine($"Status: {response.StatusCode}");
Console.WriteLine(body);
