using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Api.Models.Abstractions;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models.Services
{
    public class UsersService : AbstractionService, ICommonService<UserModel>
    {
        // идентифицируем базу данных 
        private readonly ApplicationContext _db;
        public UsersService(ApplicationContext db)
        {
            _db = db;
        }
        // Метод получения логина и пароля из базовой аутентификации 
        public Tuple<string, string> GetUserLogiPassFromBasicAuth(HttpRequest request) 
        {
            string userName = "";
            string userPass = "";
            // Получаем хедер из реквеста по ключу Authorization
            string authHeader = request.Headers["Authorization"].ToString();
            // Проверка не равен нулю и начинается с Basic
            if (authHeader != null && authHeader.StartsWith("Basic")) 
            {
                // Закодированные данные в которых убирается Basic 
                string encodedUserNamePass = authHeader.Replace("Basic ", "");
                // Раскодирование данных 
                var encoding = Encoding.GetEncoding("iso-8859-1");
                // массив который получим из Юзер нэйм и пароля 
                string[] namepassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(":");
                userName = namepassArray[0]; // Логин
                userPass = namepassArray[1]; // Пароль
            }
            return new Tuple<string, string>(userName, userPass); // Возвращаем кортеж 
        }
        // Получить юзера 
        public User GetUser(string login, string password) 
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
            return user;

        }
        // Переходим в AccountContoller
        public User GetUser(string login)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login);
            return user;

        }
        // Логика для получения токена
        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null)
            {
                currentUser.LastLoginDate = DateTime.Now;
                _db.Users.Update(currentUser);
                _db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Status.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        public bool Create(UserModel model) // Реализуется интерфейс ICommonService Create
        {
            try
            {
                User newUser = new User(model.FirstName, model.LastName, model.Email,
                        model.Password, model.Status, model.Phone, model.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(int id, UserModel model)
        {
            User UserForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            if (UserForUpdate != null)
            {
                return DoAction(delegate ()
                {
                    UserForUpdate.FirstName = model.FirstName;
                    UserForUpdate.LastName = model.LastName;
                    UserForUpdate.Password = model.Password;
                    UserForUpdate.Phone = model.Phone;
                    UserForUpdate.Photo = model.Photo;
                    UserForUpdate.Status = model.Status;
                    UserForUpdate.Email = model.Email;
                    _db.Users.Update(UserForUpdate);
                    _db.SaveChanges();
                });
            }
            return false;
        }

        public bool Delete(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return DoAction(delegate ()
                {
                    _db.Users.Remove(user);
                    _db.SaveChanges();
                });
            }
            return false;
            
        }
        public bool CreateMultipleUsers( List<UserModel> userModels)
        {
            return DoAction(delegate () // try cath провекри через делегат
            {
                var newUsers = userModels.Select(u => new User(u));
                _db.Users.AddRange(newUsers);
                _db.SaveChanges();
            });
            
        }

        public UserModel Get(int id)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            return userForUpdate?.ToDto();
        }
        public IEnumerable<UserModel> GetAllByIds(List<int> ids) 
        {
            foreach(int id in ids) 
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == id).ToDto();
                yield return user;
            }
            
        }
       
    }
}
