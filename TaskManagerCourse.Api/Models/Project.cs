using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Api.Models.Abstractions;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class Project : CommonObject
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public ProjectAdmin Admin { get; set; }
        public List<User> AllUsers { get; set; } = new List<User>();
        public List<Desk> AllDesks { get; set; } = new List<Desk>();
        public ProjectStatus Status { get; set; }
        public byte[]? Photo { get; set; } = null;
        public Project() { }
        // В CommonObject.cs необходимо создать констуркт который применяет CommonModel 
        public Project(ProjectModel projectModel) :base(projectModel) // Обращаемся к базовому классу ProjectModel
        {
            Id = projectModel.Id;
            AdminId = projectModel.AdminId;
            Status = projectModel.Status;

        }
        public ProjectModel ToDto() 
        {
            return new ProjectModel()
            {
                Id=this.Id,
                Name=this.Name,
                CreationDate=this.CreationDate,
                Photo = this.Photo,
                AdminId=this.AdminId,
                Status=this.Status,
                Description = this.Description

            };
        }
    }
}
