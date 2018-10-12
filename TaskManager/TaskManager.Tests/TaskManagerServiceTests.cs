using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.BusinessLayer;
using TaskManager.BusinessLayer.BusinessEntities;

namespace TaskManager.Tests
{
    [TestFixture]
    public class TaskManagerServiceTests
    {
        readonly Mock<ITaskService> _mockTaskService;

        public TaskManagerServiceTests()
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

            _mockTaskService = taskService;
        }

        [Test]
        public void When_GetAllTasks_Then_VerifyResult()
        {
            var taskList = _mockTaskService.Object.GetAllTasks();

            Assert.NotNull(taskList);
            Assert.AreEqual(taskList.Count, 3);
        }

        [Test]
        public void When_GetTaskById_Then_VerifyTaskDetails()
        {
            var taskEntity = _mockTaskService.Object.GetTaskById(2);

            Assert.AreEqual(taskEntity.TaskId, 2);
            Assert.AreEqual(taskEntity.TaskName, "Task 2");
            Assert.AreEqual(taskEntity.Priority, 10);
        }

        [Test]
        public void When_CreateNewTask_Then_VerifyTaskInserted()
        {
            var taskToBeCreated = GetNewTaskEntity();
            var newTaskId = _mockTaskService.Object.CreateTask(taskToBeCreated);

            Assert.NotNull(newTaskId);
            Assert.AreEqual(newTaskId, 101);
        }

        [Test]
        public void When_UpdateTask_Then_VerifyTaskUpdated()
        {
            var taskToBeUpdated = _mockTaskService.Object.GetTaskById(2);

            var isTaskUpdated = _mockTaskService.Object.UpdateTask(taskToBeUpdated);

            Assert.True(isTaskUpdated);
        }

        [Test]
        public void When_DeleteTask_Then_VerifyTaskDeleted()
        {
            var isTaskDeleted = _mockTaskService.Object.DeleteTask(3);
            Assert.True(isTaskDeleted);
        }

        [Test]
        public void When_EndTask_Then_VerifyTaskEnded()
        {
            var isTaskEnded = _mockTaskService.Object.EndTask(3);
            Assert.True(isTaskEnded);
        }

        #region Test data

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

        #endregion
    }
}
