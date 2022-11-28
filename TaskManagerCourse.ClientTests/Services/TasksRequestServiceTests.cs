using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerCourse.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Common.Models;
using System.Net;

namespace TaskManagerCourse.Client.Services.Tests
{
    [TestClass()]
    public class TasksRequestServiceTests
    {
        private AuthToken _token;
        private TasksRequestService _service;
        public TasksRequestServiceTests()
        {
            _token = new UserRequestService().GetToken("admin", "qwerty123");
            _service = new TasksRequestService();
        }
        [TestMethod()]
        public void GetAllTasksTest()
        {
            var tasks = _service.GetAllTasks(_token);
            Console.WriteLine(tasks.Count);
            Assert.AreNotEqual(0, tasks);
        }

        [TestMethod()]
        public void GetTasksByIdTest()
        {
            var task = _service.GetTasksById(_token, 2);

            Assert.AreNotEqual(null, task);
        }

        [TestMethod()]
        public void GetTasksByDeskTest()
        {
            var tasks = _service.GetTasksByDesk(_token, 1);
            Assert.AreNotEqual(0, tasks.Count);
        }

        [TestMethod()]
        public void CreateTaskTest()
        {
            var task = new TaskModel("Задача 1", "Задача из визуал студио", DateTime.Now, DateTime.Now, "New");
            task.DeskId = 2;
            task.ExecutorId = 1;
            var result = _service.CreateTask(_token, task);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateTaskTest()
        {
            var task = new TaskModel("Задача 2.0", "Задача из визуал студио", DateTime.Now, DateTime.Now, "New");
            task.Id = 5;
            task.ExecutorId = 4;
            var result = _service.UpdateTask(_token, task);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DelettTasksByIdTest()
        {
            var result = _service.DelettTasksById(_token, 5);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }
}