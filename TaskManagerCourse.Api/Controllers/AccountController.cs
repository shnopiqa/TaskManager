using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // Контроллер который выдает права доступа на использование запросов
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _userservice; // Создаем поле после создания UsersService.cs
        public AccountController(ApplicationContext db)
        {
            _db = db;
            _userservice = new UsersService(db);
        }
        [HttpGet("info")]
        public IActionResult GetCurrentUserInfo() 
        {
            string username = HttpContext.User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == username);
            if (user != null)
                return Ok(user.ToDto());
            return NotFound();
        }
        // Для того чтобы пользоваться токеном надо установить библиотеку JWT Bearer
        // Далее перейти в Startup.cs
        [HttpPost("token")] // Из запроса необходимо получить данные по пользователю
        public IActionResult GetToken() 
        {
            // Далее создание класса UsersService.cs 
            var userDate = _userservice.GetUserLogiPassFromBasicAuth(Request);// передаем запрос 
            var login = userDate.Item1; // Логин 
            var pass = userDate.Item2; // Пароль
            var identity = _userservice.GetIdentity(login, pass);
            var now = DateTime.Now;

            // Формирование токена
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };
            return Ok(response);
        }
        [Authorize]
        [HttpPatch("update")]
        public IActionResult updateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                string userName = HttpContext.User.Identity.Name;
                User UserForUpdate = _db.Users.FirstOrDefault(u => u.Email == userName);
                if (UserForUpdate != null)
                { 
                    UserForUpdate.FirstName = userModel.FirstName;
                    UserForUpdate.LastName = userModel.LastName;
                    UserForUpdate.Password = userModel.Password;
                    UserForUpdate.Phone = userModel.Phone;
                    UserForUpdate.Photo = userModel.Photo;
                    UserForUpdate.Email = userModel.Email;
                    _db.Users.Update(UserForUpdate);
                    _db.SaveChanges();
                    return Ok();

                }
                return NotFound();
            }
            return BadRequest();
        }

    }
}
