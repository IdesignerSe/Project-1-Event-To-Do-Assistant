namespace EventTodoAssistant.Models
{
    
    public class TaskItem
    {
        public string Title { get; set; } = "";
        public DateTime DueDate { get; set; }
        public string Project { get; set; } = "";

        public string Category { get; set; } = "General";

        public string Priority { get; set; } = "Medium";
        
        public bool IsCompleted { get; set; }
        public bool IsOverdue => !IsCompleted && DueDate.Date < DateTime.Now.Date;

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

        string ReadCategory(string message)
        
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                    return input.Trim();

                Console.WriteLine("Category cannot be empty.");
    }
}

        public void UpdateCategory(string newCategory) => Category = newCategory;
    }
}
