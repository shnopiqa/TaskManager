using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Api.Models
{
    public class ProjectAdmin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
        public ProjectAdmin() { }
        public ProjectAdmin(User user)
        {
            UserId = user.Id;
            User = user;
        }

    }
}
