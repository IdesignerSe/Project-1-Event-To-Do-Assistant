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

        // ⭐ NEW METHOD — Show Task List with Colors
        public void ShowTaskList(List<TaskItem> tasks)
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("────────────────────────────────────────");
            Console.WriteLine();

            Console.WriteLine("=== TASK LIST ===\n");

            Console.WriteLine("Idx  ID   Title                 Due Date    Project            Pri   Status");
            Console.WriteLine("---------------------------------------------------------------------------");

            int idx = 1;

            foreach (var t in tasks)
            {
                string id = t.ShortId;
                string title = Truncate(t.Title, 20);
                string project = Truncate(t.Project, 15);
                string pri = Truncate(t.Priority, 4);

                // ⭐ STATUS COLOR
                if (t.Status == "OVERDUE")
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (t.Status == "Done")
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine($"{idx,-4}{id,-4}{title,-22}{t.DueDate:yyyy-MM-dd}  {project,-16}{pri,-5}{t.Status}");
                Console.ResetColor();

                // Tags
                if (t.Tags != null && t.Tags.Count > 0)
                    Console.WriteLine($"     Tags: {string.Join(", ", t.Tags)}");

                // Description
                if (!string.IsNullOrWhiteSpace(t.Description))
                    Console.WriteLine($"     Desc: {Truncate(t.Description, 40)}");

                Console.WriteLine(); // spacing
                idx++;
            }

            Console.WriteLine("────────────────────────────────────────");
            Console.WriteLine();
            Console.WriteLine("Press any key to return to Main Menu...");
        }

        private string Truncate(string text, int max)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= max ? text : text.Substring(0, max - 3) + "...";
        }
    }
}
