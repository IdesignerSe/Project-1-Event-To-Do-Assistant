namespace EventTodoAssistant.UI
{
    public class MenuUI
    {
        public void ShowMainMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

Console.WriteLine("========================================");
Console.WriteLine("========================================");
Console.WriteLine("========================================");
Console.WriteLine("========= EVENT TODO ASSISTANT =========");
Console.WriteLine("========================================");
Console.WriteLine("========================================");
Console.WriteLine("========================================");

Console.ResetColor();


            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("MAIN MENU");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");
            
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("TASK MANAGEMENT");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("1. Show Tasks");
            Console.ResetColor();
            Console.WriteLine("   → View all your tasks in a clean list");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("2. Add Task");
            Console.ResetColor();
            Console.WriteLine("   → Create a new task with title, date, project, and priority");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("3. Edit Task");
            Console.ResetColor();
            Console.WriteLine("   → Change the details of an existing task");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("4. Mark Task as Done");
            Console.ResetColor();
            Console.WriteLine("   → Mark a task as completed");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("5. Remove Task");
            Console.ResetColor();
            Console.WriteLine("   → Delete a task permanently");

            Console.WriteLine();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("VIEW & SORTING OPTIONS");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("6. Sort by Date");
            Console.ResetColor();
            Console.WriteLine("   → Show tasks ordered by due date");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("7. Sort by Project");
            Console.ResetColor();
            Console.WriteLine("   → Group tasks by project name");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("11. Sort by Priority");
            Console.ResetColor();
            Console.WriteLine("   → Show urgent tasks first");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("14. Sort by Category");
            Console.ResetColor();
            Console.WriteLine("   → Organize tasks by category (Work, Personal, Event, etc.)");

            Console.WriteLine();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("SEARCH & ANALYTICS");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("10. Search & Filter Tasks");
            Console.ResetColor();
            Console.WriteLine("   → Find tasks by keyword, date, project, or tag");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0. Dashboard Summary");
            Console.ResetColor();
            Console.WriteLine("   → Quick overview of your tasks, deadlines, and progress");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("15. View Tag Statistics");
            Console.ResetColor();
            Console.WriteLine("   → See how many tasks use each tag");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("17. View Task History");
            Console.ResetColor();
            Console.WriteLine("   → View completed tasks and changes over time");

            Console.WriteLine();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("AI-POWERED FEATURES");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("9. AI Event Suggestions");
            Console.ResetColor();
            Console.WriteLine("   → Get automatic ideas for tasks based on event type");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("16. AI Event Suggestions with Context");
            Console.ResetColor();
            Console.WriteLine("   → Smarter suggestions using your existing tasks");

            Console.WriteLine();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("SAVE & EXPORT");
            Console.ResetColor();
            Console.WriteLine("----------------------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("12. Export to CSV");
            Console.ResetColor();
            Console.WriteLine("   → Save your tasks to a spreadsheet file");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("13. View Task Details");
            Console.ResetColor();
            Console.WriteLine("   → See full information about a specific task");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("8. Save & Quit");
            Console.ResetColor();
            Console.WriteLine("   → Save your work and exit the app");

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Choose an option number: ");
            Console.ResetColor();
        }
    }
}
