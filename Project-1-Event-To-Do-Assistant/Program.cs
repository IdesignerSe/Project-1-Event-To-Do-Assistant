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

                    Console.WriteLine("\nPress any key to return...");
                    Console.ReadKey();
                    break;

                case "1":
                    Console.Clear();
                    Console.WriteLine("=== TASK LIST ===");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(
                        Pad("Title", 20) +
                        Pad("Due Date", 12) +
                        Pad("Project", 15) +
                        Pad("Priority", 10) +
                        Pad("Category", 15) +
                        Pad("Description", 25) +
                        "Status"
                    );
                    Console.ResetColor();

                    Console.WriteLine(new string('-', 120));

                    foreach (var task in taskManager.Tasks)
                    {
                        string status = task.IsCompleted
                            ? "Done"
                            : (task.IsOverdue ? "OVERDUE" : "Pending");

                        if (task.IsCompleted)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (task.IsOverdue)
                            Console.ForegroundColor = ConsoleColor.Red;
                        else
                            Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.WriteLine(
                            Pad(task.Title, 20) +
                            Pad(task.DueDate.ToString("yyyy-MM-dd"), 12) +
                            Pad(task.Project, 15) +
                            Pad(task.Priority, 10) +
                            Pad(task.Category, 15) +
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
                    string category = ReadCategory("Category: ");
                    string description = ReadDescription("Description (optional): ");

                    taskManager.AddTask(new TaskItem
                    {
                        Title = title,
                        DueDate = date,
                        Project = project,
                        Priority = priority,
                        Category = category,
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
                        taskToEdit.UpdateCategory(ReadCategory("New category: "));
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
                    string eventName = ReadNonEmpty("Enter event name: ");

                    var suggestions = aiService.GetMockSuggestions(eventName);
                    displayService.ShowSplitScreen(taskManager.Tasks, suggestions);

                    Console.ReadKey();
                    break;

                case "10":
                    Console.Clear();
                    Console.WriteLine("=== SEARCH & FILTER ===");
                    Console.WriteLine("1. Search by Project");
                    Console.WriteLine("2. Filter by Date");
                    Console.WriteLine("3. Filter by Status");
                    Console.WriteLine("4. Search by Category");
                    Console.WriteLine("5. Back to Main Menu");

                    string sfChoice = ReadNonEmpty("Choose an option: ");

                    if (sfChoice == "1")
                    {
                        string projectName = ReadNonEmpty("Enter project name: ");
                        var results = taskManager.SearchByProject(projectName);

                        Console.WriteLine("=== RESULTS ===");
                        foreach (var t in results)
                            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate:yyyy-MM-dd} - {t.Priority} - {(t.IsCompleted ? "Done" : "Pending")}");

                        Console.ReadKey();
                    }
                    else if (sfChoice == "2")
                    {
                        DateTime filterDate = ReadDate("Enter date (yyyy-mm-dd): ");
                        Console.Write("Filter (before/after/on): ");
                        string mode = Console.ReadLine() ?? "on";

                        var results = taskManager.FilterByDate(filterDate, mode);

                        Console.WriteLine("=== RESULTS ===");
                        foreach (var t in results)
                            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate:yyyy-MM-dd} - {t.Priority} - {(t.IsCompleted ? "Done" : "Pending")}");

                        Console.ReadKey();
                    }
                    else if (sfChoice == "3")
                    {
                        Console.Write("Show completed? (yes/no): ");
                        string ans = Console.ReadLine()?.ToLower() ?? "no";

                        bool completed = ans == "yes";

                        var results = taskManager.FilterByStatus(completed);

                        Console.WriteLine("=== RESULTS ===");
                        foreach (var t in results)
                            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate:yyyy-MM-dd} - {t.Priority} - {(t.IsCompleted ? "Done" : "Pending")}");

                        Console.ReadKey();
                    }
                    else if (sfChoice == "4")
                    {
                        string cat = ReadCategory("Enter category: ");
                        var results = taskManager.Tasks
                            .Where(t => t.Category.Contains(cat, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        Console.WriteLine("=== RESULTS ===");
                        foreach (var t in results)
                            Console.WriteLine($"{t.Title} - {t.Category} - {t.DueDate:yyyy-MM-dd} - {t.Priority}");

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
                    Console.WriteLine($"Category:    {selectedTask.Category}");
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
            }
        }
    }

    // ============================
    // Helper Methods (ALL STATIC)
    // ============================

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

    static string ReadCategory(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
                return input.Trim();

            Console.WriteLine("Category cannot be empty.");
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
