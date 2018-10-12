using System.Collections.Generic;
using System.Transactions;
using TaskManager.BusinessLayer.BusinessEntities;
using TaskManager.DataLayer.UnitOfWork;
using TaskManager.BusinessLayer.Mapper;
using System.Linq;

namespace TaskManager.BusinessLayer
{
    public class TaskService : ITaskService
    {
        /// <summary>
        /// Unit of work for database operation.
        /// </summary>
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// TaskService constructor.
        /// </summary>
        public TaskService()
        {
            _unitOfWork = new UnitOfWork();
        }

        /// <summary>
        /// Create new task entity.
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public int CreateTask(TaskEntity taskEntity)
        {
            using (var scope = new TransactionScope())
            {
                var task = taskEntity.Map();

                _unitOfWork.TaskRepository.Insert(task);
                _unitOfWork.Save();
                scope.Complete();
                return task.TaskId;
            }
        }

        /// <summary>
        /// Delete task entity.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool DeleteTask(int taskId)
        {
            var success = false;
            if (taskId > 0)
            {
                using (var scope = new TransactionScope())
                {
                    var task = _unitOfWork.TaskRepository.GetByID(taskId);
                    if (task != null)
                    {
                        _unitOfWork.TaskRepository.Delete(task);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// Get all task entities.
        /// </summary>
        /// <returns></returns>
        public List<TaskEntity> GetAllTasks()
        {
            var tasks = _unitOfWork.TaskRepository.GetAll().ToList();
            return tasks.Map();
        }

        /// <summary>
        /// Get task by taskid.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public TaskEntity GetTaskById(int taskId)
        {
            var task = _unitOfWork.TaskRepository.GetByTaskId(taskId);
            if (task != null)
            {
                return task.Map();
            }
            return null;
        }

        /// <summary>
        /// Update task entity.
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        public bool UpdateTask(TaskEntity taskEntity)
        {
            var success = false;
            if (taskEntity != null)
            {
                using (var scope = new TransactionScope())
                {
                    var task = _unitOfWork.TaskRepository.GetByID(taskEntity.TaskId);
                    if (task != null)
                    {
                        task = taskEntity.Map(task);
                        _unitOfWork.TaskRepository.Update(task);
                        _unitOfWork.Save();
                        scope.Complete();
                        success = true;
                    }
                }
            }
            return success;
        }

        /// <summary>
        /// End currently selected task.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool EndTask(int taskId)
        {
            var success = false;
            using (var scope = new TransactionScope())
            {
                var task = _unitOfWork.TaskRepository.GetByID(taskId);
                if (task != null)
                {
                    task.IsTaskEnded = true;
                    _unitOfWork.TaskRepository.Update(task);
                    _unitOfWork.Save();
                    scope.Complete();
                    success = true;
                }
            }
            return success;
        }
    }
}
