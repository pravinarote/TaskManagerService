using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using TaskManager.API.Controllers;
using TaskManager.BusinessLayer;
using TaskManager.BusinessLayer.BusinessEntities;

namespace TaskManager.Tests
{
    [TestFixture]
    public class TaskManagerControllerTests
    {
        readonly TaskManagerController controller = null;

        public TaskManagerControllerTests()
        {
            var taskService = this.Configure();
            controller = new TaskManagerController(taskService.Object);
        }

        [Test]
        public void When_GetAllTasks_Then_ShouldReturnAllTasks()
        {
            var result = controller.Get() as OkNegotiatedContentResult<List<TaskEntity>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.Count, 3);
        }

        [Test]
        public void When_GetTaskById_Then_VerifyTaskDetails()
        {
            var result = controller.Get(2) as OkNegotiatedContentResult<TaskEntity>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.TaskId, 2);
            Assert.AreEqual(result.Content.TaskName, "Task 2");
            Assert.AreEqual(result.Content.Priority, 10);
        }

        [Test]
        public void When_CreateNewTask_Then_VerifyTaskInserted()
        {
            var taskToBeCreated = GetNewTaskEntity();
            var result = controller.Post(taskToBeCreated) as OkNegotiatedContentResult<int>;

            Assert.NotNull(result);
            Assert.AreEqual(result.Content, 101);
        }

        [Test]
        public void When_UpdateTask_Then_VerifyTaskUpdated()
        {
            var taskToBeUpdated = controller.Get(2) as OkNegotiatedContentResult<TaskEntity>;
            var result = controller.Put(taskToBeUpdated.Content) as OkNegotiatedContentResult<bool>;
            Assert.NotNull(result);
            Assert.True(result.Content);
        }

        [Test]
        public void When_DeleteTask_Then_VerifyTaskDeleted()
        {
            var result = controller.Delete(3) as OkNegotiatedContentResult<bool>;
            Assert.NotNull(result);
            Assert.True(result.Content);
        }

        [Test]
        public void When_EndTask_Then_VerifyTaskEnded()
        {
            var result = controller.EndTask(3) as OkNegotiatedContentResult<bool>;
            Assert.NotNull(result);
            Assert.True(result.Content);
        }

        private Mock<ITaskService> Configure()
        {
            Mock<ITaskService> taskService = new Mock<ITaskService>();

            taskService.Setup(x => x.GetAllTasks()).Returns(TaskEntityList);

            taskService.Setup(mr => mr.GetTaskById(
                It.IsAny<int>())).Returns((int i) => TaskEntityList.Where(
                x => x.TaskId == i).Single());

            taskService.Setup(mr => mr.CreateTask(It.IsAny<TaskEntity>())).Returns(
                (TaskEntity target) =>
                {
                    target.TaskId = 101;
                    return target.TaskId;
                });

            taskService.Setup(mr => mr.UpdateTask(It.IsAny<TaskEntity>())).Returns(true);

            taskService.Setup(x => x.DeleteTask(It.IsAny<int>())).Returns(true);

            taskService.Setup(x => x.EndTask(It.IsAny<int>())).Returns(true);

            return taskService;
        }

        public static TaskEntity GetNewTaskEntity()
        {
            return new TaskEntity()
            {
                TaskName = "Task 3",
                ParentTaskId = 1,
                ParentTaskName = "Task 1",
                Priority = 15,
                StartDate = new DateTime(2010, 10, 05),
                EndDate = new DateTime(2019, 10, 05),
            };
        }

        public static List<TaskEntity> TaskEntityList = new List<TaskEntity>()
        {
            new TaskEntity()
            {
                TaskId =1,
                TaskName = "Task 1",
                ParentTaskId = null,
                Priority = 5,
                StartDate = new DateTime(2010,10,05),
                EndDate = new DateTime(2019, 10, 05),
            },
            new TaskEntity()
            {
                TaskId =2,
                TaskName = "Task 2",
                ParentTaskId = null,
                Priority = 10,
                StartDate = new DateTime(2010,10,05),
                EndDate = new DateTime(2019, 10, 05),
            },
            new TaskEntity()
            {
                TaskId =3,
                TaskName = "Task 3",
                ParentTaskId = 1,
                ParentTaskName = "Task 1",
                Priority = 15,
                StartDate = new DateTime(2010,10,05),
                EndDate = new DateTime(2019, 10, 05),
            }
        };
    }
}
