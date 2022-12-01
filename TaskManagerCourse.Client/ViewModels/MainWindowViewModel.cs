using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Client.Views;
using TaskManagerCourse.Client.Views.Pages;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private CommonViewService _viewService;
        #region COMMANDS
        public DelegateCommand OpenMyProjectsPageCommand { get; private set; }
        public DelegateCommand OpenMyDesksPageCommand { get; private set; }
        public DelegateCommand OpenMyTasksPageCommand { get; private set; }
        public DelegateCommand OpenMyInfoPageCommand { get; private set; }
        public DelegateCommand LogOutCommand { get; private set; }

        public DelegateCommand OpenUsersManagmentCommand;

        #endregion

        public MainWindowViewModel(AuthToken token, UserModel currentUser, Window currentWindow = null) 
        {
            _viewService = new CommonViewService();
            Token = token;
            CurrentUser = currentUser;
            _currentWindow = currentWindow;
            OpenMyInfoPageCommand = new DelegateCommand(OpenMyInfoPage);
            NavButtons.Add(_userInfoBtnName, OpenMyInfoPageCommand);

            OpenMyProjectsPageCommand = new DelegateCommand(OpenMyProjectsPage);
            NavButtons.Add(_userProjectsBtnName, OpenMyProjectsPageCommand);

            OpenMyDesksPageCommand = new DelegateCommand(OpenMyDesksPage);
            NavButtons.Add(_userDesksBtnName, OpenMyDesksPageCommand);

            OpenMyTasksPageCommand = new DelegateCommand(OpenMyTasksPage);
            NavButtons.Add(_userTasksBtnName, OpenMyTasksPageCommand);

            if(CurrentUser.Status == UserStatus.Admin) 
            {
                OpenUsersManagmentCommand = new DelegateCommand(OpenUsersManagment);
                NavButtons.Add(_manageUsersBtnName, OpenUsersManagmentCommand);
            }

            LogOutCommand = new DelegateCommand(LogOut);
            NavButtons.Add(_logOutBtnName, LogOutCommand);

            OpenMyInfoPage();

    }
        #region PROPERTIES

        private readonly string _userProjectsBtnName = "My projects";
        private readonly string _userDesksBtnName = "My desks";
        private readonly string _userTasksBtnName = "My tasks";
        private readonly string _userInfoBtnName = "My Info";
        private readonly string _logOutBtnName = "Logout";

        private readonly string _manageUsersBtnName = "Users";

        private Window _currentWindow;
        private UserModel _currentUser;

        public UserModel CurrentUser
        {
            get => _currentUser;
            private set 
            { 
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }


        private AuthToken _token;

        public AuthToken Token
        {
            get => _token; 
            private set 
            { 
                _token = value;
                RaisePropertyChanged(nameof(Token));
            }
        }

        private Dictionary<string, DelegateCommand> _navButtons = new Dictionary<string, DelegateCommand>();

        public Dictionary<string, DelegateCommand> NavButtons
        {
            get => _navButtons;

            set 
            { 
                _navButtons = value;
                RaisePropertyChanged(nameof(NavButtons));
            }
        }

        
        private string _selectedPageName;

        public string SelectedPageName
        {
            get { return _selectedPageName; }
            set 
            {
                _selectedPageName = value;
                RaisePropertyChanged(nameof(SelectedPageName));
            }
        }

        private Page _selectedPage;
        public Page SelectedPage
        {
            get => _selectedPage; 
            set 
            { 
                _selectedPage = value;

                RaisePropertyChanged(nameof(SelectedPage));
            }
        }

        #endregion

        #region METHODS

        private void OpenMyProjectsPage()
        {

            var page = new ProjectsPage();
            OpenPage(page, _userProjectsBtnName, new PorjectsPageViewModel(Token, this));
         
        }
        private void OpenMyDesksPage()
        {
            var page = new UserDesksPage();
            OpenPage(page, _userDesksBtnName, new UserDeskPageViewModel(Token));
        }
        private void OpenMyTasksPage()
        {
            var page = new UserTasksPage();
            OpenPage(page, _userTasksBtnName, new UserTasksPageViewModel(Token));
        }
        private void OpenMyInfoPage() 
        {
            var page = new UserInfoPage();
            OpenPage(page, _userInfoBtnName, this);
        }
        private void LogOut()
        {

            var question = MessageBox.Show("Are you shure?", "LogOut", MessageBoxButton.YesNo);
            if(question == MessageBoxResult.Yes && _currentWindow != null) 
            {
                Login login = new Login();
                login.Show();
                _currentWindow.Close();
            }
        }
        private void OpenUsersManagment()
        {
            SelectedPageName = _manageUsersBtnName;
            _viewService.ShowMessage(_manageUsersBtnName);
        }


        #endregion

        
        public void OpenPage(Page page, string pageName, BindableBase viewmodel) 
        {
            SelectedPageName = pageName;
            SelectedPage = page;
            SelectedPage.DataContext = viewmodel;
        } 
        
    }
}
