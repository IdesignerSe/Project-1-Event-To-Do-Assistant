using EventTodoAssistant.Models;
using EventTodoAssistant.Services;
using EventTodoAssistant.UI;

class Program
{
    static void Main(string[] args)
    {
        var taskManager = new TaskManager();
        var aiService = new AISuggestionService();
        var displayService = new DisplayService();
        var menu = new MenuUI();

        // AI service (mock for now — safe, no internet needed)
        //IAIService aiService = new AISuggestionServiceAdapter(new AISuggestionService());

        // Later you can switch to real AI:
        // aiService = new CloudAIService("YOUR_API_KEY");
        // aiService = new LocalAIService();

        string filePath = "tasks.json";
        taskManager.LoadFromJson(filePath);

        bool running = true;

        while (running)
        {
            Console.Clear();
            menu.ShowMainMenu();

            string choice = ReadNonEmpty("Choose an option number: ");

            switch (choice)
            {
                case "0":
                    Console.Clear();
                    Console.WriteLine("=== DASHBOARD SUMMARY ===");

                    var summary = taskManager.GetSummary();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Overdue Tasks: {summary.overdue}");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Due Today: {summary.dueToday}");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Due This Week: {summary.dueWeek}");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Completed: {summary.completed}");

                    Console.ResetColor();
                    Console.WriteLine($"Total Tasks: {summary.total}");

                    Console.WriteLine("\nPress any key to return to MAIN MENU.");
                    Console.ReadKey();
                    break;

                case "1":
                    Console.Clear();
                    Console.WriteLine("=== TASK LIST ===");

                    Console.WriteLine(
                        Pad("Idx", 5) +
                        Pad("ID", 4) +
                        Pad("Title", 20) +
                        Pad("Due Date", 12) +
                        Pad("Project", 15) +
                        Pad("Priority", 10) +
                        Pad("Tags", 20) +
                        Pad("Description", 25) +
                        "Status"
                    );
                    Console.ResetColor();

                    Console.WriteLine(new string('-', 130));

                    for (int i = 0; i < taskManager.Tasks.Count; i++)
                    {
                        var task = taskManager.Tasks[i];
                        string shortId = task.Id.ToString().Substring(0, 3);

                        string status = task.IsCompleted ? "Done" :
                                        task.IsOverdue ? "OVERDUE" :
                                        "Pending";

                        Console.WriteLine(
                            Pad((i + 1).ToString(), 5) +
                            Pad(task.Id.ToString(), 4) +
                            Pad(task.Title, 20) +
                            Pad(task.DueDate.ToString("yyyy-MM-dd"), 12) +
                            Pad(task.Project, 15) +
                            Pad(task.Priority, 10) +
                            Pad(string.Join(", ", task.Tags), 20) +
                            Pad(task.Description, 25) +
                            status
                        );

                        Console.ResetColor();
                    }

                    Console.ReadKey();
                    break;

                case "2":
                    Console.Clear();
                    string title = ReadNonEmpty("Task title: ");
                    DateTime date = ReadDate("Due date (yyyy-mm-dd): ");
                    string project = ReadNonEmpty("Project: ");
                    string priority = ReadPriority("Priority (low/medium/high): ");

                    string tagsInput = ReadNonEmpty("Tags (comma-separated): ");
                    List<string> tags = tagsInput.Split(',').Select(t => t.Trim()).ToList();

                    string description = ReadDescription("Description (optional): ");

                    taskManager.AddTask(new TaskItem
                    {
                        Title = title,
                        DueDate = date,
                        Project = project,
                        Priority = priority,
                        Tags = tags,
                        Description = description,
                        IsCompleted = false
                    });

                    Console.WriteLine("Task added!");
                    Console.ReadKey();
                    break;

                case "3":
                    Console.Clear();
                    int editIndex = ReadInt("Enter task index to edit: ");

                    var taskToEdit = taskManager.GetTask(editIndex);
                    if (taskToEdit != null)
                    {
                        taskToEdit.UpdateTitle(ReadNonEmpty("New title: "));
                        taskToEdit.UpdateDate(ReadDate("New date (yyyy-mm-dd): "));
                        taskToEdit.UpdateProject(ReadNonEmpty("New project: "));

                        string newTagsInput = ReadNonEmpty("New tags (comma-separated): ");
                        taskToEdit.UpdateTags(newTagsInput.Split(',').Select(t => t.Trim()).ToList());

                        taskToEdit.UpdateDescription(ReadDescription("New description: "));

                        Console.WriteLine("Task updated!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid index.");
                    }
                    Console.ReadKey();
                    break;

                case "4":
                    Console.Clear();
                    int doneIndex = ReadInt("Enter task index to mark as done: ");

                    var taskToMark = taskManager.GetTask(doneIndex);
                    if (taskToMark != null)
                    {
                        taskToMark.MarkCompleted();
                        Console.WriteLine("Task marked as done!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid index.");
                    }
                    Console.ReadKey();
                    break;

                case "5":
                    Console.Clear();
                    int removeIndex = ReadInt("Enter task index to remove: ");

                    taskManager.RemoveTask(removeIndex);
                    Console.WriteLine("Task removed!");
                    Console.ReadKey();
                    break;

                case "6":
                    taskManager.SortByDate();
                    Console.WriteLine("Sorted by date!");
                    Console.ReadKey();
                    break;

                case "7":
                    taskManager.SortByProject();
                    Console.WriteLine("Sorted by project!");
                    Console.ReadKey();
                    break;

                case "8":
                    taskManager.SaveToJson(filePath);
                    running = false;
                    break;


                case "9":
    Console.Clear();
    Console.WriteLine("=== AI EVENT SUGGESTIONS ===\n");

    Console.Write("Enter event name: ");
    string eventName = Console.ReadLine()?.Trim();

    Console.WriteLine("\nGenerating suggestions...\n");

    // Get suggestions from AI service
    var suggestions = aiService.GetMockSuggestions(eventName);

    // Store suggestions temporarily in TaskManager
    taskManager.AISuggestions.Clear();
    foreach (var s in suggestions)
    {
        taskManager.AISuggestions.Add(new AISuggestionItem
        {
            EventName = eventName,
            Suggestion = s,
            IsSaved = false
        });
    }

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"Event: {eventName}\n");
    Console.ResetColor();

    Console.WriteLine("Suggested Tasks:");
    Console.WriteLine("----------------");

    for (int i = 0; i < taskManager.AISuggestions.Count; i++)
    {
        var sug = taskManager.AISuggestions[i];
        Console.WriteLine($"{i}. {sug.Suggestion}");
    }

    Console.WriteLine("\nOptions:");
    Console.WriteLine("1. Save a suggestion");
    Console.WriteLine("2. Delete a suggestion");
    Console.WriteLine("3. Save ALL suggestions");
    Console.WriteLine("4. Return to Main Menu");

    Console.Write("\nChoose an option: ");
    string aiChoice = Console.ReadLine()?.Trim();

    if (aiChoice == "1")
    {
        Console.Write("Enter suggestion index to save: ");
        int idx = int.Parse(Console.ReadLine());

        var sug = taskManager.AISuggestions[idx];

        taskManager.AddTask(new TaskItem
        {
            Title = sug.Suggestion,
            Project = sug.EventName,
            Priority = "Medium",
            Tags = new List<string> { "AI", "Suggestion" },
            Description = $"AI-generated suggestion for event: {sug.EventName}",
            DueDate = DateTime.Now.AddDays(7)
        });

        sug.IsSaved = true;

        Console.WriteLine("\nSuggestion saved as a task!");
        Console.WriteLine("\nAfter saving, Go to Main Menu Option 8 to fully save all tasks to file.");

        Console.ReadKey();
    }
    else if (aiChoice == "2")
    {
        Console.Write("Enter suggestion index to delete: ");
        int idx = int.Parse(Console.ReadLine());

        taskManager.AISuggestions.RemoveAt(idx);

        Console.WriteLine("\nSuggestion removed.");
        Console.ReadKey();
    }
    else if (aiChoice == "3")
    {
        foreach (var sug in taskManager.AISuggestions)
        {
            taskManager.AddTask(new TaskItem
            {
                Title = sug.Suggestion,
                Project = sug.EventName,
                Priority = "Medium",
                Tags = new List<string> { "AI", "Suggestion" },
                Description = $"AI-generated suggestion for event: {sug.EventName}",
                DueDate = DateTime.Now.AddDays(7)
            });

            sug.IsSaved = true;
        }

        Console.WriteLine("\nAll suggestions saved!");
        Console.ReadKey();
    }

    break;

case "10":
    Console.Clear();
    Console.WriteLine("=== SEARCH & FILTER ===");
    Console.WriteLine("1. Search by Project");
    Console.WriteLine("2. Filter by Date");
    Console.WriteLine("3. Filter by Status");
    Console.WriteLine("4. Search by Tag");
    Console.WriteLine("5. Back to Main Menu");

    string sfaiChoice = ReadNonEmpty("Choose an option: ");

    if (sfaiChoice == "1")
    {
        string projectName = ReadNonEmpty("Enter project name: ");
        var results = taskManager.SearchByProject(projectName);

        Console.WriteLine("=== RESULTS ===");
        foreach (var t in results)
            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate:yyyy-MM-dd} - {t.Priority}");

        Console.ReadKey();
    }
    else if (sfaiChoice == "2")   // ⭐ FIXED HERE
    {
        DateTime filterDate = ReadDate("Enter date (yyyy-mm-dd): ");
        Console.Write("Filter (before/after/on): ");
        string mode = Console.ReadLine() ?? "on";

        var results = taskManager.FilterByDate(filterDate, mode);

        Console.WriteLine("=== RESULTS ===");
        foreach (var t in results)
            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate:yyyy-MM-dd} - {t.Priority}");

        Console.ReadKey();
    }
    else if (sfaiChoice == "3")
    {
        Console.Write("Show completed? (yes/no): ");
        string ans = Console.ReadLine()?.ToLower() ?? "no";

        bool completed = ans == "yes";

        var results = taskManager.FilterByStatus(completed);

        Console.WriteLine("=== RESULTS ===");
        foreach (var t in results)
            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate:yyyy-MM-dd} - {t.Priority}");

        Console.ReadKey();
    }
    else if (sfaiChoice == "4")
    {
        string tagSearch = ReadNonEmpty("Enter tag to search: ");

        var results = taskManager.Tasks
            .Where(t => t.Tags.Any(tag =>
                tag.Contains(tagSearch, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        Console.WriteLine("=== RESULTS ===");
        foreach (var t in results)
            Console.WriteLine($"{t.Title} - {string.Join(", ", t.Tags)} - {t.DueDate:yyyy-MM-dd} - {t.Priority}");

        Console.ReadKey();
    }

    break;


                case "11":
                    taskManager.SortByPriority();
                    Console.WriteLine("Tasks sorted by priority!");
                    Console.ReadKey();
                    break;

                case "12":
                    string exportPath = "Exports/tasks.csv";
                    taskManager.ExportToCsv(exportPath);
                    Console.WriteLine($"Tasks exported to {exportPath}");
                    Console.ReadKey();
                    break;

                case "13":
                    Console.Clear();
                    Console.WriteLine("=== VIEW TASK DETAILS ===");

                    if (taskManager.Tasks.Count == 0)
                    {
                        Console.WriteLine("No tasks available.");
                        Console.ReadKey();
                        break;
                    }

                    for (int i = 0; i < taskManager.Tasks.Count; i++)
                        Console.WriteLine($"{i + 1}. {taskManager.Tasks[i].Title}");

                    int index = ReadInt("Select task number: ") - 1;

                    if (index < 0 || index >= taskManager.Tasks.Count)
                    {
                        Console.WriteLine("Invalid selection.");
                        Console.ReadKey();
                        break;
                    }

                    var selectedTask = taskManager.Tasks[index];

                    Console.Clear();
                    Console.WriteLine("=== TASK DETAILS ===\n");

                    Console.WriteLine($"Title:       {selectedTask.Title}");
                    Console.WriteLine($"Due Date:    {selectedTask.DueDate:yyyy-MM-dd}");
                    Console.WriteLine($"Project:     {selectedTask.Project}");
                    Console.WriteLine($"Priority:    {selectedTask.Priority}");
                    Console.WriteLine($"Tags:        {string.Join(", ", selectedTask.Tags)}");
                    Console.WriteLine($"Completed:   {(selectedTask.IsCompleted ? "Yes" : "No")}");
                    Console.WriteLine($"Overdue:     {(selectedTask.IsOverdue ? "Yes" : "No")}");

                    Console.WriteLine("\nDescription:");
                    Console.WriteLine(string.IsNullOrWhiteSpace(selectedTask.Description)
                        ? "(No description)"
                        : selectedTask.Description);

                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    break;

                case "14":
                    taskManager.SortByCategory();
                    Console.WriteLine("Tasks sorted by category!");
                    Console.ReadKey();
                    Console.Clear();
                    goto case "1";

                default:
                    Console.WriteLine("Invalid choice.");
                    Console.ReadKey();
                    break;

                case "15":
    Console.Clear();
    Console.WriteLine("=== TAG STATISTICS ===\n");

    var stats = taskManager.GetTagStatistics();

    if (stats.Count == 0)
    {
        Console.WriteLine("No tags found.");
        Console.ReadKey();
        break;
    }

    foreach (var kvp in stats)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value} task(s)");
    }

    Console.WriteLine("\nPress any key to return...");
    Console.ReadKey();
    break;

                case "16":
    Console.Clear();
    Console.WriteLine("=== SMART EVENT ASSISTANT ===\n");

    string evName = ReadNonEmpty("Event name: ");
    DateTime evDate = ReadDate("Event date (yyyy-mm-dd): ");

    var generated = aiService.GenerateEventTasks(evName, evDate);

    Console.WriteLine("\nGenerated tasks:\n");
    foreach (var t in generated)
        Console.WriteLine($"- {t.Title} (Due: {t.DueDate:yyyy-MM-dd})");

    Console.Write("\nAdd these tasks to your list? (yes/no): ");
    string ans2 = Console.ReadLine()?.ToLower() ?? "no";

    if (ans2 == "yes")
    {
        foreach (var t in generated)
            taskManager.AddTask(t);

        Console.WriteLine("Tasks added!");
    }
    else
    {
        Console.WriteLine("Cancelled.");
    }

    Console.ReadKey();
    break;

                case "17":
    Console.Clear();
    Console.WriteLine("=== TASK HISTORY ===\n");

    string historyPath = "task_history.txt";

    if (!File.Exists(historyPath))
    {
        Console.WriteLine("No history available yet.");
    }
    else
    {
        var lines = File.ReadAllLines(historyPath);

        if (lines.Length == 0)
        {
            Console.WriteLine("History file is empty.");
        }
        else
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }

    Console.WriteLine("\nPress any key to return to Main Menu...");
    Console.ReadKey();
    break;
       
            }
        }
    }


    static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int number))
                return number;

            Console.WriteLine("Invalid number. Try again.");
        }
    }

    static DateTime ReadDate(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (DateTime.TryParse(input, out DateTime date))
                return date;

            Console.WriteLine("Invalid date. Use format yyyy-mm-dd.");
        }
    }

    static string ReadNonEmpty(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.WriteLine("Input cannot be empty.");
        }
    }

    static string ReadPriority(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine()?.ToLower();

            if (input == "low" || input == "medium" || input == "high")
                return char.ToUpper(input[0]) + input.Substring(1);

            Console.WriteLine("Invalid priority. Use: low, medium, or high.");
        }
    }

    static string Pad(string text, int width)
    {
        if (text.Length >= width)
            return text.Substring(0, width - 1) + " ";

        return text.PadRight(width);
    }

    static string ReadDescription(string message)
    {
        Console.Write(message);
        string? input = Console.ReadLine();
        return input?.Trim() ?? "";
    }
}
