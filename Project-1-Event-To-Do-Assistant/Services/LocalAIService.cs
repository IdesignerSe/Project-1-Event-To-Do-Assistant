using System.Net.Http.Json;
using EventTodoAssistant.Models;

namespace EventTodoAssistant.Services
{
    public class LocalAIService : IAIService
    {
        private readonly HttpClient _http = new HttpClient();

        public async Task<List<string>> GenerateSuggestionsAsync(string eventName)
        {
            var request = new
            {
                model = "llama3",
                prompt = $"Give me 5 task suggestions for: {eventName}",
                stream = false
            };

            var response = await _http.PostAsJsonAsync("http://localhost:11434/api/generate", request);

            // Safely read JSON
            var json = await response.Content.ReadFromJsonAsync<dynamic>();

            // Safely extract "response" field
            string text = json?.response ?? "";

            // If empty, return fallback
            if (string.IsNullOrWhiteSpace(text))
                return new List<string> { "No suggestions returned from Local AI." };

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
