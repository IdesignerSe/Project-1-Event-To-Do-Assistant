namespace EventTodoAssistant.Models
{
    public class TaskItem
    {
        public string Title { get; set; } = "";
        public DateTime DueDate { get; set; }
        public string Project { get; set; } = "";
        public string Priority { get; set; } = "Medium";
        public string Category { get; set; } = "General";
        public string Description { get; set; } = "";

        public bool IsCompleted { get; set; }
        public bool IsOverdue => !IsCompleted && DueDate.Date < DateTime.Now.Date;

        // ===== Update Methods =====

        public void UpdateTitle(string newTitle) => Title = newTitle;

        public void UpdateDate(DateTime newDate) => DueDate = newDate;

        public void UpdateProject(string newProject) => Project = newProject;

        public void UpdatePriority(string newPriority) => Priority = newPriority;

        public void UpdateCategory(string newCategory) => Category = newCategory;

        public void UpdateDescription(string newDescription) => Description = newDescription;

        public void MarkCompleted() => IsCompleted = true;
    }
}
