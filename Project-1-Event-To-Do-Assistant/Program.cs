using EventTodoAssistant.Models;
using EventTodoAssistant.Services;
using EventTodoAssistant.UI;

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
        case "1":
            Console.Clear();
            Console.WriteLine("=== YOUR TASKS ===");
            foreach (var task in taskManager.Tasks)
            {
Console.WriteLine($"{task.Title} - {task.DueDate.ToShortDateString()} - {task.Project} - {task.Priority} - {(task.IsCompleted ? "Done" : "Pending")}");
            }
            Console.ReadKey();
            break;

        case "2":
            Console.Clear();
            string title = ReadNonEmpty("Task title: ");
            DateTime date = ReadDate("Due date (yyyy-mm-dd): ");
            string project = ReadNonEmpty("Project: ");
            string priority = ReadPriority("Priority (low/medium/high): ");

            taskManager.AddTask(new TaskItem
            {
                Title = title,
                DueDate = date,
                Project = project,
                Priority = priority,
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
    Console.WriteLine("4. Back to Menu");

    string sfChoice = ReadNonEmpty("Choose an option: ");

    if (sfChoice == "1")
    {
        string projectName = ReadNonEmpty("Enter project name: ");
        var results = taskManager.SearchByProject(projectName);

        Console.WriteLine("=== RESULTS ===");
        foreach (var t in results)
            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate.ToShortDateString()} - {t.Priority} - {(t.IsCompleted ? "Done" : "Pending")}");

        Console.ReadKey();
    }
    else if (sfChoice == "2")
    {
        DateTime date = ReadDate("Enter date (yyyy-mm-dd): ");
        Console.Write("Filter (before/after/on): ");
        string mode = Console.ReadLine() ?? "on";

        var results = taskManager.FilterByDate(date, mode);

        Console.WriteLine("=== RESULTS ===");
        foreach (var t in results)
            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate.ToShortDateString()} - {t.Priority} - {(t.IsCompleted ? "Done" : "Pending")}");

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
            Console.WriteLine($"{t.Title} - {t.Project} - {t.DueDate.ToShortDateString()} - {t.Priority} - {(t.IsCompleted ? "Done" : "Pending")}");

        Console.ReadKey();
    }
    break;

        default:
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            break;
    }
}

int ReadInt(string message)
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

DateTime ReadDate(string message)
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

string ReadNonEmpty(string message)
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

string ReadPriority(string message)
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
