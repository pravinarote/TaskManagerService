using System.Collections.Generic;
using TaskManager.Entities;

namespace TaskManager.DataLayer.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<Task> GetAll();

        Task GetByID(int id);

        Task GetByTaskId(int id);

        int Insert(Task task);

        bool Delete(int taskId);

        void Delete(Task entityToDelete);

        Task Update(Task entityToUpdate);
    }
}
