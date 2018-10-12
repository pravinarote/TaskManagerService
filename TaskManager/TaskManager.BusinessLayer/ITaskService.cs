using System.Collections.Generic;
using TaskManager.BusinessLayer.BusinessEntities;

namespace TaskManager.BusinessLayer
{
    public interface ITaskService
    {
        int CreateTask(TaskEntity taskEntity);
        bool DeleteTask(int taskId);
        bool EndTask(int taskId);
        List<TaskEntity> GetAllTasks();
        TaskEntity GetTaskById(int taskId);
        bool UpdateTask(TaskEntity taskEntity);
    }
}
