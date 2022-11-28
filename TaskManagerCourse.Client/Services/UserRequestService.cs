using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class UserRequestService : CommonRequestService
    { 
        public AuthToken GetToken(string userName, string paswword)
        {

            string url = $"{HOST}account/token";
            string resultStr = GetDataByUrl(HttpMethod.Post, url, null, userName, paswword);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(resultStr);
            return token;
        }
        public HttpStatusCode CreateUser(AuthToken token, UserModel user) 
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendhDataByUrl(HttpMethod.Post, _usersContollerUrl, token, userJson);
            return result;
        }
        public UserModel GetCurrentUser(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, HOST + "account/info", token);
            UserModel user = JsonConvert.DeserializeObject<UserModel>(response);
            return user;
        }
        public UserModel GetUserById(AuthToken token, int? userId)
        {
            string response = GetDataByUrl(HttpMethod.Get, _usersContollerUrl + $"/{userId}", token);
            UserModel user = JsonConvert.DeserializeObject<UserModel>(response);
            return user;
        }
        public List<UserModel> GetAllUsers(AuthToken token) 
        {
            string response = GetDataByUrl(HttpMethod.Get, _usersContollerUrl, token);
            List<UserModel> users = JsonConvert.DeserializeObject<List<UserModel>>(response);
            return users;
        }
        public HttpStatusCode DeleteUser(AuthToken token, int userId)
        {
            
            var result = DeleteDataByUrl(_usersContollerUrl + $"/{userId}", token);
            return result;
        }
        public HttpStatusCode CreateMultipleUsers(AuthToken token, List<UserModel> users)
        {
            string userJson = JsonConvert.SerializeObject(users);
            var result = SendhDataByUrl(HttpMethod.Post, _usersContollerUrl + "/all", token, userJson);
            return result;
        }
        public HttpStatusCode UpdateUser(AuthToken token, UserModel user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendhDataByUrl(HttpMethod.Patch, _usersContollerUrl + $"/{user.Id}", token, userJson);
            return result;
        }

    }
}
