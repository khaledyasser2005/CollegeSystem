using System.Text;
using CollegeSystem.Models;
using Google.GenAI;
using Google.GenAI.Types;

namespace CollegeSystem.Services
{
    /// <summary>
    /// Calls the Google Gemini API to generate chat responses using the official Google Gen AI .NET SDK.
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
        private readonly Client _client;
        private readonly string _model;
        private readonly ILogger<GeminiChatService> _logger;

        public GeminiChatService(
            IConfiguration configuration,
            ILogger<GeminiChatService> logger)
        {
            _logger = logger;

            bool useAdc = configuration.GetValue<bool>("Gemini:UseAdc", false);
            var apiKey = configuration["Gemini:ApiKey"];

            if (useAdc)
            {
                // Initializes using OAuth 2.0 / Application Default Credentials (ADC).
                // This resolves the ACCESS_TOKEN_TYPE_UNSUPPORTED restriction.
                _client = new Client();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(apiKey) ||
                    apiKey.Equals("YOUR_GEMINI_API_KEY_HERE", StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "Gemini:ApiKey is not configured. Please set a real API key, or set 'Gemini:UseAdc': true to use Application Default Credentials.");
                }

                _client = new Client(apiKey: apiKey);
            }

            _model  = configuration["Gemini:Model"] ?? "gemini-2.5-flash";
        }

        public async Task<string> GetResponseAsync(
            string systemPrompt,
            IEnumerable<ChatMessage> history,
            string userMessage)
        {
            var contents = new List<Content>();

            // Map history to SDK Content models
            foreach (var msg in history)
            {
                string geminiRole = msg.Role == "assistant" ? "model" : "user";
                contents.Add(new Content
                {
                    Role = geminiRole,
                    Parts = new List<Part> { new Part { Text = msg.Content } }
                });
            }

            // Append current message
            contents.Add(new Content
            {
                Role = "user",
                Parts = new List<Part> { new Part { Text = userMessage } }
            });

            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content 
                { 
                    Parts = new List<Part> { new Part { Text = systemPrompt } } 
                },
                Temperature = 0.7f,
                MaxOutputTokens = 1024,
                TopP = 0.9f
            };

            try
            {
                var response = await _client.Models.GenerateContentAsync(
                    model: _model,
                    contents: contents,
                    config: config
                );

                return response.Text ?? "I'm sorry, I couldn't generate a response.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Google Gen AI SDK Error");
                // Rethrow the exception to be caught and displayed by the ChatController
                throw;
            }
        }
    }
}
