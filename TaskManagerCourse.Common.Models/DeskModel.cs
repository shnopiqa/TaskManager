using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Common.Models
{
    public class DeskModel : CommonModel // от CommonModel model
    {
        public bool IsPrivate { get; set; }
        public string[]? Columns { get; set; } = null;
        public int ProjectId
        {
            get; set;
        }
        public int AdminId { get; set; }
        public List<int>? TasksIds { get; set; } = null;  // TaskModel для задач

        public DeskModel() { }
        public DeskModel(string name, string description, bool isPrivate, string[]? columns) 
        {
            Name = name;
            Description = description;
            IsPrivate = isPrivate;
            Columns = columns;
        }
    }
}
