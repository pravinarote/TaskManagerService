using Moq;
using NBench;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.API.Controllers;
using TaskManager.BusinessLayer;
using TaskManager.BusinessLayer.BusinessEntities;

namespace TaskManager.PerformanceTests
{
    public class ServicePerformanceTests : PerformanceTestSuite<ServicePerformanceTests>
    {
        private Counter _counter;
        readonly TaskManagerController controller = null;

        public ServicePerformanceTests()
        {
            var taskService = this.Configure();
            controller = new TaskManagerController(taskService.Object);
        }

        [PerfSetup]
        public void Setup(BenchmarkContext context)
        {
            _counter = context.GetCounter("TaskServiceCounter");
        }


        [CounterThroughputAssertion("TaskServiceCounter", MustBe.GreaterThan, 10000.0d)]
        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        [CounterTotalAssertion("TaskServiceCounter", MustBe.GreaterThan, 10000.0d)]
        [CounterMeasurement("TaskServiceCounter")]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void Benchmark_Performance_GetAllTasks()
        {
            var result = controller.Get();
            _counter.Increment();
        }

        [CounterThroughputAssertion("TaskServiceCounter", MustBe.GreaterThan, 50000.0d)]
        [PerfBenchmark(NumberOfIterations = 10, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        [CounterTotalAssertion("TaskServiceCounter", MustBe.GreaterThan, 50000.0d)]
        [CounterMeasurement("TaskServiceCounter")]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void Benchmark_Performance_GetTaskById()
        {
            var result = controller.Get(2);
            _counter.Increment();
        }

        [CounterThroughputAssertion("TaskServiceCounter", MustBe.GreaterThan, 50000.0d)]
        [PerfBenchmark(NumberOfIterations = 3, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        [CounterTotalAssertion("TaskServiceCounter", MustBe.GreaterThan, 50000.0d)]
        [CounterMeasurement("TaskServiceCounter")]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void Benchmark_Performance_CreateNewTask()
        {
            var taskToBeCreated = GetNewTaskEntity();
            var result = controller.Post(taskToBeCreated);
            _counter.Increment();
        }

        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        [CounterMeasurement("TaskServiceCounter")]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void Benchmark_Performance_UpdateTask()
        {
            var taskToBeUpdated = TaskEntityList.FirstOrDefault(x => x.TaskId == 2);
            var result = controller.Put(taskToBeUpdated);
        }

        [CounterThroughputAssertion("TaskServiceCounter", MustBe.GreaterThan, 100000.0d)]
        [PerfBenchmark(NumberOfIterations = 1, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        [CounterTotalAssertion("TaskServiceCounter", MustBe.GreaterThan, 100000.0d)]
        [CounterMeasurement("TaskServiceCounter")]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void Benchmark_Performance_DeleteTask()
        {
            var result = controller.Delete(2);
            _counter.Increment();
        }

        [CounterThroughputAssertion("TaskServiceCounter", MustBe.GreaterThan, 50000.0d)]
        [PerfBenchmark(NumberOfIterations = 10, RunMode = RunMode.Throughput, TestMode = TestMode.Test, SkipWarmups = true)]
        [ElapsedTimeAssertion(MaxTimeMilliseconds = 5000)]
        [CounterTotalAssertion("TaskServiceCounter", MustBe.GreaterThan, 50000.0d)]
        [CounterMeasurement("TaskServiceCounter")]
        [GcMeasurement(GcMetric.TotalCollections, GcGeneration.AllGc)]
        public void Benchmark_Performance_EndTask()
        {
            var result = controller.EndTask(2);
            _counter.Increment();
        }

        [PerfCleanup]
        public void Cleanup()
        {
            // does nothing
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
