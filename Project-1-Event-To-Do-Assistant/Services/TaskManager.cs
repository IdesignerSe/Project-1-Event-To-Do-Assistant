using EventTodoAssistant.Models;
using System.Text.Json;

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
        
    }
}
