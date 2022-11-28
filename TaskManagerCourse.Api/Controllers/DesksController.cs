using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;
using TaskManagerCourse.Common.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesksController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UsersService _usersService;
        private readonly DesksService _desksService;
        public DesksController(ApplicationContext db)
        {
            _db = db;
            _usersService = new UsersService(db);
            _desksService = new DesksService(db);    
           
        }
        // GET: api/<DesksController>
        [HttpGet]
        public  async Task<IEnumerable<CommonModel>> GetDeskForCurrentUser()
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if(user != null) 
            {
                return await _desksService.GetAll(user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }

        // GET api/<DesksController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var desk = _desksService.Get(id);
            return desk == null ? NotFound() : Ok(desk);
        }
        // GET api/<DesksController>/5
        [HttpGet("project")]  
        public async Task<IEnumerable<CommonModel>> GetProjectDesks(int projectId)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if(user != null) 
            {
                return await _desksService.GetProjectDesks(projectId, user.Id).ToListAsync();
            }
            return Array.Empty<CommonModel>();
        }
        // POST api/<DesksController>
        [HttpPost]
        public IActionResult Create([FromBody] DeskModel deskModel)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if(user != null) 
            {
                if(deskModel != null) 
                {
                    deskModel.AdminId = user.Id;
                    bool resutl = _desksService.Create(deskModel);
                    return resutl? Ok(resutl) : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        // PUT api/<DesksController>/5
        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] DeskModel deskModel)
        {
            var user = _usersService.GetUser(HttpContext.User.Identity.Name);
            if (user != null)
            {
                if (deskModel != null)
                {
                    bool resutl = _desksService.Update(id, deskModel);
                    return resutl ? Ok(resutl) : NotFound();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        // DELETE api/<DesksController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool resutl = _desksService.Delete(id);
            return resutl ? Ok(resutl) : NotFound();
        }
    }
}
