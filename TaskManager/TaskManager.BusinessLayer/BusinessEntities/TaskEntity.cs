using System;

namespace TaskManager.BusinessLayer.BusinessEntities
{
    public class TaskEntity
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string ParentTaskName { get; set; }
        public int? ParentTaskId { get; set; }
        public int Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsTaskEnded { get; set; }
    }
}
