namespace EventTodoAssistant.Models
{
    public class TaskItem
    {
        public string Title { get; set; } = "";
        public DateTime DueDate { get; set; }
        public string Project { get; set; } = "";

        public string Priority { get; set; } = "Medium";

        public bool IsCompleted { get; set; }

        public void MarkCompleted()
        {
            IsCompleted = true;
        }

        public void UpdateTitle(string newTitle)
        {
            Title = newTitle;
        }

        public void UpdateDate(DateTime newDate)
        {
            DueDate = newDate;
        }

        public void UpdateProject(string newProject)
        {
            Project = newProject;
        }
    }
}
