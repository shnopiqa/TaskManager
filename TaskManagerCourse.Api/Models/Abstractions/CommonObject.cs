using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models.Abstractions
{
    public class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[]? Photo { get; set; } = null;
        public CommonObject()
        { }
        public CommonObject(CommonModel model)
        {
            Name = model.Name;
            Description = model.Description;
            CreationDate = DateTime.Now;
            Photo = model.Photo;
        }


    }
}
