using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using TaskManagerCourse.Client.Models;
using TaskManagerCourse.Client.Services;
using TaskManagerCourse.Client.Views;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        UserRequestService _userRequestService;

        #region COMMANDS
        public DelegateCommand<object> GetUserFromDBCommand { get; private set; }
        public DelegateCommand<object> LoginFromCacheCommand { get; private set; }
        #endregion

        #region PROPERTIES
        private string _cachePath = Path.GetTempPath() + "usertaskmanagercourse.txt";
        private Window _currentWnd;
        public LoginViewModel()
        {
            CurrentUserCache = GetUserCache();

            _userRequestService = new UserRequestService();
            GetUserFromDBCommand = new DelegateCommand<object>(GetUserFromDb);
            LoginFromCacheCommand = new DelegateCommand<object>(LoginFromCache);
        }
        public string UserLogin { get; set; }
        public string UserPassword { get; private set; }

        private UserCache _currentUserCache;

        public UserCache CurrentUserCache
        {
            get => _currentUserCache;
            set
            {
                _currentUserCache = value;
                RaisePropertyChanged(nameof(CurrentUserCache));
            }
        }

        private UserModel _curentUser;
        public UserModel CurrentUser
        {
            get => _curentUser;
            set
            {
                _curentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }
        }

        private AuthToken _authToken;
        public AuthToken AuthToken
        {
            get => _authToken;
            set
            {
                _authToken = value;
                RaisePropertyChanged(nameof(AuthToken));
            }
        }
        #endregion

        #region GetUserFromDB
        private void GetUserFromDb(object parameter)
        {
            var passbox = parameter as PasswordBox;
            bool isNewUser = false;
            _currentWnd = Window.GetWindow(passbox);

            if (UserLogin != CurrentUserCache?.Login ||
                UserPassword != CurrentUserCache?.Password)
            {
                isNewUser = true;
            }

            UserPassword = passbox.Password;
            AuthToken = _userRequestService.GetToken(UserLogin, UserPassword);
            if (AuthToken == null)
                return;

            CurrentUser = _userRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                if (isNewUser)
                {
                    var saveUserCacheMessage = MessageBox.Show("Хотите сохранить логин и пароль?",
                        "Сохранение данных", MessageBoxButton.YesNo);
                    if (saveUserCacheMessage == MessageBoxResult.Yes)
                    {
                        UserCache newUserCache = new UserCache
                        {
                            Login = UserLogin,
                            Password = UserPassword

                        };
                        CreateUserCache(newUserCache);
                    }
                    OpenMainWindow();
                }
                
            }

        }

        private void CreateUserCache(UserCache userCache)
        {
            string jsonUserCache = JsonConvert.SerializeObject(userCache);
            using (StreamWriter sw = new StreamWriter(_cachePath, false, Encoding.Default))
            {
                sw.Write(jsonUserCache);
                MessageBox.Show("Успех!");
            }
        }
        private UserCache GetUserCache()
        {
            bool isCacheExist = File.Exists(_cachePath);

            if (isCacheExist && File.ReadAllText(_cachePath)?.Length > 0)
            {
                return JsonConvert.DeserializeObject<UserCache>(File.ReadAllText(_cachePath));
            }
            return null;
        }

        private void LoginFromCache(object wnd)
        {
            _currentWnd = wnd as Window;
            UserLogin = CurrentUserCache.Login;
            UserPassword = CurrentUserCache.Password;
            AuthToken = _userRequestService.GetToken(UserLogin, UserPassword);
            CurrentUser = _userRequestService.GetCurrentUser(AuthToken);
            if (CurrentUser != null)
            {
                OpenMainWindow();
            }

        }
        private void OpenMainWindow()
        {
            MainWindow window = new MainWindow();
            window.DataContext = new MainWindowViewModel(AuthToken, CurrentUser, window);
            window.Show();
            _currentWnd.Close();
        }
        #endregion
    }
}
