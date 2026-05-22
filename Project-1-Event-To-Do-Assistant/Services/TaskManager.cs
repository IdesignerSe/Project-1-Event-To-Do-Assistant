using EventTodoAssistant.Models;

namespace EventTodoAssistant.Services
{
    public class TaskManager
    {
        public List<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

        public void AddTask(TaskItem task)
        {
            Tasks.Add(task);
        }

        public void RemoveTask(int index)
        {
            if (index >= 0 && index < Tasks.Count)
                Tasks.RemoveAt(index);
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


public void SaveToFile(string filePath)
{
    var lines = Tasks.Select(t =>
        $"{t.Title}|{t.DueDate:yyyy-MM-dd}|{t.Project}|{t.IsCompleted}");

    File.WriteAllLines(filePath, lines);
}

public void LoadFromFile(string filePath)
{
    if (!File.Exists(filePath))
        return;

    var lines = File.ReadAllLines(filePath);

    foreach (var line in lines)
    {
        var parts = line.Split('|');
        if (parts.Length == 4)
        {
            Tasks.Add(new TaskItem
            {
                Title = parts[0],
                DueDate = DateTime.Parse(parts[1]),
                Project = parts[2],
                IsCompleted = bool.Parse(parts[3])
            });
        }
    }
}

        
    }
}
