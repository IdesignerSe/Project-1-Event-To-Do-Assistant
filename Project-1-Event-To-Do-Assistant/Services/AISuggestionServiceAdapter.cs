using EventTodoAssistant.Models;

namespace EventTodoAssistant.Services
{
    // This adapter lets your old AISuggestionService work with the new IAIService interface
    public class AISuggestionServiceAdapter : IAIService
    {
        private readonly AISuggestionService _mock;

        public AISuggestionServiceAdapter(AISuggestionService mock)
        {
            _mock = mock;
        }

        public Task<List<string>> GenerateSuggestionsAsync(string eventName)
        {
            // Use your existing mock method
            var list = _mock.GetMockSuggestions(eventName);
            return Task.FromResult(list);
        }

        public Task<List<TaskItem>> GenerateEventTasksAsync(string eventName, DateTime eventDate)
        {
            // Use your existing mock method
            var list = _mock.GenerateEventTasks(eventName, eventDate);
            return Task.FromResult(list);
        }
    }
}
