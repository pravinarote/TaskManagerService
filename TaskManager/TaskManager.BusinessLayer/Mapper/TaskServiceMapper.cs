using System.Collections.Generic;
using TaskManager.BusinessLayer.BusinessEntities;
using TaskManager.Entities;

namespace TaskManager.BusinessLayer.Mapper
{
    /// <summary>
    /// This class provides mapping mechanism between model and dtos.
    /// </summary>
    public static class TaskServiceMapper
    {
        public static Task Map(this TaskEntity taskEntity, Task task)
        {
            task.TaskName = taskEntity.TaskName;
            task.Priority = taskEntity.Priority;
            task.StartDate = taskEntity.StartDate;
            task.EndDate = taskEntity.EndDate;
            task.IsTaskEnded = taskEntity.IsTaskEnded;
            task.ParentTaskId = taskEntity.ParentTaskId;
            return task;
        }

        public static Task Map(this TaskEntity taskEntity)
        {
            var task = new Task();

            task.TaskId = taskEntity.TaskId;
            task.TaskName = taskEntity.TaskName;
            task.Priority = taskEntity.Priority;
            task.StartDate = taskEntity.StartDate;
            task.EndDate = taskEntity.EndDate;
            task.IsTaskEnded = taskEntity.IsTaskEnded;
            task.ParentTaskId = taskEntity.ParentTaskId;

            return task;
        }

        public static TaskEntity Map(this Task task)
        {
            var taskEntity = new TaskEntity();

            taskEntity.TaskId = task.TaskId;
            taskEntity.TaskName = task.TaskName;
            if (task.ParentTask != null)
            {
                taskEntity.ParentTaskId = task.ParentTask.TaskId;
                taskEntity.ParentTaskName = task.ParentTask.TaskName;
            }
            taskEntity.Priority = task.Priority;
            taskEntity.StartDate = task.StartDate;
            taskEntity.EndDate = task.EndDate;
            taskEntity.IsTaskEnded = task.IsTaskEnded;
            return taskEntity;
        }

        public static List<TaskEntity> Map(this List<Task> taskList)
        {
            var taskEntityList = new List<TaskEntity>();

            taskList.ForEach(task =>
            {
                var taskEntity = new TaskEntity();

                taskEntity.TaskId = task.TaskId;
                taskEntity.TaskName = task.TaskName;
                if (task.ParentTask != null)
                {
                    taskEntity.ParentTaskId = task.ParentTask.TaskId;
                    taskEntity.ParentTaskName = task.ParentTask.TaskName;
                }
                taskEntity.Priority = task.Priority;
                taskEntity.StartDate = task.StartDate;
                taskEntity.EndDate = task.EndDate;
                taskEntity.IsTaskEnded = task.IsTaskEnded;

                taskEntityList.Add(taskEntity);
            });

            return taskEntityList;
        }

    }
}
