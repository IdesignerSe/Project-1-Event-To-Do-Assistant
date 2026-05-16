using EventTodoAssistant.Models;
using EventTodoAssistant.Services;
using EventTodoAssistant.UI;

var taskManager = new TaskManager();
var aiService = new AISuggestionService();
var displayService = new DisplayService();
var menu = new MenuUI();

bool running = true;

while (running)
{
    Console.Clear();
    menu.ShowMainMenu();

    Console.Write("Choose an option: ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Clear();
            Console.WriteLine("=== YOUR TASKS ===");
            foreach (var task in taskManager.Tasks)
            {
                Console.WriteLine($"{task.Title} - {task.DueDate.ToShortDateString()} - {task.Project} - {(task.IsCompleted ? "Done" : "Pending")}");
            }
            Console.ReadKey();
            break;

        case "2":
            Console.Clear();
            Console.Write("Task title: ");
            string title = Console.ReadLine() ?? "";

            Console.Write("Due date (yyyy-mm-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine() ?? "");

            Console.Write("Project: ");
            string project = Console.ReadLine() ?? "";

            taskManager.AddTask(new TaskItem
            {
                Title = title,
                DueDate = date,
                Project = project,
                IsCompleted = false
            });

            Console.WriteLine("Task added!");
            Console.ReadKey();
            break;

        case "3":
            Console.Clear();
            Console.Write("Enter task index to edit: ");
            int editIndex = int.Parse(Console.ReadLine() ?? "0");

            var taskToEdit = taskManager.GetTask(editIndex);
            if (taskToEdit != null)
            {
                Console.Write("New title: ");
                taskToEdit.UpdateTitle(Console.ReadLine() ?? "");

                Console.Write("New date (yyyy-mm-dd): ");
                taskToEdit.UpdateDate(DateTime.Parse(Console.ReadLine() ?? ""));

                Console.Write("New project: ");
                taskToEdit.UpdateProject(Console.ReadLine() ?? "");

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
            Console.Write("Enter task index to mark as done: ");
            int doneIndex = int.Parse(Console.ReadLine() ?? "0");

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
            Console.Write("Enter task index to remove: ");
            int removeIndex = int.Parse(Console.ReadLine() ?? "0");

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
            running = false;
            break;

        case "9":
            Console.Clear();
            Console.Write("Enter event name: ");
            string eventName = Console.ReadLine() ?? "";

            var suggestions = aiService.GetMockSuggestions(eventName);
            displayService.ShowSplitScreen(taskManager.Tasks, suggestions);

            Console.ReadKey();
            break;

        default:
            Console.WriteLine("Invalid choice.");
            Console.ReadKey();
            break;
    }
}
