using EventTodoAssistant.Models;

namespace EventTodoAssistant.Services
{
    public interface IAIService
    {
        Task<List<string>> GenerateSuggestionsAsync(string eventName);
        Task<List<TaskItem>> GenerateEventTasksAsync(string eventName, DateTime eventDate);
    }
}
