using System;

namespace TodoApp.Domain
{
    public enum KanbanState { New, InProgress, Done }

    public class Todo
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public KanbanState KanbanState { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsOverdue => (DueDate ?? DateTime.MaxValue) < DateTime.Now;
    }
}