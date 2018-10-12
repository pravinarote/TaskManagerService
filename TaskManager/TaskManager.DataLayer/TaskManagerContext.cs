using System.Data.Entity;
using TaskManager.Entities;

namespace TaskManager.DataLayer
{
    /// <summary>
    /// TaskManager Context.
    /// </summary>
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext():base("TaskManagerConn")
        {
        }

        public DbSet<Task> Tasks { get; set; }
    }
}
