namespace EventTodoAssistant.Services
{
    public class AISuggestionService
    {
        public List<string> GetMockSuggestions(string eventName)
        {
            return new List<string>
            {
                $"Prepare for {eventName}",
                $"Buy items for {eventName}",
                $"Invite friends for {eventName}",
                $"Plan food for {eventName}",
                $"Decorate for {eventName}"
            };
        }
    }
}
