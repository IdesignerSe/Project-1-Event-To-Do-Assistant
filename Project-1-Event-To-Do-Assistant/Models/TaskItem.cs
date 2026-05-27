namespace EventTodoAssistant.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ShortId => Id.ToString().Substring(0, 3);


        public string Title { get; set; } = "";
        public DateTime DueDate { get; set; }
        public string Project { get; set; } = "";
        public string Priority { get; set; } = "Medium";
        public List<string> Tags { get; set; } = new List<string>();
        public string Description { get; set; } = "";

        public bool IsCompleted { get; set; }
        public bool IsOverdue => !IsCompleted && DueDate.Date < DateTime.Now.Date;

        // ===== Update Methods =====

        public void UpdateTitle(string newTitle) => Title = newTitle;

        public void UpdateDate(DateTime newDate) => DueDate = newDate;

        public void UpdateProject(string newProject) => Project = newProject;

        public void UpdatePriority(string newPriority) => Priority = newPriority;

        public void UpdateTags(List<string> newTags) => Tags = newTags;

        public void UpdateDescription(string newDescription) => Description = newDescription;

        public void MarkCompleted() => IsCompleted = true;

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

    }
}
