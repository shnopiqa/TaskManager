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
    public class ProjectsRequestService : CommonRequestService
    {
        private string _projectControllerUrl = HOST + "projects";
        public List<ProjectModel> GetAllProjects(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _projectControllerUrl, token);
            List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(response);
            return projects;
        }
        public ProjectModel GetProjectById(AuthToken token, int projectId)
        {
            var response = GetDataByUrl(HttpMethod.Get,_projectControllerUrl + $"/{projectId}", token);
            ProjectModel project = JsonConvert.DeserializeObject<ProjectModel>(response);
            return project;
        }
        public HttpStatusCode CreateProject(AuthToken token, ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = SendhDataByUrl(HttpMethod.Post, _projectControllerUrl, token, projectJson);
            return result;
        }
        public HttpStatusCode UpdateProject(AuthToken token, ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            var result = SendhDataByUrl(HttpMethod.Patch, _projectControllerUrl + $"/{project.Id}", token, projectJson);
            return result;
        }
        public HttpStatusCode DeleteProject(AuthToken token, int projectId)
        {

            var result = DeleteDataByUrl(_projectControllerUrl + $"/{projectId}", token);
            return result;
        }
        public HttpStatusCode AddUsersToProject(AuthToken token, int projectId, List<int> userssIds)
        {
            string userIdsJson = JsonConvert.SerializeObject(userssIds);
            var result = SendhDataByUrl(HttpMethod.Patch, _projectControllerUrl + $"/{projectId}/users", token, userIdsJson);
            return result;
        }
        public HttpStatusCode RemoveUsersFromProject(AuthToken token, int projectId, List<int> userssIds)
        {
            string userIdsJson = JsonConvert.SerializeObject(userssIds);
            var result = SendhDataByUrl(HttpMethod.Patch, _projectControllerUrl + $"/{projectId}/users/remove", token, userIdsJson);
            return result;
        }
    }
    
}
