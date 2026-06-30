using System.Text;
using System.Text.Json;
using CollegeSystem.Models;

namespace CollegeSystem.Services
{
    /// <summary>
    /// Calls the Google Gemini REST API to generate chat responses.
    /// Configured via the "Gemini" section in appsettings.json.
    /// </summary>
    public interface IGeminiChatService
    {
        Task<string> GetResponseAsync(
            string systemPrompt,
            IEnumerable<ChatMessage> history,
            string userMessage);
    }

    public class GeminiChatService : IGeminiChatService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly string _model;
        private readonly ILogger<GeminiChatService> _logger;

        public GeminiChatService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<GeminiChatService> logger)
        {
            _http   = httpClientFactory.CreateClient("Gemini");
            _logger = logger;

            var apiKey = configuration["Gemini:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey) ||
                apiKey.Equals("YOUR_GEMINI_API_KEY_HERE", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    "Gemini:ApiKey is not configured. Please set a real API key in appsettings.json " +
                    "(or use User Secrets / an environment variable) under the 'Gemini' section.");
            }

            _apiKey = apiKey;
            _model  = configuration["Gemini:Model"] ?? "gemini-1.5-flash";
        }

        public async Task<string> GetResponseAsync(
            string systemPrompt,
            IEnumerable<ChatMessage> history,
            string userMessage)
        {
            var contents = new List<object>();

            // The Gemini API requires:
            // 1. roles must alternate between "user" and "model"
            // 2. the first turn must be "user"
            // Strategy: prepend the system prompt to the first user message,
            // then replay history turns, then add the current userMessage.

            var historyList = history.ToList();

            // Inject system prompt into the first user turn (or as a standalone first turn)
            bool systemInjected = false;
            foreach (var msg in historyList)
            {
                string geminiRole = msg.Role == "assistant" ? "model" : "user";

                if (!systemInjected && geminiRole == "user")
                {
                    // Prepend system context to the first user message
                    contents.Add(new
                    {
                        role = "user",
                        parts = new[] { new { text = $"[System context — always follow these instructions]\n{systemPrompt}\n\n[User message]\n{msg.Content}" } }
                    });
                    systemInjected = true;
                }
                else
                {
                    contents.Add(new
                    {
                        role = geminiRole,
                        parts = new[] { new { text = msg.Content } }
                    });
                }
            }

            // If there was no history (or no user turn in history), inject system prompt with current message
            if (!systemInjected)
            {
                contents.Add(new
                {
                    role = "user",
                    parts = new[] { new { text = $"[System context — always follow these instructions]\n{systemPrompt}\n\n[User message]\n{userMessage}" } }
                });
            }
            else
            {
                // Append the current user message as a new turn
                contents.Add(new
                {
                    role = "user",
                    parts = new[] { new { text = userMessage } }
                });
            }

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
            using var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("Gemini API error {StatusCode}: {Body}", (int)response.StatusCode, errorBody);
                throw new HttpRequestException(
                    $"Gemini API error {(int)response.StatusCode}");
            }

            using var stream = await response.Content.ReadAsStreamAsync();
            using var doc    = await JsonDocument.ParseAsync(stream);

            var root = doc.RootElement;

            // Check for prompt-level blocked content
            if (root.TryGetProperty("promptFeedback", out var feedback) &&
                feedback.TryGetProperty("blockReason", out _))
            {
                return "I'm sorry, that message was blocked by safety filters. Please rephrase your question.";
            }

            var candidates = root.GetProperty("candidates");
            if (candidates.GetArrayLength() == 0)
                return "I'm sorry, I couldn't generate a response. Please try again.";

            var firstCandidate = candidates[0];

            // Check for response-level safety block
            if (firstCandidate.TryGetProperty("finishReason", out var finishReason) &&
                finishReason.GetString() == "SAFETY")
            {
                return "I'm sorry, my response was blocked by safety filters. Please rephrase your question.";
            }

            var text = firstCandidate
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "I'm sorry, I couldn't generate a response.";
        }
    }
}
