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
    public class DesksRequestServiceTests
    {
        private AuthToken _token;
        private DesksRequestService _service;
        public DesksRequestServiceTests()
        {
            _token = new UserRequestService().GetToken("admin", "qwerty123");
            _service = new DesksRequestService();
        }
        [TestMethod()]
        public void GetAllDesksTest()
        {
            var desks = _service.GetAllDesks(_token);
            Console.WriteLine(desks.Count);
            Assert.AreNotEqual(Array.Empty<DeskModel>(), desks);
        }

        [TestMethod()]
        public void GetDesksByIdTest()
        {
            var desk = _service.GetDesksById(_token, 2);

            Assert.AreNotEqual(null, desk);
        }

        [TestMethod()]
        public void GetDesksByProjectTest()
        {
            var desks = _service.GetDesksByProject(_token, 39);
            Assert.AreEqual(3, desks.Count);
        }

        [TestMethod()]
        public void CreateDeskTest()
        {
            DeskModel desk = new DeskModel("Тестовый проект", "Доска созданная из VisualStudio", true, new string[] { "В процессе", "Готовые" });
            desk.ProjectId = 38;
            desk.AdminId = 1;
            var result = _service.CreateDesk(_token, desk);
            Assert.AreEqual(HttpStatusCode.OK, result);



        }

        [TestMethod()]
        public void UpdateDeskTest()
        {
            DeskModel desk = new DeskModel("Тестовый проектМ2", "Доска созданная из VisualStudio  V2/0", true, new string[] { "В процессе", "Готовые", "Не готовые" });
            desk.ProjectId = 38;
            desk.AdminId = 1;
            desk.Id = 4;
            var result = _service.UpdateDesk(_token, desk);
            Assert.AreEqual(HttpStatusCode.OK, result);
        }

        [TestMethod()]
        public void DelettDesksByIdTest()
        {
            var result = _service.DelettDesksById(_token, 4);
            Assert.AreEqual(HttpStatusCode.OK,result);
        }
    }
}