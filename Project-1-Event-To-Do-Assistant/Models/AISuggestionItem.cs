namespace EventTodoAssistant.Models
{
    public class AISuggestionItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventName { get; set; } = "";
        public string Suggestion { get; set; } = "";
        public bool IsSaved { get; set; } = false;
    }
}