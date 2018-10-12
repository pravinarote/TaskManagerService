using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TaskManager.Entities;

namespace TaskManager.DataLayer.Repository
{
    public class TaskRepository : ITaskRepository
    {
        internal TaskManagerContext Context;
        internal DbSet<Task> DbSet;

        public TaskRepository(TaskManagerContext context)
        {
            Context = context;
            this.DbSet = context.Set<Task>();
        }

        public bool Delete(int taskId)
        {
            var entityToDelete = DbSet.Find(taskId);
            Delete(entityToDelete);
            return true;
        }

        public void Delete(Task entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public IEnumerable<Entities.Task> GetAll()
        {
            IQueryable<Task> query = DbSet;
            return query.ToList();
        }

        public Task GetByID(int id)
        {
            return DbSet.Find(id);
        }

        public Task GetByTaskId(int id)
        {
            return Context.Tasks.Include("ParentTask").FirstOrDefault(x => x.TaskId == (int)id);
        }

        public int Insert(Entities.Task task)
        {
            DbSet.Add(task);
            Context.SaveChanges();
            return task.TaskId;
        }

        public Task Update(Entities.Task entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
            Context.SaveChanges();
            return entityToUpdate;
        }
    }
}
