using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Common.Models
{
    public class ProjectModel : CommonModel // наследуемся от CommonModel
    {
        // Такие же свойста как и у модели Project
        // Создадим CommonModel
        public ProjectStatus Status { get; set; }
        public int? AdminId { get; set; }
        public List<int>? AllUsersIds { get; set; } = null;
        public List<int>? AllDesksIds { get; set; } = null; 
        public ProjectModel() 
        {
        }
        public ProjectModel(string name, string description, ProjectStatus status) 
        {
            Name = name;
            Description = description;
            Status = status;
            
        }
    }
}
