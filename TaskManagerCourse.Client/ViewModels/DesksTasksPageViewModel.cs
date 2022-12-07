using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Client.Views.AddWindows;
using TaskManagerCourse.Client.Views.Components;
using TaskManagerCourse.Client.Views.Pages;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class DesksTasksPageViewModel : BindableBase
    {
        private AuthToken _token;
        private UserRequestService _usersRequestService;
        private DeskModel _desk;
        private TasksRequestService _tasksRequestService;
        private ProjectsRequestService _projectsRequestService;
        private CommonViewService _viewService;
        private DeskTasksPage _page;
        #region COMMANDS
        public DelegateCommand OpenNewTaskCommand { get; private set; }
        public DelegateCommand OpenUpdateTaskCommand { get; private set; }
        public DelegateCommand CreateOrUpdateTaskCommand { get; private set; }
        public DelegateCommand DeleteTaskCommand { get; private set; }

        #endregion
        public DesksTasksPageViewModel(AuthToken token, DeskModel desk, DeskTasksPage page)
        {
            _desk = desk;
            _token = token;
            _page = page;
            _usersRequestService = new UserRequestService();
            _tasksRequestService = new TasksRequestService();
            _projectsRequestService = new ProjectsRequestService();
            _viewService = new CommonViewService();
            TasksByColumns = GetTasksByColumns(_desk.Id);
            _page.TasksGrid.Children.Add(CreateTaskGrid());
            OpenNewTaskCommand = new DelegateCommand(OpenNewTask);
            OpenUpdateTaskCommand = new DelegateCommand(OpenUpdateTask);
            CreateOrUpdateTaskCommand = new DelegateCommand(CreateOrUpdateTasks);
            DeleteTaskCommand = new DelegateCommand(DeleteTask);

        }
        #region PROPERTIES
        private Dictionary<string, List<TaskClient>> _tasksByColumns;

        public Dictionary<string, List<TaskClient>> TasksByColumns
        {
            get { return _tasksByColumns; }
            set 
            {
                _tasksByColumns = value; 
                RaisePropertyChanged(nameof(TasksByColumns));
            }
        }

        private TaskClient _selectedTask;

        public TaskClient SelectedTask
        {
            get { return _selectedTask; }
            set 
            {
                _selectedTask = value;
                RaisePropertyChanged(nameof(SelectedTask));
            }
        }
        private ModelClientAction _typeActionWithTask;

        public ModelClientAction TypeActionWithTask
        {
            get { return _typeActionWithTask; }
            set { 
                _typeActionWithTask = value;
                RaisePropertyChanged(nameof(TypeActionWithTask));
            }
        }

        private UserModel _selectedTaskExecutor;

        public UserModel SelectedTaskExecutor
        {
            get { return _selectedTaskExecutor; }
            set { _selectedTaskExecutor = value;
                RaisePropertyChanged(nameof(SelectedTaskExecutor));
            }
        }
        private ProjectModel Project 
        {
            get => _projectsRequestService.GetProjectById(_token, _desk.ProjectId);
        }
        public List<UserModel> AllProjectUsers 
        {
            get => Project?.AllUsersIds?.Select(userId => _usersRequestService.GetUserById(_token, userId)).ToList();
        }

        private string _selectedColumnName;
        public string SelectedColumnName 
        {
            get => _selectedColumnName; 
            set 
            {
                _selectedColumnName = value;
                RaisePropertyChanged(nameof(SelectedColumnName));
            }
        }
        #endregion
        #region METHODS

        private Grid CreateTaskGrid() 
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri("./Resources/Styles/MainStyle.xaml", UriKind.Relative);
            Grid grid = new Grid();
            var row0 = new RowDefinition();
            row0.Height = new GridLength(30);

            var row1 = new RowDefinition();
            grid.RowDefinitions.Add(row0);
            grid.RowDefinitions.Add(row1);

            int columnCount = 0;
            foreach(var column in TasksByColumns) 
            {
                var col = new ColumnDefinition();
                grid.ColumnDefinitions.Add(col);

                //header
                TextBlock header = new TextBlock();
                header.Text = column.Key;
                header.Style = resource["headerTBlock"] as Style;
                Grid.SetRow(header, 0);
                Grid.SetColumn(header, columnCount);

                grid.Children.Add(header);

                // Column 

                ItemsControl columnControl = new ItemsControl();
                columnControl.Style = resource["tasksCommonPanel"] as Style;
                columnControl.Tag = column.Key;
                columnControl.MouseEnter += new System.Windows.Input.MouseEventHandler((sender, e) =>
                {
                    GetSelectedColumn(sender);
                });
                //columnControl.MouseEnter += new System.Windows.Input.MouseEventHandler((s, e) => SendTaskToNewColumn());
                columnControl.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler((s, e) => SendTaskToNewColumn());
                Grid.SetRow(columnControl, 1);
                Grid.SetColumn(columnControl, columnCount);
                var tasksViews = new List<TaskControl>();
                foreach (var task in column.Value) 
                {
                    var taskView = new TaskControl(task);
                    taskView.MouseDown += new System.Windows.Input.MouseButtonEventHandler((s, e) =>
                    {
                        SelectedTask = task;
                    });
                    //taskView.MouseLeave += new System.Windows.Input.MouseEventHandler((s, e) => SendTaskToNewColumn());
                    tasksViews.Add(taskView);
                }
                columnControl.ItemsSource = tasksViews;
                grid.Children.Add(columnControl);
                columnCount++;
            }
            return grid;
        }

        private void CreateOrUpdateTasks()
        {
            if (TypeActionWithTask == ModelClientAction.Create)
            {

              
                CreateTask();
            }
            if (TypeActionWithTask == ModelClientAction.Update)
            {
                UpdateTask();
            }
            UpdatePage();

        }
        private Dictionary<string, List<TaskClient>> GetTasksByColumns(int deskId) 
        {
            var tasksByColumns = new Dictionary<string, List<TaskClient>>();
            var allTasks = _tasksRequestService.GetTasksByDesk(_token, deskId);
            foreach(string column in _desk.Columns) 
            {

                tasksByColumns.Add(column, allTasks
                    .Where(t => t.Column == column)
                    .Select(t =>
                    {
                        var taskClient = new TaskClient(t);
                        taskClient.Creator = _usersRequestService.GetCurrentUser(_token);
                        if(t.ExecutorId != null)
                            taskClient.Executor = _usersRequestService.GetUserById(_token, t.ExecutorId);

                        return taskClient;
                    }
                    ).ToList());
            }
            return tasksByColumns;
        }

        private void CreateTask()
        { 
            SelectedTask.Model.DeskId = _desk.Id;
            SelectedTask.Model.ExecutorId = SelectedTaskExecutor.Id;
            SelectedTask.Model.Column = _desk.Columns.FirstOrDefault();
            var resultAction = _tasksRequestService.CreateTask(_token, SelectedTask.Model);
            _viewService.ShowActionResult(resultAction, "deks is created");

        }
        private void UpdateTask()
        {

            _tasksRequestService.UpdateTask(_token,SelectedTask.Model);

        }


        private void DeleteTask()
        {
            _tasksRequestService.DelettTasksById(_token, SelectedTask.Model.Id);
            UpdatePage();
        }
        private void OpenNewTask() 
        {
            SelectedTask = new TaskClient(new TaskModel());
            TypeActionWithTask = ModelClientAction.Create;
            var wnd = new CreateOrUpdateTaskWindow();
            _viewService.OpenWindow(wnd, this);
        }
        private void OpenUpdateTask()
        {
            TypeActionWithTask = ModelClientAction.Update;
            var wnd = new CreateOrUpdateTaskWindow();
            _viewService.OpenWindow(wnd, this);
        }
        private void UpdatePage() 
        {
            SelectedTask = null;
            
            TasksByColumns = GetTasksByColumns(_desk.Id);
            _page.TasksGrid.Children.Add(CreateTaskGrid());
            _viewService.CurrentOpenedWindow?.Close();
        }
        private void GetSelectedColumn(object senderControl) 
        {
            SelectedColumnName = ((ItemsControl)senderControl).Tag.ToString();
            
        }
        private void SendTaskToNewColumn() 
        {
            if (SelectedTask != null && SelectedTask?.Model?.Column != SelectedColumnName) 
            {
                SelectedTask.Model.Column = SelectedColumnName;
                _tasksRequestService.UpdateTask(_token, SelectedTask.Model);
                UpdatePage();
                SelectedTask = null;
            }
        }
        #endregion
    }
}
