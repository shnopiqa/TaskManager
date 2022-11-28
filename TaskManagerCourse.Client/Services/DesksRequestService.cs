using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class DesksRequestService : CommonRequestService
    {
        private string _deskControllerUrl = HOST + "desks";
        public List<DeskModel> GetAllDesks(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _deskControllerUrl, token);
            List<DeskModel> desks = JsonConvert.DeserializeObject<List<DeskModel>>(response);
            return desks;
        }
        public DeskModel GetDesksById(AuthToken token, int deskId)
        {
            var response = GetDataByUrl(HttpMethod.Get, _deskControllerUrl + $"/{deskId}", token);
            DeskModel desk = JsonConvert.DeserializeObject<DeskModel>(response);
            return desk;
        }
        public List<DeskModel> GetDesksByProject(AuthToken token, int projectId)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("projectId", projectId.ToString());
            string response = GetDataByUrl(HttpMethod.Get, _deskControllerUrl + "/project", token, null, null, parameters);
            List<DeskModel> desks = JsonConvert.DeserializeObject<List<DeskModel>>(response);
            return desks;
        }

        public HttpStatusCode CreateDesk(AuthToken token, DeskModel desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = SendhDataByUrl(HttpMethod.Post, _deskControllerUrl, token, deskJson);
            return result;
        }
        public HttpStatusCode UpdateDesk(AuthToken token, DeskModel desk)
        {
            string deskJson = JsonConvert.SerializeObject(desk);
            var result = SendhDataByUrl(HttpMethod.Patch, _deskControllerUrl + $"/{desk.Id}", token, deskJson);
            return result;
        }
        public HttpStatusCode DelettDesksById(AuthToken token, int deskId)
        {
            var result = DeleteDataByUrl(_deskControllerUrl + $"/{deskId}", token);
           
            return result;
        }
    }
}
