using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.DataLayer.Repository;
using TaskManager.Entities;

namespace TaskManager.Tests
{
    [TestFixture]
    public class TaskManagerRepositoryTests
    {
        readonly ITaskRepository _repository;

        public TaskManagerRepositoryTests()
        {
            Mock<ITaskRepository> repository = new Mock<ITaskRepository>();

            var tasks = Tasks;
            repository.Setup(x => x.GetAll()).Returns(tasks);

            repository.Setup(mr => mr.GetByID(
                It.IsAny<int>())).Returns((int i) => tasks.Where(
                x => x.TaskId == i).Single());

            repository.Setup(mr => mr.Delete(
                It.IsAny<int>())).Returns(true);

            repository.Setup(mr => mr.Insert(It.IsAny<Task>())).Returns(
                (Task target) =>
                {
                    target.TaskId = 1001;
                    return target.TaskId;
                });

            repository.Setup(mr => mr.Update(It.IsAny<Task>())).Returns(
                (Task target) =>
                {
                    target.TaskName = "New task Updated";
                    return target;
                });

            _repository = repository.Object;
        }

        [Test]
        public void When_GetAll_VerifyResults()
        {
            var taskList = _repository.GetAll();

            Assert.IsNotNull(taskList);
            Assert.AreEqual(taskList.Count(), 3);
        }

        [Test]
        public void When_GetById_Then_VerifyTaskDetails()
        {
            var searchedTask = _repository.GetByID(2);

            Assert.AreEqual(searchedTask.TaskId, 2);
            Assert.AreEqual(searchedTask.TaskName, "Task 2");
            Assert.AreEqual(searchedTask.Priority, 5);
        }

        [Test]
        public void When_InsertNewTask_Then_VerifyTaskInserted()
        {
            var taskToBeInserted = GetNewTaskToBeInserted();
            var insertedTaskId  = _repository.Insert(taskToBeInserted);

            Assert.AreEqual(insertedTaskId, 1001);
        }

        [Test]
        public void When_UpdateTask_Then_VerifyTaskUpdated()
        {
            var searchedTask = _repository.GetByID(2);
            var updatedTask = _repository.Update(searchedTask);

            Assert.AreEqual(updatedTask.TaskName, "New task Updated");
            Assert.AreEqual(updatedTask.TaskId, 2);
        }

        [Test]
        public void When_DeleteTask_Then_VerifyTaskDeleted()
        {
            var isTaskDeleted = _repository.Delete(3);
            Assert.True(isTaskDeleted);
        }

        public static Task GetNewTaskToBeInserted()
        {
            return new Task()
            {
                TaskName = "Task 15",
                Priority = 13,
                StartDate = new DateTime(2009, 10, 01),
                EndDate = new DateTime(2019, 10, 01)
            };
        }

        public static List<Task> Tasks = new List<Task>()
        {
            new Task()
            {
                TaskId = 1,
                TaskName = "Task 1",
                Priority = 3,
                StartDate = new DateTime(2009,10,01),
                EndDate = new DateTime(2019,10,01)
            },
            new Task()
            {
                TaskId = 2,
                TaskName = "Task 2",
                Priority = 5,
                StartDate = new DateTime(2009,10,01),
                EndDate = new DateTime(2019,10,01)
            },
            new Task()
            {
                TaskId = 3,
                TaskName = "Task 3",
                Priority = 10,
                StartDate = new DateTime(2009,10,01),
                EndDate = new DateTime(2019,10,01)
            }
        };
    }
}
