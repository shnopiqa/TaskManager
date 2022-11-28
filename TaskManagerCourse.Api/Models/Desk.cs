using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Api.Models.Abstractions;
using TaskManagerCourse.Common.Models;
using Newtonsoft.Json;
namespace TaskManagerCourse.Api.Models
{
    public class Desk : CommonObject
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Columns { get; set; }
        public int AdminId { get; set; }
        public User Admin { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();

        public Desk() { }
        public Desk(DeskModel deskModel) : base(deskModel) // Обращаемся к базовому классу ProjectModel
        {
            Id = deskModel.Id;
            AdminId = deskModel.AdminId;
            IsPrivate = deskModel.IsPrivate;
            if (deskModel.Columns.Any())
                Columns = JsonConvert.SerializeObject(deskModel.Columns);

            ProjectId = deskModel.ProjectId;
        }
        public DeskModel ToDto()
        {
            return new DeskModel()
            {
                Id = this.Id,
                Name = this.Name,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
                AdminId = this.AdminId,
                Description = this.Description,
                IsPrivate = this.IsPrivate,
                Columns = JsonConvert.DeserializeObject<string[]>(this.Columns),
                ProjectId = this.ProjectId

            };

        }
        public CommonModel ToShortDto()
        {
            return new CommonModel()
            {
                Id = this.Id,
                Name = this.Name,
                CreationDate = this.CreationDate,
                Photo = this.Photo,
                Description = this.Description,

            };

        }



    }
}

