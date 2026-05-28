using EventTodoAssistant.Models;
using EventTodoAssistant.Services;
using EventTodoAssistant.UI;

class Program
{
    static void Main(string[] args)
    {
        var taskManager = new TaskManager();
        //var aiService = new AISuggestionService();
        var displayService = new DisplayService();
        var menu = new MenuUI();

        // AI service (mock for now — safe, no internet needed)
        IAIService aiService = new AISuggestionServiceAdapter(new AISuggestionService());

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
                    Console.WriteLine();
                    Console.WriteLine("────────────────────────────────────────");
                    Console.WriteLine();

                    Console.WriteLine("=== DASHBOARD SUMMARY ===");
                    Console.WriteLine("-------------------------\n");
                    Console.WriteLine($"Current Date: {DateTime.Now:yyyy-MM-dd}\n");

                    var summary = taskManager.GetSummary();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Overdue Tasks: ---> {summary.overdue}");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Due Today:--------> {summary.dueToday}");

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Due This Week:----> {summary.dueWeek}");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Completed:--------> {summary.completed}");

                    Console.WriteLine("-------------------------\n");
                    Console.ResetColor();
                    Console.WriteLine($"Total Tasks:------> {summary.total}");

                    Console.WriteLine("\nPress any key to return to MAIN MENU.");
                    Console.ReadKey();
                    break;

                case "1":
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("────────────────────────────────────────");
                    Console.WriteLine();

                    Console.WriteLine("=== TASK LIST ===");
                    Console.WriteLine();


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

                    Console.WriteLine();
                    Console.WriteLine("────────────────────────────────────────");
                    Console.WriteLine();
                    Console.WriteLine("\nPress any key to return to Main Menu...");


                    Console.ReadKey();
                    break;

                case "2":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== ADD TASK ===");

                        // Show existing tasks in a clean table
                        var allTasks = taskManager.Tasks;

                        Console.WriteLine("=== TASK LIST ===");
                        Console.WriteLine("Idx   Title");
                        Console.WriteLine("-------------------------------");

                        if (allTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < allTasks.Count; i++)
                            {
                                Console.WriteLine($"{i + 1,-5} {allTasks[i].Title}");
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("1. Add a new task");
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice2 = ReadNonEmpty("Choose an option: ");

                        if (subChoice2 == "0")
                            break;

                        if (subChoice2 == "1")
                        {
                            Console.Clear();

                            // Show task list again before adding
                            Console.WriteLine("=== TASK LIST ===");
                            Console.WriteLine("Idx   Title");
                            Console.WriteLine("-------------------------------");

                            if (allTasks.Count == 0)
                            {
                                Console.WriteLine("No tasks yet.");
                            }
                            else
                            {
                                for (int i = 0; i < allTasks.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1,-5} {allTasks[i].Title}");
                                }
                            }

                            Console.WriteLine("────────────────────────────────────────");
                            Console.WriteLine();
                            Console.WriteLine("=== NEW TASK ===");

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

                            Console.WriteLine();
                            Console.WriteLine("Task added!");
                            Console.WriteLine("\nPress any key to return...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                            Console.ReadKey();
                        }
                    }
                    break;

                case "3":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== EDIT TASK ===");

                        // Show existing tasks in a clean table
                        var allTasks = taskManager.Tasks;

                        Console.WriteLine("=== TASK LIST ===");
                        Console.WriteLine("Idx   Title");
                        Console.WriteLine("-------------------------------");

                        if (allTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < allTasks.Count; i++)
                            {
                                Console.WriteLine($"{i + 1,-5} {allTasks[i].Title}");
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("1. Edit a task");
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice3 = ReadNonEmpty("Choose an option: ");

                        if (subChoice3 == "0")
                            break;

                        if (subChoice3 == "1")
                        {
                            Console.Clear();

                            // Show task list again before editing
                            Console.WriteLine("=== TASK LIST ===");
                            Console.WriteLine("Idx   Title");
                            Console.WriteLine("-------------------------------");

                            if (allTasks.Count == 0)
                            {
                                Console.WriteLine("No tasks yet.");
                            }
                            else
                            {
                                for (int i = 0; i < allTasks.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1,-5} {allTasks[i].Title}");
                                }
                            }

                            Console.WriteLine("────────────────────────────────────────");
                            Console.WriteLine();

                            int editIndex = ReadInt("Enter task number to edit: ") - 1;

                            if (editIndex < 0 || editIndex >= allTasks.Count)
                            {
                                Console.WriteLine("Invalid number. Try again.");
                                Console.ReadKey();
                                continue;
                            }

                            var task = allTasks[editIndex];

                            Console.Clear();
                            Console.WriteLine("=== EDITING TASK ===");

                            Console.WriteLine($"Current Title: {task.Title}");
                            string newTitle = ReadOptional("New title (leave empty to keep): ");
                            if (!string.IsNullOrWhiteSpace(newTitle))
                                task.Title = newTitle;

                            Console.WriteLine($"Current Due Date: {task.DueDate:yyyy-MM-dd}");
                            DateTime newDate = ReadOptionalDate("New due date (yyyy-mm-dd, leave empty to keep): ", task.DueDate);
                            task.DueDate = newDate;

                            Console.WriteLine($"Current Project: {task.Project}");
                            string newProject = ReadOptional("New project (leave empty to keep): ");
                            if (!string.IsNullOrWhiteSpace(newProject))
                                task.Project = newProject;

                            Console.WriteLine($"Current Priority: {task.Priority}");
                            string newPriority = ReadOptionalPriority("New priority (low/medium/high, leave empty to keep): ", task.Priority);
                            task.Priority = newPriority;

                            Console.WriteLine($"Current Tags: {string.Join(", ", task.Tags)}");
                            string newTags = ReadOptional("New tags (comma-separated, leave empty to keep): ");
                            if (!string.IsNullOrWhiteSpace(newTags))
                                task.Tags = newTags.Split(',').Select(t => t.Trim()).ToList();

                            Console.WriteLine($"Current Description: {task.Description}");
                            string newDesc = ReadOptional("New description (leave empty to keep): ");
                            if (!string.IsNullOrWhiteSpace(newDesc))
                                task.Description = newDesc;

                            Console.WriteLine();
                            Console.WriteLine("Task updated!");
                            Console.WriteLine("\nPress any key to return...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                            Console.ReadKey();
                        }
                    }
                    break;


                case "4":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== MARK TASK AS COMPLETED ===");

                        // Show existing tasks in a clean table
                        var allTasks = taskManager.Tasks;

                        Console.WriteLine("=== TASK LIST ===");
                        Console.WriteLine("Idx   Status   Title");
                        Console.WriteLine("------------------------------------------");

                        if (allTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < allTasks.Count; i++)
                            {
                                string status = allTasks[i].IsCompleted ? "[DONE]" : "[   ]";
                                Console.WriteLine($"{i + 1,-5} {status,-8} {allTasks[i].Title}");
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("1. Mark a task as completed");
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice4 = ReadNonEmpty("Choose an option: ");

                        if (subChoice4 == "0")
                            break;

                        if (subChoice4 == "1")
                        {
                            Console.Clear();

                            // Show task list again before marking completed
                            Console.WriteLine("=== TASK LIST ===");
                            Console.WriteLine("Idx   Status   Title");
                            Console.WriteLine("------------------------------------------");

                            if (allTasks.Count == 0)
                            {
                                Console.WriteLine("No tasks yet.");
                            }
                            else
                            {
                                for (int i = 0; i < allTasks.Count; i++)
                                {
                                    string status = allTasks[i].IsCompleted ? "[DONE]" : "[   ]";
                                    Console.WriteLine($"{i + 1,-5} {status,-8} {allTasks[i].Title}");
                                }
                            }

                            Console.WriteLine("────────────────────────────────────────");
                            Console.WriteLine();

                            int completeIndex = ReadInt("Enter task number to mark as completed: ") - 1;

                            if (completeIndex < 0 || completeIndex >= allTasks.Count)
                            {
                                Console.WriteLine("Invalid number. Try again.");
                                Console.ReadKey();
                                continue;
                            }

                            var task = allTasks[completeIndex];

                            if (task.IsCompleted)
                            {
                                Console.WriteLine("This task is already completed.");
                            }
                            else
                            {
                                task.IsCompleted = true;
                                Console.WriteLine("Task marked as completed!");
                            }

                            Console.WriteLine("\nPress any key to return...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                            Console.ReadKey();
                        }
                    }
                    break;

                case "5":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== REMOVE TASK ===");

                        // Show existing tasks in a clean table
                        var allTasks = taskManager.Tasks;

                        Console.WriteLine("=== TASK LIST ===");
                        Console.WriteLine("Idx   Title");
                        Console.WriteLine("-------------------------------");

                        if (allTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < allTasks.Count; i++)
                            {
                                Console.WriteLine($"{i + 1,-5} {allTasks[i].Title}");
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("1. Remove a task");
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice5 = ReadNonEmpty("Choose an option: ");

                        if (subChoice5 == "0")
                            break;

                        if (subChoice5 == "1")
                        {
                            Console.Clear();

                            // Show task list again before removing
                            Console.WriteLine("=== TASK LIST ===");
                            Console.WriteLine("Idx   Title");
                            Console.WriteLine("-------------------------------");

                            if (allTasks.Count == 0)
                            {
                                Console.WriteLine("No tasks yet.");
                            }
                            else
                            {
                                for (int i = 0; i < allTasks.Count; i++)
                                {
                                    Console.WriteLine($"{i + 1,-5} {allTasks[i].Title}");
                                }
                            }

                            Console.WriteLine("────────────────────────────────────────");
                            Console.WriteLine();

                            int deleteIndex = ReadInt("Enter task number to remove: ") - 1;

                            if (deleteIndex < 0 || deleteIndex >= allTasks.Count)
                            {
                                Console.WriteLine("Invalid number. Try again.");
                                Console.ReadKey();
                                continue;
                            }

                            taskManager.Tasks.RemoveAt(deleteIndex);

                            Console.WriteLine("Task removed!");
                            Console.WriteLine("\nPress any key to return...");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Invalid option.");
                            Console.ReadKey();
                        }
                    }
                    break;

                case "6":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== SORT TASKS BY DATE ===");

                        // Sort tasks by date
                        var sortedTasks = taskManager.Tasks
                            .OrderBy(t => t.DueDate)
                            .ToList();

                        Console.WriteLine("=== TASK LIST (Sorted by Date) ===");
                        Console.WriteLine("Idx   Title                           Due Date");
                        Console.WriteLine("------------------------------------------------------");

                        if (sortedTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < sortedTasks.Count; i++)
                            {
                                Console.WriteLine($"{i + 1,-5} {sortedTasks[i].Title,-30} {sortedTasks[i].DueDate:yyyy-MM-dd}");
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("The list is automatically sorted by date. To save this order, go to Main Menu Option 8 to save to file.");
                        Console.WriteLine("-----------------------------");

                        string subChoice6 = ReadNonEmpty("Choose an option: ");

                        if (subChoice6 == "0")
                            break;

                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                    }
                    break;

                case "7":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== SORT TASKS BY PROJECT ===");

                        // Sort tasks by project name
                        var sortedTasks = taskManager.Tasks
                            .OrderBy(t => t.Project)
                            .ToList();

                        Console.WriteLine("=== TASK LIST (Sorted by Project) ===");
                        Console.WriteLine("Idx   Project                           Priority    Status         Title");
                        Console.WriteLine("---------------------------------------------------------------------------------------");

                        if (sortedTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < sortedTasks.Count; i++)
                            {
                                string status = sortedTasks[i].IsCompleted ? "[DONE]" : "[   ]";

                                Console.WriteLine(
                                    $"{i + 1,-5}" +
                                    $"{sortedTasks[i].Project,-35}" +
                                    $"{sortedTasks[i].Priority,-12}" +
                                    $"{status,-10}" +
                                    $"{sortedTasks[i].Title}"
                                );
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice7 = ReadNonEmpty("Choose an option: ");

                        if (subChoice7 == "0")
                            break;

                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                    }
                    break;

                case "8":
                    taskManager.SaveToJson(filePath);
                    running = false;
                    break;

                case "9":
                    Console.Clear();
                    Console.WriteLine("=== AI EVENT SUGGESTIONS ===\n");

                    Console.Write("Enter event name: ");
                    string eventName = Console.ReadLine()?.Trim() ?? "";

                    Console.WriteLine("\nGenerating suggestions...\n");

                    // Get suggestions from AI service
                    var suggestions = aiService.GenerateSuggestionsAsync(eventName).Result;

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
                    string aiChoice = Console.ReadLine()?.Trim() ?? "";

                    if (aiChoice == "1")
                    {
                        Console.Write("Enter suggestion index to save: ");
                        int idx = int.Parse(Console.ReadLine() ?? "0");

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
                        int idx = int.Parse(Console.ReadLine() ?? "0");

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
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== SEARCH & FILTER ===");
                        Console.WriteLine("1. Search by Project");
                        Console.WriteLine("2. Filter by Date");
                        Console.WriteLine("3. Filter by Status");
                        Console.WriteLine("4. Search by Tag");
                        Console.WriteLine("5. Back to Main Menu");

                        string sfChoice = ReadNonEmpty("Choose an option: ");

                        if (sfChoice == "5")
                            break;

                        // ---------------------------------------------------------
                        // ALWAYS SHOW FULL TASK LIST FIRST
                        // ---------------------------------------------------------
                        Console.Clear();
                        Console.WriteLine("=== TASK LIST ===");
                        Console.WriteLine("Idx   Status     Priority     Project                           Title");
                        Console.WriteLine("---------------------------------------------------------------------------------------");

                        var allTasks = taskManager.Tasks;

                        if (allTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < allTasks.Count; i++)
                            {
                                string status = allTasks[i].IsCompleted ? "[DONE]" : "[   ]";

                                Console.WriteLine(
                                    $"{i + 1,-5}" +
                                    $"{status,-10}" +
                                    $"{allTasks[i].Priority,-12}" +
                                    $"{allTasks[i].Project,-35}" +
                                    $"{allTasks[i].Title}"
                                );
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();

                        // ---------------------------------------------------------
                        // OPTION 1 — SEARCH BY PROJECT
                        // ---------------------------------------------------------
                        if (sfChoice == "1")
                        {
                            string projectName = ReadNonEmpty("Enter project name: ");

                            var results = taskManager.Tasks
                                .Where(t => t.Project.Contains(projectName, StringComparison.OrdinalIgnoreCase))
                                .ToList();

                            Console.Clear();
                            Console.WriteLine($"=== RESULTS FOR PROJECT: {projectName} ===");
                            Console.WriteLine("Idx   Status     Priority     Project                           Title");
                            Console.WriteLine("---------------------------------------------------------------------------------------");

                            if (results.Count == 0)
                            {
                                Console.WriteLine("No matching tasks found.");
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    string status = results[i].IsCompleted ? "[DONE]" : "[   ]";

                                    Console.WriteLine(
                                        $"{i + 1,-5}" +
                                        $"{status,-10}" +
                                        $"{results[i].Priority,-12}" +
                                        $"{results[i].Project,-35}" +
                                        $"{results[i].Title}"
                                    );
                                }
                            }
                        }

                        // ---------------------------------------------------------
                        // OPTION 2 — FILTER BY DATE
                        // ---------------------------------------------------------
                        else if (sfChoice == "2")
                        {
                            DateTime filterDate = ReadDate("Enter date (yyyy-mm-dd): ");
                            Console.Write("Filter (before/after/on): ");
                            string mode = Console.ReadLine() ?? "on";

                            var results = taskManager.FilterByDate(filterDate, mode);

                            Console.Clear();
                            Console.WriteLine($"=== RESULTS FOR DATE FILTER ({mode}) ===");
                            Console.WriteLine("Idx   Status     Priority     Project                           Title");
                            Console.WriteLine("---------------------------------------------------------------------------------------");

                            if (results.Count == 0)
                            {
                                Console.WriteLine("No matching tasks found.");
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    string status = results[i].IsCompleted ? "[DONE]" : "[   ]";

                                    Console.WriteLine(
                                        $"{i + 1,-5}" +
                                        $"{status,-10}" +
                                        $"{results[i].Priority,-12}" +
                                        $"{results[i].Project,-35}" +
                                        $"{results[i].Title}"
                                    );
                                }
                            }
                        }

                        // ---------------------------------------------------------
                        // OPTION 3 — FILTER BY STATUS
                        // ---------------------------------------------------------
                        else if (sfChoice == "3")
                        {
                            Console.Write("Show completed? (yes/no): ");
                            string ans = Console.ReadLine()?.ToLower() ?? "no";
                            bool completed = ans == "yes";

                            var results = taskManager.FilterByStatus(completed);

                            Console.Clear();
                            Console.WriteLine($"=== RESULTS FOR STATUS: {(completed ? "Completed" : "Pending")} ===");
                            Console.WriteLine("Idx   Status     Priority     Project                           Title");
                            Console.WriteLine("---------------------------------------------------------------------------------------");

                            if (results.Count == 0)
                            {
                                Console.WriteLine("No matching tasks found.");
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    string status = results[i].IsCompleted ? "[DONE]" : "[   ]";

                                    Console.WriteLine(
                                        $"{i + 1,-5}" +
                                        $"{status,-10}" +
                                        $"{results[i].Priority,-12}" +
                                        $"{results[i].Project,-35}" +
                                        $"{results[i].Title}"
                                    );
                                }
                            }
                        }

                        // ---------------------------------------------------------
                        // OPTION 4 — SEARCH BY TAG
                        // ---------------------------------------------------------
                        else if (sfChoice == "4")
                        {
                            string tagSearch = ReadNonEmpty("Enter tag to search: ");

                            var results = taskManager.Tasks
                                .Where(t => t.Tags.Any(tag =>
                                    tag.Contains(tagSearch, StringComparison.OrdinalIgnoreCase)))
                                .ToList();

                            Console.Clear();
                            Console.WriteLine($"=== RESULTS FOR TAG: {tagSearch} ===");
                            Console.WriteLine("Idx   Status     Priority     Project                           Title");
                            Console.WriteLine("---------------------------------------------------------------------------------------");

                            if (results.Count == 0)
                            {
                                Console.WriteLine("No matching tasks found.");
                            }
                            else
                            {
                                for (int i = 0; i < results.Count; i++)
                                {
                                    string status = results[i].IsCompleted ? "[DONE]" : "[   ]";

                                    Console.WriteLine(
                                        $"{i + 1,-5}" +
                                        $"{status,-10}" +
                                        $"{results[i].Priority,-12}" +
                                        $"{results[i].Project,-35}" +
                                        $"{results[i].Title}"
                                    );
                                }
                            }
                        }

                        // ---------------------------------------------------------
                        // NAVIGATION OPTIONS AFTER RESULTS
                        // ---------------------------------------------------------
                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine("0. Back to Search & Filter Menu");
                        Console.WriteLine("1. Back to Main Menu");

                        string nav = ReadNonEmpty("Choose an option: ");

                        if (nav == "1")
                            break;
                        else
                            continue;
                    }
                    break;

                case "11":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== SORT TASKS BY PRIORITY ===");

                        // Sort tasks by priority (High → Medium → Low)
                        var sortedTasks = taskManager.Tasks
                            .OrderBy(t => t.Priority) // If you want custom order, tell me
                            .ToList();

                        Console.WriteLine("=== TASK LIST (Sorted by Priority) ===");
                        Console.WriteLine("Idx   Priority     Project                           Status     Title");
                        Console.WriteLine("---------------------------------------------------------------------------------------");

                        if (sortedTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < sortedTasks.Count; i++)
                            {
                                string status = sortedTasks[i].IsCompleted ? "[DONE]" : "[   ]";

                                Console.WriteLine(
                                    $"{i + 1,-5}" +
                                    $"{sortedTasks[i].Priority,-12}" +
                                    $"{sortedTasks[i].Project,-35}" +
                                    $"{status,-10}" +
                                    $"{sortedTasks[i].Title}"
                                );
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice11 = ReadNonEmpty("Choose an option: ");

                        if (subChoice11 == "0")
                            break;

                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                    }
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

                    Console.WriteLine("\nPress any key to return to Main Menu...");
                    Console.ReadKey();
                    break;

                case "14":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== SORT TASKS BY STATUS ===");

                        // Sort tasks: Pending first, Done last
                        var sortedTasks = taskManager.Tasks
                            .OrderBy(t => t.IsCompleted) // false (pending) first, true (done) last
                            .ToList();

                        Console.WriteLine("=== TASK LIST (Sorted by Status) ===");
                        Console.WriteLine("Idx  Status    Priority     Project                           Title");
                        Console.WriteLine("---------------------------------------------------------------------------------------");

                        if (sortedTasks.Count == 0)
                        {
                            Console.WriteLine("No tasks yet.");
                        }
                        else
                        {
                            for (int i = 0; i < sortedTasks.Count; i++)
                            {
                                string status = sortedTasks[i].IsCompleted ? "[DONE]" : "[   ]";

                                Console.WriteLine(
                                    $"{i + 1,-5}" +
                                    $"{status,-10}" +
                                    $"{sortedTasks[i].Priority,-12}" +
                                    $"{sortedTasks[i].Project,-35}" +
                                    $"{sortedTasks[i].Title}"
                                );
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("0. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string subChoice14 = ReadNonEmpty("Choose an option: ");

                        if (subChoice14 == "0")
                            break;

                        Console.WriteLine("Invalid option.");
                        Console.ReadKey();
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice.");
                    Console.ReadKey();
                    break;

                case "15":
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("=== TAG STATISTICS ===");
                        Console.WriteLine();

                        // Build tag dictionary
                        var tagCounts = new Dictionary<string, int>();

                        foreach (var task in taskManager.Tasks)
                        {
                            foreach (var tag in task.Tags)
                            {
                                if (!tagCounts.ContainsKey(tag))
                                    tagCounts[tag] = 0;

                                tagCounts[tag]++;
                            }
                        }

                        // Sort alphabetically
                        var sortedTags = tagCounts.OrderBy(t => t.Key).ToList();

                        // Header
                        Console.WriteLine("Tag                            Count");
                        Console.WriteLine("----------------------------------------------");

                        if (sortedTags.Count == 0)
                        {
                            Console.WriteLine("No tags found.");
                        }
                        else
                        {
                            foreach (var entry in sortedTags)
                            {
                                Console.WriteLine($"{entry.Key,-30} {entry.Value} task(s)");
                            }
                        }

                        Console.WriteLine("────────────────────────────────────────");
                        Console.WriteLine();
                        Console.WriteLine("1. Back to Main Menu");
                        Console.WriteLine("-----------------------------");

                        string nav = ReadNonEmpty("Choose an option: ");

                        if (nav == "1")
                            break;
                        else
                            continue;
                    }
                    break;


                case "16":
                    Console.Clear();
                    Console.WriteLine("=== SMART EVENT ASSISTANT ===\n");

                    string evName = ReadNonEmpty("Event name: ");
                    DateTime evDate = ReadDate("Event date (yyyy-mm-dd): ");

                    var generated = aiService.GenerateEventTasksAsync(evName, evDate).Result;

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
            string? input = Console.ReadLine() ?? "";

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
    // OPTIONAL INPUT HELPERS
    static string ReadOptional(string message)
    {
        Console.Write(message);
        return Console.ReadLine() ?? "";
    }

    static DateTime ReadOptionalDate(string message, DateTime currentValue)
    {
        Console.Write(message);
        string input = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(input))
            return currentValue;

        if (DateTime.TryParse(input, out DateTime result))
            return result;

        Console.WriteLine("Invalid date. Keeping old value.");
        return currentValue;
    }

    static string ReadOptionalPriority(string message, string currentValue)
    {
        Console.Write(message);
        string input = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(input))
            return currentValue;

        input = input.Trim().ToLower();

        if (input == "low" || input == "medium" || input == "high")
            return input;

        Console.WriteLine("Invalid priority. Keeping old value.");
        return currentValue;
    }

}
