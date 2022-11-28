using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaskManagerCourse.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Common.Models;
using TaskManagerCourse.Client.Models;
using System.Net;

namespace TaskManagerCourse.Client.Services.Tests
{

    [TestClass()]
    public class ProjectsRequestServiceTests
    {
        private AuthToken _token;
        private ProjectsRequestService _service;
        public ProjectsRequestServiceTests()
        {
            _token = new UserRequestService().GetToken("admin", "qwerty123");
            _service = new ProjectsRequestService();
        }
        [TestMethod()]
        public void GetAllProjectTest()
        {

            var projects = _service.GetAllProjects(_token);
            Console.WriteLine(projects.Count);
            Assert.AreNotEqual(Array.Empty<ProjectModel>(), projects);
        }
        [TestMethod()]
        public void GetProjectsByIdTest()
        {
            var project = _service.GetProjectById(_token, 38);

            Assert.AreNotEqual(null, project);
        }

        [TestMethod()]
        public void CreateProjectTest()
        {
            ProjectModel project = new ProjectModel("Тестовый проект", "Проект созданный из VisualStudio", ProjectStatus.InProgress);
            project.AdminId = 1;
            var result = _service.CreateProject(_token, project);


            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void UpdateProjectTest()
        {
            ProjectModel project = new ProjectModel("Тестовый проект V 2.O", "Проект созданный из VisualStudio V 2.O", ProjectStatus.Suspended);
            project.Id = 44;
            var result = _service.UpdateProject(_token, project);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DeleteProjectTest()
        {
            var result = _service.DeleteProject(_token, 45);

        }

        [TestMethod()]
        public void AddUsersToProjectTest()
        {
            List<int> usersIds = new List<int>() { 16, 17, 18 };
            var result = _service.AddUsersToProject(_token, 44, usersIds);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void RemoveUsersFromProjectTest()
        {
            List<int> usersIds = new List<int>() { 16, 17 };
            var result = _service.RemoveUsersFromProject(_token, 44, usersIds);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }
    }

}