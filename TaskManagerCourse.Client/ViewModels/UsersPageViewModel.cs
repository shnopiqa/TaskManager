using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class UsersPageViewModel : BindableBase
    {
        private UserRequestService _userRequestService;
        private TasksRequestService _tasksRequestService;
        private CommonViewService _viewService;
        private AuthToken _token;
        #region COMMANDS 
        public DelegateCommand<object> OpenUpdateUserCommand { get; private set; }
        public DelegateCommand OpenNewUserCommand { get; private set; }
        public DelegateCommand<object> DeleteUserCommand { get; private set; }
        public DelegateCommand CreateOrUpdateUserCommand { get; private set; }
        public DelegateCommand OpenSelectUsersFromExcelCommand { get; private set; }
        public DelegateCommand GettUsersFromExcelCommand { get; private set; }
        public DelegateCommand AddUsersFromExcelCommand { get; private set; }
        #endregion 
        public UsersPageViewModel(AuthToken token) 
        {
            _token = token;
            _userRequestService = new UserRequestService();
            _tasksRequestService = new TasksRequestService();
            _viewService = new CommonViewService();

            OpenUpdateUserCommand = new DelegateCommand<object>(OpenUpdateUser);
            OpenNewUserCommand = new DelegateCommand(OpenNewUser);
            DeleteUserCommand = new DelegateCommand<object>(DeleteUser);
            CreateOrUpdateUserCommand = new DelegateCommand(CreateOrUpdateUser);
            OpenSelectUsersFromExcelCommand = new DelegateCommand(OpenSelectUsersFromExcel);
            GettUsersFromExcelCommand = new DelegateCommand(GettUsersFromExcel);
            AddUsersFromExcelCommand = new DelegateCommand(AddUsersFromExcel);
        }

        #region PROPERTIES
        public List<UserModel> AllUsers 
        {
            get => _userRequestService.GetAllUsers(_token);
        }
        private List<UserModel> _usersFromExcel;

        public List<UserModel> UsersFromExcel
        {
            get { return _usersFromExcel; }
            set 
            {
                _usersFromExcel = value;
                RaisePropertyChanged(nameof(UsersFromExcel));
            }
        }


        private UserModel _selectedUsersFromExcel;

        public UserModel SelectedUsersFromExcel
        {
            get { return _selectedUsersFromExcel; }
            set 
            { 
                _selectedUsersFromExcel = value;
                RaisePropertyChanged(nameof(SelectedUsersFromExcel));
            }
        }

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get { return _selectedUser; }
            set 
            {
                _selectedUser = value;
                RaisePropertyChanged(nameof(SelectedUser));
            }
        }



        #endregion
        #region METHODS
        private void OpenUpdateUser(object userId)
        {

        }
        private void OpenNewUser() 
        {

        }
        private void DeleteUser(object userId)
        {

        }
        private void CreateOrUpdateUser()
        {

        }
        
        private void OpenSelectUsersFromExcel()
        {

        }
        private void GettUsersFromExcel()
        {

        }
        private void AddUsersFromExcel() 
        {

        }
        #endregion
    }
}
