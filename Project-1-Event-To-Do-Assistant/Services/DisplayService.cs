using EventTodoAssistant.Models;

namespace EventTodoAssistant.Services
{
    public class DisplayService
    {
        public void ShowSplitScreen(List<TaskItem> tasks, List<string> suggestions)
        {
            Console.Clear();
            Console.WriteLine("YOUR TASKS".PadRight(40) + "AI SUGGESTIONS");
            Console.WriteLine(new string('-', 80));

            int max = Math.Max(tasks.Count, suggestions.Count);

            for (int i = 0; i < max; i++)
            {
                string left = i < tasks.Count
                    ? $"{i}. {tasks[i].Title} ({tasks[i].DueDate.ToShortDateString()})"
                    : "";

                string right = i < suggestions.Count
                    ? suggestions[i]
                    : "";

                Console.WriteLine(left.PadRight(40) + right);
            }
        }
    }
}
