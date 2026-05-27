namespace EventTodoAssistant.Models
{
    public class TaskItem
    {
        // Unique ID for each task
        public Guid Id { get; set; } = Guid.NewGuid();

        // Short ID for UI display (first 3 chars of GUID)
        public string ShortId => Id.ToString().Substring(0, 3);

        // Core fields
        public string Title { get; set; } = "";
        public DateTime DueDate { get; set; }
        public string Project { get; set; } = "";
        public string Priority { get; set; } = "Medium";
        public List<string> Tags { get; set; } = new List<string>();
        public string Description { get; set; } = "";

        // Completion state
        public bool IsCompleted { get; set; } = false;

        // Overdue logic
        public bool IsOverdue => !IsCompleted && DueDate.Date < DateTime.Now.Date;

        // Computed status for UI
        public string Status
        {
            get
            {
                if (IsCompleted)
                    return "Done";

                if (IsOverdue)
                    return "OVERDUE";

                return "Pending";
            }
        }

        // Update methods (optional but useful)
        public void MarkCompleted() => IsCompleted = true;
        public void UpdateTitle(string newTitle) => Title = newTitle;
        public void UpdateDate(DateTime newDate) => DueDate = newDate;
        public void UpdateProject(string newProject) => Project = newProject;
        public void UpdatePriority(string newPriority) => Priority = newPriority;
        public void UpdateTags(List<string> newTags) => Tags = newTags;
        public void UpdateDescription(string newDescription) => Description = newDescription;
    }
}
