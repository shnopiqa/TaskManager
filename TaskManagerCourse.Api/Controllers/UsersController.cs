using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")] // атрибут как обращаться с фронтенда к бэкенду
    [ApiController]
    public class UsersController : ControllerBase // Контроллер наследуется от класса 
    {
        private readonly UsersService _usersService;// реализуем юзерсервис
        private readonly ApplicationContext _db; //Экземпляр класса для работы с базой данныз

        //Конструктор который принимает аргумент типа ApplicationContext
        public UsersController(ApplicationContext db)
        {
            _db = db; // Присваеваем приватному полю значение ApplicationContext
            _usersService = new UsersService(db);
        }


        // Тестовое API для проверки теста 
        [HttpGet("test")]
        [AllowAnonymous] // доступ без авторизации 
        public IActionResult TestApi() { return Ok("Сервак Работает" + " " + DateTime.Now); }

        [Authorize(Roles = "Admin")]// Атрибут для того чтобы запрос мог использовать только админ 
        [HttpPost] // Регистрация запроса через атрибут 
        //Создание Юзера
        // выносим создание юезра в userservis
        public IActionResult CreateUser([FromBody] UserModel userModel) //Получение из тела запроса необходимых данных
        {
            if(userModel != null) // Проверка на null 
            {
                bool result = _usersService.Create(userModel); // Вызываем метод который реализован в UserService
                return result ? Ok() : NotFound(); // если тру - то ок, если нет то NotFound
            }
            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        // Обновление данных в юзере, получаем юзера из базы данных, обновляем данные
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel) // примнемается UserModel, id 
        {
            if (userModel != null) 
            {
                bool result = _usersService.Update(id, userModel);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id) 
        {
            bool result = _usersService.Delete(id);
            return result ? Ok() : NotFound();
        }
        // Ассинхронное получение всех пользователей 
        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers() 
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("all")]
        public async Task<IActionResult> CreateMultiplyUsers([FromBody] List<UserModel> userModels) 
        {
            if(userModels != null && userModels.Count > 0) 
            {
                bool result = _usersService.CreateMultipleUsers(userModels);
                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }
        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUser(int id)
        {
            var user = _usersService.Get(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
   
}
