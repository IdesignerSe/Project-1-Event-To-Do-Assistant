using System.Net.Http.Json;
using EventTodoAssistant.Models;

namespace EventTodoAssistant.Services
{
    public class CloudAIService : IAIService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public CloudAIService(string apiKey)
        {
            _http = new HttpClient();
            _apiKey = apiKey;
        }

        public async Task<List<string>> GenerateSuggestionsAsync(string eventName)
        {
            var request = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "user", content = $"Give me 5 task suggestions for: {eventName}" }
                }
            };

            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _http.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);
            var json = await response.Content.ReadFromJsonAsync<dynamic>();

            string text = json.choices[0].message.content;

            return text.Split('\n')
                       .Where(x => !string.IsNullOrWhiteSpace(x))
                       .Select(x => x.TrimStart('-', ' '))
                       .ToList();
        }

        public Task<List<TaskItem>> GenerateEventTasksAsync(string eventName, DateTime eventDate)
        {
            throw new NotImplementedException();
        }
    }
}
