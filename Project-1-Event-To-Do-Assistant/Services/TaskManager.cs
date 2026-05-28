//TODO: Later add history log to Json file and load it on startup, also add option to view history in the menu. 
using EventTodoAssistant.Models;
using System.Text.Json;

namespace EventTodoAssistant.Services
{
    public class TaskManager
    {
        public List<TaskItem> Tasks { get; private set; } = new List<TaskItem>();
        public List<AISuggestionItem> AISuggestions { get; set; } = new List<AISuggestionItem>();


        public void AddTask(TaskItem task)
        {
            Tasks.Add(task);
            LogHistory($"Created task \"{task.Title}\" with due date {task.DueDate:yyyy-MM-dd}");
        }

        public void RemoveTask(int index)
        {
            if (index >= 0 && index < Tasks.Count)
            {
                var task = Tasks[index];
                Tasks.RemoveAt(index);
                LogHistory($"Removed task \"{task.Title}\" with due date {task.DueDate:yyyy-MM-dd}");
            }
        }

        public TaskItem? GetTask(int index)
        {
            if (index >= 0 && index < Tasks.Count)
                return Tasks[index];

            return null;
        }

        public void SortByDate()
        {
            Tasks = Tasks.OrderBy(t => t.DueDate).ToList();
        }

        public void SortByProject()
        {
            Tasks = Tasks.OrderBy(t => t.Project).ToList();
        }

        public void SortByPriority()
        {
            Tasks = Tasks
                .OrderBy(t => t.Priority switch
                {
                    "Low" => 1,
                    "Medium" => 2,
                    "High" => 3,
                    _ => 4
                })
                .ToList();
        }

        public void SaveToJson(string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(Tasks, options);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromJson(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            string json = File.ReadAllText(filePath);

            var loadedTasks = JsonSerializer.Deserialize<List<TaskItem>>(json);

            if (loadedTasks != null)
                Tasks = loadedTasks;
        }

        public List<TaskItem> SearchByProject(string project)
        {
            return Tasks
                .Where(t => t.Project.Contains(project, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<TaskItem> FilterByDate(DateTime date, string mode)
        {
            return mode.ToLower() switch
            {
                "before" => Tasks.Where(t => t.DueDate < date).ToList(),
                "after" => Tasks.Where(t => t.DueDate > date).ToList(),
                "on" => Tasks.Where(t => t.DueDate.Date == date.Date).ToList(),
                _ => new List<TaskItem>()
            };
        }

        public List<TaskItem> FilterByStatus(bool isCompleted)
        {
            return Tasks
                .Where(t => t.IsCompleted == isCompleted)
                .ToList();
        }

        public void ExportToCsv(string filePath)
        {
            var lines = new List<string>();

            // Updated header
            lines.Add("Title,DueDate,Project,Priority,Tags,Description,IsCompleted,IsOverdue");

            foreach (var t in Tasks)
            {
                string line =
                    $"{t.Title}," +
                    $"{t.DueDate:yyyy-MM-dd}," +
                    $"{t.Project}," +
                    $"{t.Priority}," +
                    $"\"{string.Join(";", t.Tags)}\"," +   // tags exported as semicolon-separated
                    $"\"{t.Description}\"," +
                    $"{t.IsCompleted}," +
                    $"{t.IsOverdue}";

                lines.Add(line);
            }

            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllLines(filePath, lines);
        }

        public (int overdue, int dueToday, int dueWeek, int completed, int total) GetSummary()
        {
            DateTime today = DateTime.Now.Date;
            DateTime weekEnd = today.AddDays(7);

            int overdue = Tasks.Count(t => !t.IsCompleted && t.DueDate.Date < today);
            int dueToday = Tasks.Count(t => !t.IsCompleted && t.DueDate.Date == today);
            int dueWeek = Tasks.Count(t => !t.IsCompleted && t.DueDate.Date > today && t.DueDate.Date <= weekEnd);
            int completed = Tasks.Count(t => t.IsCompleted);
            int total = Tasks.Count;

            return (overdue, dueToday, dueWeek, completed, total);
        }

        public void SortByCategory()
        {
            // Now sorts by first tag alphabetically
            Tasks = Tasks
                .OrderBy(t => t.Tags.Count > 0 ? t.Tags[0] : "")
                .ThenBy(t => t.Title)
                .ToList();
        }

        public Dictionary<string, int> GetTagStatistics()
        {
            var stats = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var task in Tasks)
            {
                foreach (var tag in task.Tags)
                {
                    if (stats.ContainsKey(tag))
                        stats[tag]++;
                    else
                        stats[tag] = 1;
                }
            }

            return stats
                .OrderByDescending(kvp => kvp.Value)
                .ThenBy(kvp => kvp.Key)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private readonly string historyPath = "task_history.txt";

        public void LogHistory(string message)
        {
            File.AppendAllText(historyPath, $"[{DateTime.Now:yyyy-MM-dd HH:mm}] {message}\n");
        }


    }
}
