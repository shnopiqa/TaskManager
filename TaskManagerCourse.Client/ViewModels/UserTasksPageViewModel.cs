using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
namespace TaskManagerCourse.Client.ViewModels
{
    public class UserTasksPageViewModel : BindableBase
    {
        private AuthToken _token;
        private TasksRequestService _tasksRequestService;
        private UserRequestService _usersRequestService;
        public UserTasksPageViewModel(AuthToken token)
        {
            _token = token;
            _tasksRequestService = new TasksRequestService();
            _usersRequestService = new UserRequestService();
        }
        public List<TaskClient> AllTasks
        {
            get => _tasksRequestService.GetAllTasks(_token).Select(
                task =>
                {
                    var taskClien = new TaskClient(task);
                    if (task.CreatorId != null)
                    {
                        int CreatorId = (int)task.CreatorId;
                        taskClien.Creator = _usersRequestService.GetUserById(_token, task.CreatorId);

                    }
                    if (task.ExecutorId != null)
                    {
                        int ExecutorId = (int)task.ExecutorId;
                        taskClien.Executor = _usersRequestService.GetUserById(_token, task.ExecutorId);

                    }
                    return taskClien;


                }).ToList();
        }

    }
}
