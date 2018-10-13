using System.Web.Http;
using System.Web.Http.Cors;
using TaskManager.API.Helper;
using TaskManager.BusinessLayer;
using TaskManager.BusinessLayer.BusinessEntities;

namespace TaskManager.API.Controllers
{
    [EnableCors("*", "*", "*")]
    [TaskExceptionFilter]
    public class TaskManagerController : ApiController
    {
        private readonly ITaskService _taskService;

        /// <summary>
        /// Construtor
        /// </summary>
        public TaskManagerController()
        {
            _taskService = new TaskService();
        }

        public TaskManagerController(ITaskService service)
        {
            _taskService = service;
        }

        /// <summary>
        /// Get all tasks.
        /// </summary>
        /// <returns></returns>
        [Route("TaskManager/GetAll")]
        public IHttpActionResult Get()
        {
            return Ok(_taskService.GetAllTasks());
        }

        /// <summary>
        /// Get Task by Id parameter.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("TaskManager/GetTaskById/{id}")]
        public IHttpActionResult Get(int id)
        {
            return Ok(_taskService.GetTaskById(id));
        }

        /// <summary>
        /// Create Task entity.
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        [Route("TaskManager/CreateTask")]
        public IHttpActionResult Post([FromBody]TaskEntity taskEntity)
        {
            return Ok(_taskService.CreateTask(taskEntity));
        }

        /// <summary>
        /// Update task entity.
        /// </summary>
        /// <param name="taskEntity"></param>
        /// <returns></returns>
        [Route("TaskManager/UpdateTask")]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Put([FromBody]TaskEntity taskEntity)
        {
            if (taskEntity.TaskId > 0)
            {
                return Ok(_taskService.UpdateTask(taskEntity));
            }
            return Ok(false);
        }

        /// <summary>
        /// Delete selected task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("TaskManager/DeleteTask/{id}")]
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Delete(int id)
        {
            return Ok(_taskService.DeleteTask(id));
        }

        /// <summary>
        /// End selected task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("TaskManager/EndTask/{id}")]
        [EnableCors("*","*","*")]
        public IHttpActionResult EndTask(int id)
        {
            return Ok(_taskService.EndTask(id));
        }
    }
}
