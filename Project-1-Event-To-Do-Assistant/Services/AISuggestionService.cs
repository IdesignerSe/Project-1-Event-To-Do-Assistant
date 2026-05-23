using EventTodoAssistant.Models;

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

        public List<TaskItem> GenerateEventTasks(string eventName, DateTime eventDate)
        {
            var tasks = new List<TaskItem>();

            var suggestions = new List<(string title, int daysBefore, string[] tags)>
            {
                ("Invite guests", 7, new[] { "People", "Planning" }),
                ("Plan food menu", 5, new[] { "Food", "Planning" }),
                ("Buy groceries", 2, new[] { "Shopping", "Food" }),
                ("Check weather forecast", 3, new[] { "Weather" }),
                ("Prepare decorations", 4, new[] { "Decor", "Planning" }),
                ("Set up event area", 1, new[] { "Setup" })
            };

            foreach (var s in suggestions)
            {
                tasks.Add(new TaskItem
                {
                    Title = s.title,
                    DueDate = eventDate.AddDays(-s.daysBefore),
                    Project = eventName,
                    Priority = "Medium",
                    Tags = s.tags.ToList(),
                    Description = ""
                });
            }

            return tasks;
        }
    }
}
